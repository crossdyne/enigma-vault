using Microsoft.Extensions.Options;
using Shared.Contracts.Common.Cache.Abstractions;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Redis
{
    internal sealed class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly RedisOptions _options;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public RedisCacheService(IConnectionMultiplexer redis, IOptions<RedisOptions> options)
        {
            _redis = redis;
            _options = options.Value;
            _database = _redis.GetDatabase(_options.Database);
        }

        #region Ключ-значение в виде JSON   

        public async Task<bool> SetJsonAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return await ExecuteWithRetryAsync(async () =>
            {
                var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
                var result = await _database.StringSetAsync(key, json, expiry, false);

                return result;
            },
            "SerJsonAsync");
        }

        public Task<T?> GetJsonAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            return ExecuteWithRetryAsync(async () => 
            {
                var json = await _database.StringGetAsync(key);

                if (!json.HasValue)
                    return default;

                try
                {
                    var result = JsonSerializer.Deserialize<T>(json!.ToString(), _jsonSerializerOptions);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException($"Failed to deserialize JSON for key {key}", ex);
                }
            },
            "GetJsonAsync");
        }

        #endregion

        #region Удаление по ключу

        public async Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            return await ExecuteWithRetryAsync(async () =>
            {
                var result = await _database.KeyDeleteAsync(key);

                return result;
            }, "RemoveAsync");
        }
        
        public async Task<bool> SetRemoveAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or empty", nameof(value));

            return await ExecuteWithRetryAsync(async () => await _database.SetRemoveAsync(key, value), "SetRemoveAsync");
        }

        #endregion

        #region Вспомогательные методы

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName)
        {
            try
            {
                return await operation();
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"Redis connection error during {operationName}: {ex.Message}");
                throw new ArgumentException($"Redis connection failed during {operationName}", ex);
            }
            catch (RedisTimeoutException ex)
            {
                Console.WriteLine($"Redis timeout during {operationName}: {ex.Message}");
                throw new ArgumentException($"Redis timeout during {operationName}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine( $"Unexpected error during{operationName}: {ex.Message}");
                throw new ArgumentException($"Unexpected error during {operationName}", ex);
            }
        }

        #endregion
    }
}