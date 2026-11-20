namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record class OverviewPayload(string ServiceName, string Url, string? Note, string? SvgIcon);
}