using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.SecretService.Requests;
using Shared.Contracts.SecretService.Responses;

namespace Shared.Contracts.SecretService.Clients
{
    public interface ITagService
    {
        Task<Result<List<TagResponse>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<CreateTagResponse>> CreateAsync(CreateTagRequest request, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> UpdateAsync(UpdateTagRequest request, CancellationToken cancellationToken = default);
    }
}