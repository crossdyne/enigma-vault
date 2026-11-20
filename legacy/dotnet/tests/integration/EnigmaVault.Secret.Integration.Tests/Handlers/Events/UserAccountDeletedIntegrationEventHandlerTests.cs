using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.Account.EventHandlers;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Contracts.Messaging.Events;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Events
{
    public class UserAccountDeletedIntegrationEventHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UserAccountDeletedIntegrationEventHandler _handler;
        private readonly CancellationToken _ct = default;

        public UserAccountDeletedIntegrationEventHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var tagRepo = new TagRepository(_fixture.DbContext);
            var folderRepo = new FolderRepository(_fixture.DbContext);
            var uow = new UnitOfWork(_fixture.DbContext);
            var logger = NullLogger<UserAccountDeletedIntegrationEventHandler>.Instance;

            _handler = new UserAccountDeletedIntegrationEventHandler(vaultRepo, tagRepo, folderRepo, uow, logger);
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_DeletesAllUserData()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var otherUserId = UserId.Create(Guid.NewGuid());

            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1), isFavorite: true);
            var tag = Tag.Create(userId, TagName.Create("ValidName1"), Color.FromHex("#F11111"));
            var folder = Folder.CreateRoot(userId, "folderName", "#F11111");

            var otherVault = VaultItem.Create(otherUserId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1), isFavorite: true);

            await _fixture.DbContext.Set<VaultItem>().AddRangeAsync(vault, otherVault);
            await _fixture.DbContext.Set<Tag>().AddAsync(tag, _ct);
            await _fixture.DbContext.Set<Folder>().AddAsync(folder, _ct);
            await _fixture.DbContext.SaveChangesAsync(_ct);

            var @event = new UserAccountDeletedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, userId.Value);

            await _handler.HandleAsync(@event, _ct);

            var remainingVaults = await _fixture.DbContext.Set<VaultItem>().Where(v => v.UserId == userId).ToListAsync(_ct);
            var remainingTags = await _fixture.DbContext.Set<Tag>().Where(t => t.UserId == userId).ToListAsync(_ct);
            var remainingFolders = await _fixture.DbContext.Set<Folder>().Where(f => f.UserId == userId).ToListAsync(_ct);

            remainingVaults.Should().BeEmpty();
            remainingTags.Should().BeEmpty();
            remainingFolders.Should().BeEmpty();

            var otherVaults = await _fixture.DbContext.Set<VaultItem>().Where(v => v.UserId == otherUserId).ToListAsync(_ct);
            otherVaults.Should().ContainSingle();
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_NoDataForUser_DoesNotThrow()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var @event = new UserAccountDeletedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, userId.Value);

            var act = async () => await _handler.HandleAsync(@event, _ct);

            await act.Should().NotThrowAsync();
        }
    }
}