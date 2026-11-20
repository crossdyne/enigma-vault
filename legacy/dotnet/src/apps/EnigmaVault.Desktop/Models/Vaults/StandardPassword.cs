namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record StandardPassword(string? Login, string? Password, string? Email, string? Phone, string? SecretWord, string? RecoveryKey);
}