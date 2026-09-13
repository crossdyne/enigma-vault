using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Shared.Kernel.Exceptions;
using Xunit;

namespace EnigmaVault.Service.Unit.Tests.Domain
{
    public class VaultItemTests
    {
        private static VaultItem CreateVaultItem()
        {
            UserId userId = UserId.Create(Guid.NewGuid());
            VaultType vaultType = VaultType.Create(1);
            IconId iconId = IconId.Create(Guid.NewGuid());
            EncryptedData encryptedOverview = EncryptedData.Create("OverviewData");
            EncryptedData encryptedDetails = EncryptedData.Create("EncryptedDetails");
            CryptoVersion cryptoVersion = CryptoVersion.Create(1);

            VaultItem vaultItem = VaultItem.Create(userId, vaultType, iconId, encryptedOverview, encryptedDetails, cryptoVersion);
            
            return vaultItem;
        }

        #region VaultItem - Create

        [Fact]
        public void VaultItem_Create_ValidValues_ReturnVaultItem()
        {
            UserId userId = UserId.Create(Guid.NewGuid());
            VaultType vaultType = VaultType.Create(1);
            IconId iconId = IconId.Create(Guid.NewGuid());
            EncryptedData encryptedOverview = EncryptedData.Create("OverviewData");
            EncryptedData encryptedDetails = EncryptedData.Create("EncryptedDetails");
            CryptoVersion cryptoVersion = CryptoVersion.Create(1);

            VaultItem vaultItem = VaultItem.Create(userId, vaultType, iconId, encryptedOverview, encryptedDetails, cryptoVersion);

            Assert.NotEqual(Guid.Empty, vaultItem.Id);
            Assert.Equal(vaultType, vaultItem.VaultType);
            Assert.Equal(encryptedOverview, vaultItem.EncryptedOverview);
            Assert.Equal(encryptedDetails, vaultItem.EncryptedDetails);
            Assert.False(vaultItem.IsFavorite);
            Assert.False(vaultItem.IsArchive);
            Assert.False(vaultItem.IsInTrash);
            Assert.Null(vaultItem.DeletedAt);
            Assert.NotEqual(DateTime.MinValue, vaultItem.DateAdded);
            Assert.Null(vaultItem.DateUpdated);
            Assert.Equal(iconId, vaultItem.IconId);
            Assert.Empty(vaultItem.Tags);
        }

        #endregion

        #region VaultItem - SerFavorite

        [Fact]
        public void VaultItem_SetFavorite_True_IsFavoriteChangeTrue()
        {
            VaultItem vaultItem = CreateVaultItem();

            Assert.False(vaultItem.IsFavorite);

            vaultItem.SetFavorite(true);

            Assert.True(vaultItem.IsFavorite);
        }

        [Fact]
        public void VaultItem_SetFavorite_IdenticalValue_DontChangesIsFavorite()
        {
            VaultItem vaultItem = CreateVaultItem();

            Assert.False(vaultItem.IsFavorite);

            vaultItem.SetFavorite(false);

            Assert.False(vaultItem.IsFavorite);
        }

        #endregion

        #region VaultItem - SetFavorite

        [Fact]
        public void VaultItem_SetArchive_True_IsArchiveChangeTrue()
        {
            VaultItem vaultItem = CreateVaultItem();

            Assert.False(vaultItem.IsArchive);

            vaultItem.SetArchive(true);

            Assert.True(vaultItem.IsArchive);
        }

        [Fact]
        public void VaultItem_SetArchive_IdenticalValue_DontChangesIsArchive()
        {
            VaultItem vaultItem = CreateVaultItem();

            Assert.False(vaultItem.IsArchive);

            vaultItem.SetFavorite(false);

            Assert.False(vaultItem.IsArchive);
        }

        [Fact]
        public void VaultItem_SetArchive_TrueWhenIsInTrashTrue_ThrowDomainException()
        {
            VaultItem vaultItem = CreateVaultItem();

            vaultItem.SetInTrash(true);

            Assert.Throws<DomainException>(() => vaultItem.SetArchive(true));
        }

        #endregion

        #region VaultItem - SetInTrash

        [Fact]
        public void VaultItem_SetInTrash_True_IsInTrashChangeTrueIsArchiveChangeFalseAndDeletedAtAdd30Days()
        {
            VaultItem vaultItem = CreateVaultItem();

            vaultItem.SetInTrash(true);

            Assert.True(vaultItem.IsInTrash);
            Assert.False(vaultItem.IsArchive);
            Assert.True(DateTime.UtcNow.AddDays(29) < vaultItem.DeletedAt);
        }

        [Fact]
        public void VaultItem_SetInTrash_False_IsInTrashChangeFalseDeletedAtNull()
        {
            VaultItem vaultItem = CreateVaultItem();

            vaultItem.SetInTrash(true);

            Assert.True(vaultItem.IsInTrash);
            Assert.False(vaultItem.IsArchive);
            Assert.True(DateTime.UtcNow.AddDays(29) < vaultItem.DeletedAt);

            vaultItem.SetInTrash(false);

            Assert.False(vaultItem.IsInTrash);
            Assert.False(vaultItem.IsArchive);
            Assert.Null(vaultItem.DeletedAt);
        }

        [Fact]
        public void VaultItem_SetInTrash_TrueWhenVaultItemIsArchive_ThrowDomainException()
        {
            VaultItem vaultItem = CreateVaultItem();

            vaultItem.SetArchive(true);

            Assert.Throws<DomainException>(() => vaultItem.SetInTrash(true));
        }

        #endregion
  
        #region VaultItem - AddTag

        [Fact]
        public void VaultItem_AddTag_ValidValue_TagsCountMoreZero()
        {
            VaultItem vaultItem = CreateVaultItem();

            Tag tag1 = Tag.Create(vaultItem.UserId, "Тэг #1", "#00FBFF"); 
            Tag tag2 = Tag.Create(vaultItem.UserId, "Тэг #2", "#00FBFF");

            vaultItem.AddTag(tag1.Id);
            vaultItem.AddTag(tag2.Id);

            Assert.NotEmpty(vaultItem.Tags);
            Assert.Equal(2, vaultItem.Tags.Count);
        }

        [Fact]
        public void VaultItem_AddTag_ExistingValue_TagsCountNoChanges()
        {
            VaultItem vaultItem = CreateVaultItem();

            Tag tag1 = Tag.Create(vaultItem.UserId, "Тэг #1", "#00FBFF"); 
            Tag tag2 = Tag.Create(vaultItem.UserId, "Тэг #2", "#00FBFF");

            vaultItem.AddTag(tag1.Id);
            vaultItem.AddTag(tag2.Id);
            vaultItem.AddTag(tag2.Id);

            Assert.NotEmpty(vaultItem.Tags);
            Assert.Equal(2, vaultItem.Tags.Count);
        }

        #endregion

        #region VaultItem - SetTags

        [Fact]
        public void VaultItem_SetTags_ValidValues_TagsCountMoreZero()
        {
            VaultItem vaultItem = CreateVaultItem();

            List<TagId> tagIds = [Tag.Create(vaultItem.UserId, "Тэг #1", "#00FBFF").Id, Tag.Create(vaultItem.UserId, "Тэг #2", "#00FBFF").Id];
            vaultItem.SetTags(tagIds);

            Assert.NotEmpty(vaultItem.Tags);
            Assert.Equal(2, vaultItem.Tags.Count);
        }

        [Fact]
        public void VaultItem_SetTags_ValidValuesWithDuplicate_TagsCountNotEqualInputCount()
        {
            VaultItem vaultItem = CreateVaultItem();

            Tag tag1 = Tag.Create(vaultItem.UserId, "Тэг #1", "#00FBFF"); 
            Tag tag2 = Tag.Create(vaultItem.UserId, "Тэг #2", "#00FBFF");

            List<TagId> tagIds = [tag1.Id, tag1.Id, tag2.Id];

            vaultItem.SetTags(tagIds);

            Assert.NotEqual(tagIds.Count, vaultItem.Tags.Count);
        }

        [Fact]
        public void VaultItem_SetTags_Null_TagsNoChanges()
        {
            VaultItem vaultItem = CreateVaultItem();

            vaultItem.SetTags(null!);

            Assert.Empty(vaultItem.Tags);
        }

        #endregion

        #region VaultItem - RemoveTag

        [Fact]
        public void VaultItem_RemoveTag_Success_RemovedTagNotFound()
        {
            VaultItem vaultItem = CreateVaultItem();

            Tag tag1 = Tag.Create(vaultItem.UserId, "Тэг #1", "#00FBFF"); 
            Tag tag2 = Tag.Create(vaultItem.UserId, "Тэг #2", "#00FBFF");

            List<TagId> tagIds = [tag1.Id, tag2.Id];
            
            vaultItem.SetTags(tagIds);
            vaultItem.RemoveTag(tag1.Id);

            Assert.Null(vaultItem.Tags.FirstOrDefault(t => t.TagId == tag1.Id));
        }

        #endregion
    }
}