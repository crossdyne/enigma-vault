using EnigmaVault.Secret.Domain.Exception;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Xunit;

namespace EnigmaVault.Secret.Unit.Tests.ValueObjects
{
    public class UserTests
    {
        #region User - UserId

        [Fact]
        public void UserId_From_ValidGuid_ReturnsUserIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            UserId userId = UserId.Create(guid);

            Assert.NotEqual(Guid.Empty, userId.Value);
            Assert.Equal(guid, userId.Value);
        }

        [Fact]
        public void UserId_From_EmptyGuid_ThrowsEmptyIdentifierException()
        {
            Assert.Throws<EmptyIdentifierException>(() => UserId.Create(Guid.Empty));
        }

        #endregion
    }
}