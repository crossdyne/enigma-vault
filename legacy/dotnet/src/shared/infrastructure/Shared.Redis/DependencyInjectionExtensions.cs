using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Common.Cache.Abstractions;
using StackExchange.Redis;

namespace Shared.Redis
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCashService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisOptions>(option => configuration.GetSection(RedisOptions.SectionName));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionRedisString = configuration["Redis:ConnectionString"];
                
                var config = ConfigurationOptions.Parse(connectionRedisString!);

                config.AbortOnConnectFail = false;
                config.ConnectRetry = 3;
                config.ConnectTimeout = 5000;

                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}