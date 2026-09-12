using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.RemoveFromFavorites;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class RemoveFromFavoritesVaultCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly RemoveFromFavoritesVaultCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public RemoveFromFavoritesVaultCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new RemoveFromFavoritesVaultCommandHandler(vaultRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_RemoveToFavorite()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1), isFavorite: true);

            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new RemoveFromFavoritesVaultCommand(vault.Id, userId);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            vaultInDb?.IsFavorite.Should().BeFalse();
        }
    
        [Fact]
        public async Task Handle_ValidCommand_NotFoundVaultItemReturnNotFoundError()
        {
            var command = new RemoveFromFavoritesVaultCommand(VaultItemId.Create(Guid.NewGuid()), UserId.Create(Guid.NewGuid()));
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        } 
    }
}