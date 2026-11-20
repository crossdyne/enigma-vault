using System.Net.Http.Json;
using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.FileService.Clients;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;
using Shared.Http;

namespace EnigmaVault.FileService.Client
{
    public sealed class FileStorageService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IFileService
    {
        public async Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<BatchUrlResponse>(async ct => await _http.PostAsJsonAsync("api/files/urls", request, ct), cancellationToken);
    }
}