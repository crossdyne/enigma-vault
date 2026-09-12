using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_VaultItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VaultItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecretType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EncryptedOverview = table.Column<byte[]>(type: "bytea", maxLength: 262144, nullable: false),
                    EncryptedDetails = table.Column<byte[]>(type: "bytea", maxLength: 262144, nullable: false),
                    IsFavorite = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false),
                    IsInTrash = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaultItems");
        }
    }
}
