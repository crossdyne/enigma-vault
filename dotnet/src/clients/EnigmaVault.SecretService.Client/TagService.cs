using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.SecretService.Clients;
using Shared.Contracts.SecretService.Requests;
using Shared.Contracts.SecretService.Responses;
using Shared.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.SecretService.Client
{
    public sealed class TagService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), ITagService
    {
        private string _url = "api/tags";

        public async Task<Result<List<TagResponse>>> GetAll(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<TagResponse>>(async ct => await _http.GetAsync($"{_url}", ct), cancellationToken);

        public async Task<Result<CreateTagResponse>> CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<CreateTagResponse>(async ct => await _http.PostAsJsonAsync(_url, request, _jsonOptions, ct), cancellationToken);

        public async Task<Result<Unit>> DeleteAsync(string id, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.DeleteAsync($"{_url}/{id}", ct), cancellationToken);

        public async Task<Result<Unit>> UpdateAsync(UpdateTagRequest request, CancellationToken cancellationToken = default)
            => await CatchAsync(async ct => await _http.PatchAsJsonAsync(_url, request, _jsonOptions), cancellationToken);
    }
}