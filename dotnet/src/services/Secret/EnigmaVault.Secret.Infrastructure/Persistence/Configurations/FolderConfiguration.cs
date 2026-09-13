using EnigmaVault.Secret.Domain.Models;
using EnigmaVault.Secret.Domain.ValueObjects.Common;
using EnigmaVault.Secret.Domain.ValueObjects.Folder;
using EnigmaVault.Secret.Domain.ValueObjects.User;
using EnigmaVault.Secret.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnigmaVault.Secret.Infrastructure.Persistence.Configurations
{
    internal sealed class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable("Folders");
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("Id")
                .HasConversion(id => id.Value, dbValue => FolderId.Create(dbValue))
                .ValueGeneratedNever();

            builder.Property(f => f.UserId)
                .HasColumnName("UserId")
                .HasConversion(new ValueConverter<UserId, Guid>(userId => userId.Value, v => UserId.Create(v)))
                .IsRequired(true);

            builder.Property(f => f.ParentFolderId)
                .HasColumnName("ParentFolderId")
                .HasConversion(new ValueConverter<FolderId, Guid>(folderId => folderId.Value, dbValue => FolderId.Create(dbValue)))
                .IsRequired(false);

            builder.Property(f => f.FolderName)
                .HasColumnName("FolderName")
                .HasConversion(folderName => folderName.Value, dbValue => FolderName.Create(dbValue))
                .UseCollation(PostgresConstants.COLLATION_NAME)
                .IsRequired(true);

            builder.Property(f => f.Color)
                .HasColumnName("Color")
                .HasConversion(color => color.Value, dbValue => Color.FromHex(dbValue))
                .IsRequired(true);

            builder.HasOne<Folder>()
                .WithMany().HasForeignKey(f => f.ParentFolderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}