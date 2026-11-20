namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record AsymmetricKey(string? PublicKey, string? PrivateKey, string? Application, string? Algorithm, string? KeySize, string? Passphrase, string? Format, string? Fingerprint, string? DateCreation, string? DateExpire);
}