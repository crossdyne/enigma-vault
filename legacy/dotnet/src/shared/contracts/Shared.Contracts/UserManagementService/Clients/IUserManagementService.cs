using Crossdyne.Toolkit.Results;
using Shared.Contracts.AuthenticationService.Responses;
using Shared.Contracts.UserManagementService.Responses;

namespace Shared.Contracts.UserManagementService.Clients
{
    public interface IUserManagementService
    {
        Task<Result<UserResponse>> Me(string accessToken, CancellationToken cancellationToken = default);
        Task<Result<UserPublicInfo>> GetPublicEncryptionInfo(string confirmationToken, CancellationToken cancellationToken = default);
        Task<Result<DekResponse>> GetDek(CancellationToken cancellationToken = default);
    }
}