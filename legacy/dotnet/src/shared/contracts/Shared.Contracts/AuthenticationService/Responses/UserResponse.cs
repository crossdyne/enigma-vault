namespace Shared.Contracts.AuthenticationService.Responses
{
    public sealed record UserResponse(
        string Id,
        string Login,
        string EncryptedDek,
        int DekVersion,
        string EncryptedVerifier,
        string ClientSalt,
        int SrpVersion,
        string EncryptedVerifierWrapKey,
        int KeyWrapVersion,
        string AsymmetricKeyId,
        List<string> Roles);
}