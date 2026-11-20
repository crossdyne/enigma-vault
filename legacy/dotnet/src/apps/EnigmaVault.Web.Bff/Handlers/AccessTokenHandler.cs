using System.Net.Http.Headers;

namespace EnigmaVault.Web.Bff.Handlers
{
    public sealed class AccessTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = httpContextAccessor.HttpContext?.Items["AccessToken"] as string;

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}