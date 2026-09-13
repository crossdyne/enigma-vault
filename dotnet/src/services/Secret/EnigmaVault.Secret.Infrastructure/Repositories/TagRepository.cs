using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Application.Common.Repositories;
using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Infrastructure.Repositories
{
    internal sealed class TagRepository(IApplicationDbContext context) : ITagRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task AddAsync(Tag tag, CancellationToken token) => await _context.Set<Tag>().AddAsync(tag, token);

        public void Remove(Tag tag) => _context.Set<Tag>().Remove(tag);

        public async Task<Maybe<Tag>> GetAsync(Guid id, Guid UserId, CancellationToken token = default)
            => await _context.Set<Tag>().FirstOrDefaultAsync(ic => ic.Id == id && ic.UserId == UserId, token);

        public async Task<int> RemoveAllAsync(UserId userId)
        {
            List<Tag> tags = await _context.Set<Tag>().Where(t => t.UserId == userId).ToListAsync();
            _context.Set<Tag>().RemoveRange(tags);

            return tags.Count;
        }
    }
}