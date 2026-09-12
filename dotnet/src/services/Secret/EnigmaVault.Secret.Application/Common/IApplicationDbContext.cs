using Microsoft.EntityFrameworkCore;

namespace EnigmaVault.Secret.Application.Common
{
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}