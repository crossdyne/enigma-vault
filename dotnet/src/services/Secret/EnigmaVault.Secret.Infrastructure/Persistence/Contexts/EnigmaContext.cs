using EnigmaVault.Secret.Application.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnigmaVault.Secret.Infrastructure.Persistence.Contexts
{
    public class EnigmaContext(DbContextOptions<EnigmaContext> options) : DbContext(options), IApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}