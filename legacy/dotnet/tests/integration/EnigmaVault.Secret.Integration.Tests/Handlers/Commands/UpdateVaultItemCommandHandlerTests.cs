using System.Globalization;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Infrastructure.Persistence;
using EnigmaVault.Secret.Infrastructure.Repositories;
using EnigmaVault.Secret.Application.Features.VaultItems.Commands.Update;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Secret.Integration.Tests.Handlers.Commands
{
    public class UpdateVaultItemCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdateVaultItemCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdateVaultItemCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new UpdateVaultItemCommandHandler(vaultRepo, unitOfWork);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatedVaultItem()
        {            
            var userId = UserId.Create(Guid.NewGuid());
            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1), isFavorite: true);

            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var newIconId = IconId.Create(Guid.NewGuid());
            var newEncryptedOverview = EncryptedData.Create("NewEncryptedOverview");
            var newEncryptedDetails = EncryptedData.Create("NewEncryptedDetails");
            var newCryptoVersion = CryptoVersion.Create(2);

            var command = new UpdateVaultItemCommand(userId, vault.Id, newIconId, newEncryptedOverview, newEncryptedDetails, newCryptoVersion);
            Result<string> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);
            result.IsSuccess.Should().BeTrue();
            vaultInDb?.IconId.Should().Be(newIconId);
            vaultInDb?.EncryptedOverview.Should().Be(newEncryptedOverview);
            vaultInDb?.EncryptedDetails.Should().Be(newEncryptedDetails);
            vaultInDb?.CryptoVersion.Should().Be(newCryptoVersion);
        }

        [Fact]
        public async Task Handle_ValidCommand_NotFoundVaultItemReturnNotFoundError()
        {
            var command = new UpdateVaultItemCommand(UserId.Create(Guid.NewGuid()), VaultItemId.Create(Guid.NewGuid()), IconId.Create(Guid.NewGuid()), EncryptedData.Create("A"), EncryptedData.Create("B"), CryptoVersion.Create(1));
            Result<string> result = await _handler.Handle(command, _cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}