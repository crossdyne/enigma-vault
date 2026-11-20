using EnigmaVault.Secret.Domain.Exception;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using Shared.Kernel.Exceptions;
using Xunit;

namespace EnigmaVault.Secret.Unit.Tests.ValueObjects
{
    public class PasswordTests
    {
        #region Password - VaultItemId

        [Fact]
        public void VaultItemId_Create_ValidGuid_ReturnsVaultItemIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            VaultItemId vaultItemId = VaultItemId.Create(guid);

            Assert.NotEqual(Guid.Empty, vaultItemId.Value);
            Assert.Equal(guid, vaultItemId.Value);
        }

        [Fact]
        public void VaultItemId_New_ReturnsNonEmptyVaultItemId()
        {
            VaultItemId vaultItemId = VaultItemId.New();

            Assert.NotEqual(Guid.Empty, vaultItemId.Value);
        }

        [Fact]
        public void DekId_Create_EmptyGuid_ThrowsEmptyIdentifierException()
        {
            Assert.Throws<EmptyIdentifierException>(() => VaultItemId.Create(Guid.Empty));
        }

        #endregion

        #region Password - IconId

        [Fact]
        public void IconId_Create_ValidGuid_ReturnsIconIddWithSameValue()
        {
            var guid = Guid.NewGuid();
            IconId iconId = IconId.Create(guid);

            Assert.NotEqual(Guid.Empty, iconId.Value);
            Assert.Equal(guid, iconId.Value);
        }

        [Fact]
        public void IconId_New_ReturnsNonEmptyIconId()
        {
            IconId iconId = IconId.New();

            Assert.NotEqual(Guid.Empty, iconId.Value);
        }

        #endregion

        #region Password - CryptoVersion

        [Fact]
        public void CryptoVersion_Create_ValidValue_ReturnCryptoVersion()
        {
            int version = 1;
            CryptoVersion cryptoVersion = CryptoVersion.Create(1);

            Assert.Equal(version, cryptoVersion.Value);
        }

        [Fact]
        public void CryptoVersion_Create_NegativeValue_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CryptoVersion.Create(-1));
        }

        #endregion

        #region Password - EncryptedData

        [Fact]
        public void EncryptedData_Create_ValidValue_ReturnEncryptedData()
        {
            string encryptedDataString = "encrypted_data";
            EncryptedData encryptedData = EncryptedData.Create(encryptedDataString);

            Assert.Equal(encryptedData, encryptedData.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EncryptedData_Create_NegativeValue_ThrowDomainException(string? value)
        {
            Assert.Throws<DomainException>(() => EncryptedData.Create(value!));
        }

        #endregion
    
        #region Password - VaultType

        [Fact]
        public void VaultType_FromName_ValidName_ReturnVaultType()
        {
            int type = 2;
            VaultType vaultType = VaultType.Create(type);

            Assert.Equal(type, vaultType.Value);
        }

        [Fact]
        public void VaultType_Create_NegativeValue_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => VaultType.Create(-1));
        }

        #endregion
    }
}