namespace Shared.Contracts.AuthenticationService.Requests
{
    public record LoginByTokenRequest(string RefreshToken);
}