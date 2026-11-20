namespace Shared.Contracts.SecretService.Requests
{
    public sealed record UpdateVaultItemRequest(string VaultItemId, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}