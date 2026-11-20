namespace Shared.Contracts.SecretService.Requests
{
    public sealed record CreateVaultItemRequest(int VaultType, string IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion);
}