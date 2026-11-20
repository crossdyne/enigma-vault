using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.AssetsService.Clients;
using Shared.Contracts.AssetsService.Responses;
using Shared.Http;

namespace EnigmaVault.AssetsService.Client
{
    public sealed class AssetCategoryService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IAssetCategoryService
    {        
        public async Task<Result<List<IconCategoryResponse>>> GetIconCategories(CancellationToken cancellationToken = default)
            => await CatchResponseAsync<List<IconCategoryResponse>>(async ct => await _http.GetAsync($"api/v1/aggregated/category", ct), cancellationToken);
    }
}