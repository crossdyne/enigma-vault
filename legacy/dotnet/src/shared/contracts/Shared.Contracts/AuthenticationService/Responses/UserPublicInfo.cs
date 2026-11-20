namespace Shared.Contracts.AuthenticationService.Responses
{
    public sealed record UserPublicInfo(string ClientSalt, string EncryptedDek);
}