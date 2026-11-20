using EnigmaVault.Desktop.Models;
using EnigmaVault.Desktop.Services.Managers;
using EnigmaVault.Desktop.Services.Secure;
using Shared.Contracts.AuthenticationService.Clients;
using Shared.Contracts.AuthenticationService.Requests;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EnigmaVault.Desktop.Handlers
{
    internal class RefreshTokenHandler(
        ITokenManager tokenStorage,
        IAuthService authService,
        IAuthenticationStateService authenticationStateService) : DelegatingHandler
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokensMaybe = tokenStorage.GetTokens();

            var tokens = tokensMaybe.Value;
            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _semaphore.WaitAsync(cancellationToken);

                try
                {
                    var currentTokensMaybe = tokenStorage.GetTokens();

                    var currentTokens = currentTokensMaybe.Value;
                    var currentAccessToken = currentTokens.AccessToken;
                    var currentRefreshToken = currentTokens.RefreshToken;

                    if (currentAccessToken != accessToken && !string.IsNullOrWhiteSpace(currentAccessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", currentAccessToken);
                        return await base.SendAsync(request, cancellationToken);
                    }

                    if (string.IsNullOrWhiteSpace(refreshToken))
                    {
                        tokenStorage.ClearTokens();
                        authenticationStateService.NotifyAuthenticationRequired();
                        return response;
                    }

                    var refreshResult = await authService.RefreshTokens(new LoginByTokenRequest(refreshToken));

                    if (refreshResult.IsSuccess && refreshResult.Value is not null)
                    {
                        tokenStorage.SaveTokens(new AccessData(refreshResult.Value.AccessToken, refreshResult.Value.RefreshToken));

                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshResult.Value.AccessToken);
                        return await base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        tokenStorage.ClearTokens();
                        authenticationStateService.NotifyAuthenticationRequired();
                        return response;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return response;
        }
    }
}