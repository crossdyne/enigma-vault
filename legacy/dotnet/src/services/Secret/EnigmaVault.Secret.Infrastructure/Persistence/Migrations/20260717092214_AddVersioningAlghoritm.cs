using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVersioningAlghoritm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretType",
                table: "VaultItems");

            migrationBuilder.AddColumn<int>(
                name: "VaultType",
                table: "VaultItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "crypto_version",
                table: "VaultItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VaultType",
                table: "VaultItems");

            migrationBuilder.DropColumn(
                name: "crypto_version",
                table: "VaultItems");

            migrationBuilder.AddColumn<string>(
                name: "SecretType",
                table: "VaultItems",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
