namespace EnigmaVault.Desktop.Models.Vaults
{
    public sealed record CreditCard(string? CardNumber, string? CardHolder, string? ExpireDate, string? CvvCode, string? PinCode, string? BankName, string? PaymentSystem);
}