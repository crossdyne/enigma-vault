using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Primitives;

namespace EnigmaVault.Secret.Domain.Models
{
    public sealed class VaultItem : AggregateRoot<VaultItemId>
    {
        public UserId UserId { get; private set; }
        public VaultType VaultType { get; private set; }

        public EncryptedData EncryptedOverview { get; private set; }
        public EncryptedData EncryptedDetails { get; private set; }
        public CryptoVersion CryptoVersion { get; private set; }

        public bool IsFavorite { get; private set; }
        public bool IsArchive { get; private set; }
        public bool IsInTrash { get; private set; }

        public DateTime? DeletedAt { get; private set; } 
        public DateTime DateAdded { get; private set; }
        public DateTime? DateUpdated { get; private set; }

        public IconId IconId { get; private set; }

        private readonly List<VaultTags> _tags = [];
        public IReadOnlyCollection<VaultTags> Tags => _tags.AsReadOnly();

        private VaultItem() { }

        private VaultItem(VaultItemId id, UserId userId, VaultType type, IconId iconId, EncryptedData encryptedOverview, EncryptedData encryptedDetails, CryptoVersion cryptoVersion, bool isFavorite) : base(id)
        {
            UserId = userId;
            IconId = iconId;
            VaultType = type;
            EncryptedOverview = encryptedOverview;
            EncryptedDetails = encryptedDetails;
            CryptoVersion = cryptoVersion;
            IsFavorite = isFavorite;
        }

        public static VaultItem Create(UserId UserId, VaultType type, IconId iconId, EncryptedData encryptedOverview, EncryptedData encryptedDetails, CryptoVersion cryptoVersion, bool isFavorite = false)
        {
            return new VaultItem(
                VaultItemId.New(),
                UserId,
                type,
                iconId,
                encryptedOverview,
                encryptedDetails,
                cryptoVersion,
                isFavorite)
            {
                DateAdded = DateTime.UtcNow,
            };
        }

        public void UpdateOverview(EncryptedData encryptedOverview, CryptoVersion cryptoVersion)
        {
            EncryptedOverview = encryptedOverview;
            CryptoVersion = cryptoVersion;
            UpdateDate();
        }

        public void UpdateDetails(EncryptedData encryptedDetails, CryptoVersion cryptoVersion)
        {
            EncryptedDetails = encryptedDetails;
            CryptoVersion = cryptoVersion;
            UpdateDate();
        }

        public void SetFavorite(bool isFavorite)
        {
            if (IsFavorite == isFavorite) 
                return;

            IsFavorite = isFavorite;
            UpdateDate();
        }

        public void SetIcon(IconId iconId)
        {
            IconId = iconId;
            UpdateDate();
        }

        public void SetArchive(bool isArchive)
        {
            if (IsArchive == isArchive)
                return;
            
            Guard.Against.That(isArchive && IsInTrash, () => new DomainException(new Error(AppErrors.Validation, "Нельзя архивировать запись, находящуюся в корзине")));

            IsArchive = isArchive;
            UpdateDate();
        }

        public void SetInTrash(bool isInTrash)
        {
            if (IsInTrash == isInTrash)
                return;

            Guard.Against.That(IsArchive, () => new DomainException(new Error(AppErrors.Validation, "Нельзя удалить архивированную запись")));

            if (isInTrash)
            {
                IsInTrash = true;
                DeletedAt = DateTime.UtcNow.AddDays(30);
                IsArchive = false; 
            }
            else
            {
                IsInTrash = false;
                DeletedAt = null;
            }
            IsInTrash = isInTrash;
            UpdateDate();
        }

        public void AddTag(TagId tagId)
        {
            if (_tags.Any(t => t.TagId == tagId))
                return;

            _tags.Add(VaultTags.Create(this.Id, tagId));
            UpdateDate();
        }

        public void SetTags(IEnumerable<TagId> tagIds)
        {
            _tags.Clear();
            
            if (tagIds == null) 
                return;

            foreach (var tagId in tagIds.Distinct())
                _tags.Add(VaultTags.Create(this.Id, tagId));

            UpdateDate();
        }

        public void RemoveTag(TagId tagId)
        {
             var vaultTag = _tags.FirstOrDefault(t => t.TagId == tagId);

            if (vaultTag is null)
                return;

            _tags.Remove(vaultTag);
            UpdateDate();
        }

        public void ClearTags() => _tags.Clear();

        private void UpdateDate() => DateUpdated = DateTime.UtcNow;
    }
}