namespace EnigmaVault.Desktop.Services.Secure
{
    internal interface IAuthenticationStateService
    {
        event Action? AuthenticationRequired;
        void NotifyAuthenticationRequired();
    }
}