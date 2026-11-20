using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Configuration;
using EnigmaVault.Desktop.Constants;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Items
{
    public sealed partial class StandardPasswordViewModel : CredentialItemBaseViewModel
    {
        public StandardPasswordViewModel(CredentialsVaultViewModel model) : base(model, VaultType.Password)
        {
            
        }

        /// <summary>
        /// Конструктор для режима дизайна
        /// </summary>
        public StandardPasswordViewModel() : base(null!, VaultType.Password)
        {
            ServiceName = "Тестовый сервис (Дизайн)";
            Url = "http://design-mode.com";
            Login = "design_user";
            Password = "P@ssw0rd!";
            Email = "DesignEmail@gmail.com";
            Phone = "+1-234-567-8901";
            SecretWord = "Design";
            RecoveryKey = "RECOVERY-KEY-1234";
        }

        #region Расшифрованные данные

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _phone;

        [ObservableProperty]
        private string? _secretWord;

        [ObservableProperty]
        private string? _recoveryKey;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            StandardPassword details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<StandardPassword>(encryptedDetails, context.Dek)!;
            }
            catch (Exception)
            {
                return;
            }

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;

            Login = details?.Login;
            Password = details?.Password;
            Email = details?.Email;
            Phone = details?.Phone;
            SecretWord = details?.SecretWord;
            RecoveryKey = details?.RecoveryKey;
        }

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {
            var overview = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new StandardPassword(Login, Password, Email, Phone, SecretWord, RecoveryKey);

            var encryptedOverview = secureData.EncryptData(overview, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var encryptedDetails = secureData.EncryptData(details, context.Dek);

            return (encryptedOverview, encryptedDetails, CryptoConstants.CurrentCryptoVersion);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            Login = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            SecretWord = string.Empty;
            RecoveryKey = string.Empty;
        }
    }
}