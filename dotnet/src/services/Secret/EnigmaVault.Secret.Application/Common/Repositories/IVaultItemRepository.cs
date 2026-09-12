using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.User;

namespace EnigmaVault.Secret.Application.Common.Repositories
{
    public interface IVaultItemRepository
    {
        Task AddAsync(VaultItem vaultItem, CancellationToken clt);
        Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt);
        void Remove(VaultItem vaultItem);
        Task<int> RemoveAllAsync(UserId userId, DateTime? eventTimeUtc);
    }
}