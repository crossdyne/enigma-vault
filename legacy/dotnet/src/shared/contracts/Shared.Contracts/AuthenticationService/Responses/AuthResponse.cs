namespace Shared.Contracts.AuthenticationService.Responses
{
    public sealed record AuthResponse(string AccessToken, string RefreshToken, string? M2 = null);
}