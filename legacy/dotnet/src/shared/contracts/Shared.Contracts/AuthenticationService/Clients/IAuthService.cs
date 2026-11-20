using Crossdyne.Toolkit.Results;
using Shared.Contracts.AuthenticationService.Requests;
using Shared.Contracts.AuthenticationService.Responses;

namespace Shared.Contracts.AuthenticationService.Clients
{
    public interface IAuthService
    {
        Task<Result<SrpChallengeResponse>> GetSrpChallenge(SrpChallengeRequest request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> VerifySrpProof(SrpVerifyRequest request, CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> RefreshTokens(LoginByTokenRequest request, CancellationToken cancellationToken = default);
    }
}