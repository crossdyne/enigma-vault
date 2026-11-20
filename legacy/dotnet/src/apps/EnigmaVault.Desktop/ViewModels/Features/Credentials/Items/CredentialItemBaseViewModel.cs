using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Configuration;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Items
{
    public abstract partial class CredentialItemBaseViewModel : BaseViewModel
    {
        private CredentialsVaultViewModel? _model;

        public CredentialItemBaseViewModel(CredentialsVaultViewModel model, VaultType type)
        {
            _model = model;

            if (_model != null)
            {
                _type = type;
                _isReadOnly = false;
                _isArchive = model.IsArchive;
                _isInTrash = model.IsInTrash; 
            }
        }

        [ObservableProperty]
        private bool _isReadOnly;

        [ObservableProperty]
        private bool _isArchive;

        [ObservableProperty]
        private bool _isInTrash;

        [ObservableProperty]
        private VaultType _type;

        public string Id => _model?.Id!;

        [ObservableProperty]
        private string _serviceName = null!;

        [ObservableProperty]
        private string? _note;

        [ObservableProperty]
        private string? _url;

        [ObservableProperty]
        private DrawingImage? _icon;

        [ObservableProperty]
        private string? _iconId;

        [ObservableProperty]
        private string? _svgCode;

        public abstract void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context);

        public abstract (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context);

        public abstract void Clear();

        public void SetIsReadOnly(bool condition) => IsReadOnly = condition;

        public void SetIcon(DrawingImage? icon) => Icon = icon;
    }
}
