using EnigmaVault.Secret.Domain.Exception;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using Shared.Kernel.Exceptions;
using Xunit;

namespace EnigmaVault.Secret.Unit.Tests.ValueObjects
{
    public class TagTests
    {
        #region Tag - TagId

        [Fact]
        public void TagId_Create_ValidGuid_ReturnsTagIdWithSameValue()
        {
            var guid = Guid.NewGuid();
            TagId tagId = TagId.Create(guid);

            Assert.NotEqual(Guid.Empty, tagId.Value);
            Assert.Equal(guid, tagId.Value);
        }

        [Fact]
        public void TagId_New_ReturnsNonEmptyTagId()
        {
            TagId tagId = TagId.New();

            Assert.NotEqual(Guid.Empty, tagId.Value);
        }

        [Fact]
        public void DekId_Create_EmptyGuid_ThrowsEmptyIdentifierException()
        {
            Assert.Throws<EmptyIdentifierException>(() => TagId.Create(Guid.Empty));
        }

        #endregion        
        
        #region Tag - TagName

        [Fact]
        public void TagName_Create_ValidName_ReturnsRTagNameWithSameValue()
        {
            var name = "ValidName";
            TagName tagName = TagName.Create(name);

            Assert.Equal(name, tagName.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RoleName_Create_EmptyName_ThrowsDomainException(string? value)
        {
            Assert.Throws<DomainException>(() => TagName.Create(value!));
        }

        [Theory]
        [InlineData("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq")] //Max Length = 100, InlineData = 101
        [InlineData("q")] //Nim Length - 2
        public void RoleName_Create_IncorrectDiapason_ThrowsDomainException(string value)
        {
            Assert.Throws<DomainException>(() => TagName.Create(value));
        }

        #endregion
    }
}