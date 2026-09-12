using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserId_To_IconCategory_And_Remove_IsCommon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommon",
                table: "Icons");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "IconCategories",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IconCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsCommon",
                table: "Icons",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
