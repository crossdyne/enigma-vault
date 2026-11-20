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
    public sealed partial class ApiKeyViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.ApiKey)
    {
        #region Расшифрованные данные

        [ObservableProperty]
        private string? _apiKey;

        [ObservableProperty]
        private string? _baseUrl;

        [ObservableProperty]
        private string? _clientId;

        [ObservableProperty]
        private string? _clientSecret;

        [ObservableProperty]
        private DateTime? _expirationDate;

        [ObservableProperty]
        private string? _environment;

        [ObservableProperty]
        private string? _scope;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            ApiKey details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<ApiKey>(encryptedDetails, context.Dek)!;
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

            ApiKey = details.Key;
            BaseUrl = details.BaseUrl;
            ClientId = details.ClientId;
            ClientSecret = details.ClientSecret;
            Environment = details.Environment;
            Scope = details.Scope;

            if (ExpirationDate is not null)
                ExpirationDate = DateTime.Parse(details.ExpirationDate!);
        }

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {

            var overView = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new ApiKey(ApiKey, BaseUrl, ClientId, ClientSecret, ExpirationDate?.ToString("o"), Environment, Scope);

            var encryptedDetails = secureData.EncryptData(details, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var overviewEncrypted = secureData.EncryptData(overView, context.Dek);

            return (overviewEncrypted, encryptedDetails, CryptoConstants.CurrentCryptoVersion);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            ApiKey = string.Empty;
            BaseUrl = string.Empty;
            ClientId = string.Empty;
            ClientSecret = string.Empty;
            ExpirationDate = null;
            Environment = string.Empty;
            Scope = string.Empty;
        }
    }
}