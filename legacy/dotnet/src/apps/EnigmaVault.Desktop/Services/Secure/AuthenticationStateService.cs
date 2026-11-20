namespace EnigmaVault.Desktop.Services.Secure
{
    internal sealed class AuthenticationStateService : IAuthenticationStateService
    {
        private int _isNotifying = 0;
        public event Action? AuthenticationRequired;

        public void NotifyAuthenticationRequired()
        {
            if (Interlocked.CompareExchange(ref _isNotifying, 1, 0) == 1)
                return;

            try
            {
                AuthenticationRequired?.Invoke();
            }
            finally
            {
                Interlocked.Exchange(ref _isNotifying, 0);
            }
        }
    }
}