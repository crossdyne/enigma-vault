using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.UpdateTags;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using EnigmaVault.Secret.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class UpdateTagsCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdateTagsCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdateTagsCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var appDbContext = _fixture.DbContext;
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new UpdateTagsCommandHandler(appDbContext, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_TagsUpdated()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1));

            var tag1 = Tag.Create(userId, TagName.Create("ValidName1"), Color.FromHex("#F11111"));
            var tag2 = Tag.Create(userId, TagName.Create("ValidName2"), Color.FromHex("#F22222"));

            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);
            await _fixture.DbContext.Set<Tag>().AddRangeAsync(tag1, tag2);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);
    
            var command = new UpdateTagsCommand(userId, vault.Id, [tag1.Id, tag2.Id]);
            Result<DateTime> result = await _handler.Handle(command, _cancellationToken);

            var updatedTag = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            updatedTag?.Tags.Should().HaveCount(2);
            updatedTag?.DateUpdated.Should().Be(vault.DateUpdated);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnTagNotFoundError()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tagId = TagId.Create(Guid.NewGuid());

            var command = new UpdateTagsCommand(UserId.Create(Guid.NewGuid()), VaultItemId.Create(Guid.NewGuid()), []);
            Result<DateTime> result = await _handler.Handle(command, _cancellationToken); 

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}