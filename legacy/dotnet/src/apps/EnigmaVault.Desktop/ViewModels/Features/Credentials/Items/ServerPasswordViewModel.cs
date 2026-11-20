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
    public sealed partial class ServerPasswordViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.Server)
    {
        #region Расшифрованные данные
       
        [ObservableProperty]
        private string? _ipAddress;

        [ObservableProperty]
        private int? _port;

        [ObservableProperty]
        private string? _domain;

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _rootPassword;

        [ObservableProperty]
        private string? _sshKey;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            ServerPassword details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<ServerPassword>(encryptedDetails, context.Dek)!;
            }
            catch (Exception)
            {
                return;
            }

            if (details is null)
                return;

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;
            SvgCode = overview?.SvgIcon;

            IpAddress = details.IpAddress;
            Domain = details.Domain;
            Login = details.Login;
            RootPassword = details.RootPassword;
            SshKey = details.SshKey;

            if (int.TryParse(details?.Port, out int port))
                Port = port;
        }

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {
            var overView = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new ServerPassword(IpAddress, Port?.ToString(), Domain, Login!, RootPassword, SshKey);

            var encryptedDetails = secureData.EncryptData(details, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var encryptedOverView = secureData.EncryptData(overView, context.Dek);

            return (encryptedOverView, encryptedDetails,CryptoConstants.CurrentCryptoVersion);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            IpAddress = string.Empty;
            Port = null;
            Domain = string.Empty;
            Login = string.Empty;
            RootPassword = string.Empty;
            SshKey = string.Empty;
        }
    }
}