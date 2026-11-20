using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.AddToFavorites;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class AddToFavoritesVaultCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly AddToFavoritesVaultCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public AddToFavoritesVaultCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new AddToFavoritesVaultCommandHandler(vaultRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_AddToFavorite()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1));

            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new AddToFavoritesVaultCommand(vault.Id, userId);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            vaultInDb?.IsFavorite.Should().BeTrue();
        }
    
        [Fact]
        public async Task Handle_ValidCommand_NotFoundVaultItemReturnNotFoundError()
        {
            var command = new AddToFavoritesVaultCommand(VaultItemId.Create(Guid.NewGuid()), UserId.Create(Guid.NewGuid()));
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}