using Crossdyne.Toolkit.Results;
using Shared.Contracts.AssetsService.Responses;

namespace Shared.Contracts.AssetsService.Clients
{
    public interface IAssetCategoryService
    {
        Task<Result<List<IconCategoryResponse>>> GetIconCategories(CancellationToken cancellationToken = default);
    }
}