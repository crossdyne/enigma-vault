using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RemoveTag;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class RemoveTagFromVaultItemCommandHandlerTests: IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly RemoveTagFromVaultItemCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public RemoveTagFromVaultItemCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new RemoveTagFromVaultItemCommandHandler(vaultRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_DetachTagFromVaultItem()
        {
            var userId = UserId.Create(Guid.NewGuid());

            var tag = Tag.Create(userId, TagName.Create("ValidName1"), Color.FromHex("#F1F3F9"));
            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1));
            
            await _fixture.DbContext.Set<Tag>().AddAsync(tag, _cancellationToken);
            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);

            vault.AddTag(tag.Id);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var vaultInDbWithOneTag = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);
            vaultInDbWithOneTag?.Tags.Should().HaveCount(1);

            var command = new RemoveTagFromVaultItemCommand(userId, vault.Id, tag.Id);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            vaultInDb.Should().NotBeNull();
            vaultInDb.Tags.Should().HaveCount(0);
            vaultInDb.Tags.FirstOrDefault()?.TagId.Should().Be(tag.Id);
        }
    
        [Fact]
        public async Task Handle_ValidCommand_NotFoundVaultItemReturnNotFoundError()
        {
            var command = new RemoveTagFromVaultItemCommand(UserId.Create(Guid.NewGuid()), VaultItemId.Create(Guid.NewGuid()), TagId.Create(Guid.NewGuid()));
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}