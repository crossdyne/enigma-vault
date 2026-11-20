namespace Shared.Contracts.AuthenticationService.Requests
{
    public sealed record SrpVerifyRequest(string Login, string A, string M1);
}