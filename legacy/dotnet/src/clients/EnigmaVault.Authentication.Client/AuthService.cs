using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Options;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.AuthenticationService.Requests;
using Shared.Contracts.AuthenticationService.Responses;
using Shared.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EnigmaVault.Authentication.Client
{
    public sealed class AuthService(HttpClient http, IOptions<JsonSerializerOptions> options) : HttpService(http, options.Value), IAuthService
    {
        public async Task<Result<SrpChallengeResponse>> GetSrpChallenge(SrpChallengeRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<SrpChallengeResponse>(async ct => await _http.PostAsJsonAsync("api/auth/srp/challenge", request, _jsonOptions, ct), cancellationToken);

        public async Task<Result<AuthResponse>> VerifySrpProof(SrpVerifyRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<AuthResponse>(async ct => await _http.PostAsJsonAsync("api/auth/srp/verify", request, _jsonOptions, ct),  cancellationToken);

        public async Task<Result<AuthResponse>> RefreshTokens(LoginByTokenRequest request, CancellationToken cancellationToken = default)
            => await CatchResponseAsync<AuthResponse>(async ct => await _http.PostAsJsonAsync("api/auth/refresh", request, _jsonOptions, ct), cancellationToken);
    }
}