using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Password;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnigmaVault.Secret.Infrastructure.Persistence.Configurations
{
    public sealed class VaultTagsConfiguration : IEntityTypeConfiguration<VaultTags>
    {
        public void Configure(EntityTypeBuilder<VaultTags> builder)
        {
            builder.ToTable("vault_tags");
            builder.HasKey(vt => new { vt.VaultItemId, vt.TagId });

            builder.Property(vt => vt.VaultItemId)
                .HasColumnName("vault_item_id")
                .HasConversion(vaultItem => vaultItem.Value, db => VaultItemId.Create(db))
                .ValueGeneratedNever();
        
            builder.Property(vt => vt.TagId)
                .HasColumnName("tag_id")
                .HasConversion(tagId => tagId.Value, db => TagId.Create(db))
                .ValueGeneratedNever();

            builder.HasOne<VaultItem>()
                   .WithMany(vi => vi.Tags)
                   .HasForeignKey(vt => vt.VaultItemId)
                   .OnDelete(DeleteBehavior.Cascade);
                   
            builder.HasOne<Tag>()
                   .WithMany() 
                   .HasForeignKey(vt => vt.TagId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}