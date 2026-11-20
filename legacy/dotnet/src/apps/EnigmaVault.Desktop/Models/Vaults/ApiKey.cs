namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record ApiKey(string? Key, string? BaseUrl, string? ClientId, string? ClientSecret, string? ExpirationDate, string? Environment, string? Scope);
}