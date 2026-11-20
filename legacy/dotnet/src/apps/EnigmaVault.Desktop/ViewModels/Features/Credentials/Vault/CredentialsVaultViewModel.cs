using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Common.Organization;
using Shared.Contracts.SecretService.Responses;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault
{
    public sealed partial class CredentialsVaultViewModel : BaseViewModel
    {
        private readonly EncryptedVaultResponse _model;
        private readonly ICryptoService _crypto;
        private readonly byte[] _key;

        public CredentialsVaultViewModel(EncryptedVaultResponse model, ICryptoService crypto, byte[] key, IReadOnlyCollection<TagViewModel> tags)
        {
            _model = model;
            _crypto = crypto;
            _key = key;
            _tags = tags;

            Tags = new ObservableCollection<TagViewModel>(model.TagsIds.Select(id => tags.FirstOrDefault(t => t.Id == id)).Where(t => t is not null)!);
            IsFavorite = model.IsFavorite;
            IsArchive = model.IsArchive;
            IsInTrash = model.IsInTrash;
            EncryptedOverview = model.EncryptedOverview;
            EncryptedDetails = model.EncryptedDetails;
            DateAdded = model.DateAdded.ToLocalTime();
            IconId = model.IconId;

            if (model.DateUpdate is not null)
                DateUpdate = model.DateUpdate.Value.ToLocalTime();

            if (model.DeletedAt is not null)
                DeletedAt = model.DeletedAt.Value.ToLocalTime();


            Decrypt(EncryptedOverview);
        }

        private IReadOnlyCollection<TagViewModel> _tags;

        public string Id => _model.Id;

        public VaultType Type => Enum.Parse<VaultType>(_model.Type);

        public ObservableCollection<TagViewModel> Tags { get; private set; }

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        private bool _isArchive;

        [ObservableProperty]
        private bool _isInTrash;

        [ObservableProperty]
        private string _encryptedOverview = null!;

        [ObservableProperty]
        private string _encryptedDetails = null!;

        [ObservableProperty]
        private string _serviceName = null!;

        partial void OnServiceNameChanged(string value) => OnPropertyChanged(nameof(ServiceNameFirstLetter));

        [ObservableProperty]
        private string? _url;

        [ObservableProperty]
        private DrawingImage? _icon;

        [ObservableProperty]
        private DateTime _dateAdded;

        [ObservableProperty]
        private DateTime? _dateUpdate;

        [ObservableProperty]
        private DateTime? _deletedAt;

        [ObservableProperty]
        private string? _iconId;

        public string? FirstTagName => Tags.FirstOrDefault()?.TagName ?? "Без тега";

        public string ServiceNameFirstLetter
        {
            get
            {
                if (string.IsNullOrEmpty(ServiceName))
                    return "#";

                return ServiceName.Substring(0, 1).ToUpper();
            }
        }

        public string DateOnlyAdd => DateOnly.FromDateTime(DateAdded).ToString("D");

        public string DateOnlyUpdate
        {
            get
            {
                if (!DateUpdate.HasValue)
                    return DateOnly.MinValue.ToString("D");

                return DateOnly.FromDateTime(DateUpdate.Value).ToString("D");
            }
        }

        public string VaultTypeString
        {
            get
            {
                Func<string> func = Type switch
                {
                    VaultType.Password => () => "Стандартныйе пароли",
                    VaultType.Server => () => "Серверные данные",
                    VaultType.CreditCard => () => "Банковские карты",
                    VaultType.ApiKey => () => "API",
                    VaultType.ConnectionString => () => "Строки подключения",
                    VaultType.AsymmetricKey => () => "Ассеметричные ключи",
                    _ => () => "#"
                };

                return func.Invoke();
            }
        }

        public void UpdateEncrypted(string encryptedOverview, string encryptedDetails)
        {
            EncryptedOverview = encryptedOverview;
            EncryptedDetails = encryptedDetails;

            Decrypt(encryptedOverview);
        }

        public void UpdateDate(DateTime dateUpdate) => DateUpdate = dateUpdate;

        public void AddTag(string tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);

            if (tag != null && !Tags.Contains(tag))
                Tags.Add(tag);
        }

        public void RemoveTag(string tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);

            if (tag != null && Tags.Contains(tag))
                Tags.Remove(tag);
        }

        private void Decrypt(string encryptedOverview)
        {

            try
            {
                var overview = _crypto.DecryptData<OverviewPayload>(encryptedOverview, _key);

                if (overview != null)
                {
                    ServiceName = overview.ServiceName;
                    Url = overview.Url;
                }
            }
            catch
            {

            }
        }

        public void SetIcon(DrawingImage? image) => Icon = image;
    }
}