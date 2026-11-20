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
    public sealed partial class AsymmetricKeyViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.AsymmetricKey)
    {
        [ObservableProperty]
        private string? _publicKey;
        
        [ObservableProperty]
        private string? _privateKey;

        [ObservableProperty]
        private string? _application;
        
        [ObservableProperty]
        private string? _algorithm;

        [ObservableProperty]
        private string? _keySize;

        [ObservableProperty]
        private string? _passphrase;

        [ObservableProperty]
        private string? _format;
        
        [ObservableProperty]
        private string? _fingerprint;
        
        [ObservableProperty]
        private string? _dateCreation;
        
        [ObservableProperty]
        private string? _dateExpire;

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            AsymmetricKey details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<AsymmetricKey>(encryptedDetails, context.Dek)!;
            }
            catch (Exception)
            {
                return;
            }

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;

            PublicKey = details?.PublicKey;
            PrivateKey = details?.PrivateKey;
            Application = details?.Application;
            Algorithm = details?.Algorithm;
            KeySize = details?.KeySize;
            Passphrase = details?.Passphrase;
            Format = details?.Format;
            Fingerprint = details?.Fingerprint;
            DateCreation = details?.DateCreation;
            DateExpire = details?.DateExpire;
        }

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {
            var overview = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new AsymmetricKey(PublicKey, PrivateKey, Application, Algorithm, KeySize, Passphrase, Format, Fingerprint, DateCreation, DateExpire);

            var encryptedOverview = secureData.EncryptData(overview, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var encryptedDetails = secureData.EncryptData(details, context.Dek, CryptoConstants.CurrentCryptoVersion);

            return (encryptedOverview, encryptedDetails, CryptoConstants.CurrentCryptoVersion);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            PublicKey = string.Empty;
            PrivateKey = string.Empty;
            Application = string.Empty;
            Algorithm = string.Empty;
            KeySize = string.Empty;
            Passphrase = string.Empty;
            Format = string.Empty;
            Fingerprint = string.Empty;
            DateCreation = string.Empty;
            DateExpire = string.Empty;
        }
    }
}