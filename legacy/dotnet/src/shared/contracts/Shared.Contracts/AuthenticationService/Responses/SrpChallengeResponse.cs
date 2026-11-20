namespace Shared.Contracts.AuthenticationService.Responses
{
    public sealed record SrpChallengeResponse(string Salt, string B, int SrpVersion);
}