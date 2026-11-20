using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangingStorageByteArrayToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EncryptedOverview",
                table: "VaultItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 262144);

            migrationBuilder.AlterColumn<string>(
                name: "EncryptedDetails",
                table: "VaultItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 262144);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "EncryptedOverview",
                table: "VaultItems",
                type: "bytea",
                maxLength: 262144,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<byte[]>(
                name: "EncryptedDetails",
                table: "VaultItems",
                type: "bytea",
                maxLength: 262144,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
