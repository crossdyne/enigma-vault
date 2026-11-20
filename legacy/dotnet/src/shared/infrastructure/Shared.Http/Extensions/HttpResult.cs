using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Kernel.Errors;

namespace Shared.Http.Extensions;

public static class HttpResult
{
    public static async Task<Result<TResponse>> CatchResponseAsync<TResponse>(Func<CancellationToken, Task<HttpResponseMessage>> func, JsonSerializerOptions options, CancellationToken ct = default)
    {
        HttpResponseMessage? response = null;
        try
        {
            response = await func(ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                return new Error(AppErrors.ApiError, $"HTTP {(int)response.StatusCode}: {errorBody}");
            }

            if (typeof(TResponse) == typeof(Stream))
            {
                var memory = new MemoryStream();
                await response.Content.CopyToAsync(memory, ct);
                memory.Position = 0;

                return (TResponse)(object)memory;
            }

            if (typeof(TResponse) == typeof(string))
            {
                var text = await response.Content.ReadAsStringAsync(ct);
                return (TResponse)(object)text;
            }

            if (typeof(TResponse) == typeof(byte[]))
            {
                var bytes = await response.Content.ReadAsByteArrayAsync(ct);
                return (TResponse)(object)bytes;
            }

            if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
                return new Error(AppErrors.ApiError, $"Пустое тело ответа при ожидаемом типе {typeof(TResponse).Name}");

            var json = await response.Content.ReadFromJsonAsync<TResponse>(options, ct);
            return json!;
        }
        catch (OperationCanceledException ex) when (ct.IsCancellationRequested)
        {
            return new Error(AppErrors.RequestCancelled, ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            return new Error(AppErrors.ApiError, $"Timeout: {ex.Message}");
        }
        catch (JsonException ex)
        {
            return new Error(AppErrors.ApiError, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return new Error(AppErrors.ApiError, ex.Message);
        }
        finally
        {
            response?.Dispose();
        }
    }

    public static async Task<Result<Unit>> CatchAsync(Func<CancellationToken, Task<HttpResponseMessage>> func, CancellationToken ct = default)
    {
        try
        {
            using var response = await func(ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                return new Error(AppErrors.ApiError, $"HTTP {(int)response.StatusCode}: {errorBody}");
            }

            return Unit.Value;
        }
        catch (OperationCanceledException ex) when (ct.IsCancellationRequested)
        {
            return new Error(AppErrors.RequestCancelled, ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            return new Error(AppErrors.ApiError, $"Timeout: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            return new Error(AppErrors.ApiError, ex.Message);
        }
    }
}