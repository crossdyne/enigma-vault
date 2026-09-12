using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Tag;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using EnigmaVault.Secret.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnigmaVault.Secret.Infrastructure.Persistence.Configurations
{
    internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasConversion(tagId => tagId.Value, dbValue => TagId.Create(dbValue))
                .ValueGeneratedNever();

            builder.Property(t => t.UserId)
                .HasColumnName("UserId")
                .HasConversion(userId => userId.Value, dbValue => UserId.Create(dbValue))
                .IsRequired(true);

            builder.Property(t => t.Name)
                .HasColumnName("Name")
                .HasConversion(name => name.Value, dbValue => TagName.Create(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME);

            builder.Property(f => f.Color)
                .HasColumnName("Color")
                .HasConversion(color => color.Value, dbValue => Color.FromHex(dbValue))
                .IsRequired(true);
        }
    }
}