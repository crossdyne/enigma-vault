using System.Text.Json;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Http.Extensions;

namespace Shared.Http
{
    public abstract class HttpService(HttpClient http, JsonSerializerOptions? jsonOptions = null)
    {
        protected readonly HttpClient _http = http;
        protected readonly JsonSerializerOptions _jsonOptions = jsonOptions ?? new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        protected async Task<Result<TResponse>> CatchResponseAsync<TResponse>(Func<CancellationToken, Task<HttpResponseMessage>> func, CancellationToken ct = default) 
            => await HttpResult.CatchResponseAsync<TResponse>(func, _jsonOptions, ct);

        protected async Task<Result<Unit>> CatchAsync(Func<CancellationToken, Task<HttpResponseMessage>> func, CancellationToken ct = default)
            => await HttpResult.CatchAsync(func, ct);
    }
}