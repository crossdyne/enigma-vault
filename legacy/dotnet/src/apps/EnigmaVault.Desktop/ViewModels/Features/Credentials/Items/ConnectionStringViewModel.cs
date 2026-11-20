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
    public sealed partial class ConnectionStringViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.ConnectionString)
    {

        [ObservableProperty]
        private string? _value;

        [ObservableProperty]
        private string? _application;

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {
            var overview = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new ConnectionString(Value, Application);

            var encryptedDetails = secureData.EncryptData(details, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var encryptedOverView = secureData.EncryptData(overview, context.Dek, CryptoConstants.CurrentCryptoVersion);

            return (encryptedOverView, encryptedDetails, CryptoConstants.CurrentCryptoVersion);
        }

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            ConnectionString details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<ConnectionString>(encryptedDetails, context.Dek)!;
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

            Value = details.Value;
            Application = details.Application;
        }

        public override void Clear()
        {
            Value = string.Empty;
            Application = string.Empty;
        }
    }
}