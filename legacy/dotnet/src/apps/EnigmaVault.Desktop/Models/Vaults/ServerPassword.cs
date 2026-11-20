namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record ServerPassword(string? IpAddress, string? Port, string? Domain, string Login, string? RootPassword, string? SshKey);
}