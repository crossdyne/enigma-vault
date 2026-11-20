using Crossdyne.Toolkit.Results;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;

namespace Shared.Contracts.FileService.Clients
{
    public interface IFileService
    {
        Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request, CancellationToken cancellationToken = default);
    }
}