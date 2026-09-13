using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Infrastructure.Repositories
{
    public sealed class VaultItemRepository(IApplicationDbContext context) : IVaultItemRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(VaultItem vaultItem, CancellationToken clt) => await _context.Set<VaultItem>().AddAsync(vaultItem, clt);

        public void Remove(VaultItem vaultItem) => _context.Set<VaultItem>().Remove(vaultItem);

        public async Task<Maybe<VaultItem>> GetAsync(Guid id, Guid UserId, CancellationToken clt)
            => await _context.Set<VaultItem>().Include(vi => vi.Tags).FirstOrDefaultAsync(v => v.Id == id && v.UserId == UserId, clt);

        public async Task<int> RemoveAllAsync(UserId userId, DateTime? eventTimeUtc)
        {
            IQueryable<VaultItem> query = _context.Set<VaultItem>().Where(vi => vi.UserId == userId);

            if (eventTimeUtc is not null)
                query = query.Where(vi => vi.DateAdded < eventTimeUtc);

            List<VaultItem> vaultItems = await query.ToListAsync();
            _context.Set<VaultItem>().RemoveRange(vaultItems);

            return vaultItems.Count;
        }
    }
}