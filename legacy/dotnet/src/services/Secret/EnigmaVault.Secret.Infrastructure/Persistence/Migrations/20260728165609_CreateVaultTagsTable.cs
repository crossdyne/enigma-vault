using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateVaultTagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagsIds",
                table: "VaultItems");

            migrationBuilder.RenameColumn(
                name: "IconName",
                table: "Tags",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "vault_tags",
                columns: table => new
                {
                    vault_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vault_tags", x => new { x.vault_item_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_vault_tags_Tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "Tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vault_tags_VaultItems_vault_item_id",
                        column: x => x.vault_item_id,
                        principalTable: "VaultItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vault_tags_tag_id",
                table: "vault_tags",
                column: "tag_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vault_tags");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tags",
                newName: "IconName");

            migrationBuilder.AddColumn<Guid[]>(
                name: "TagsIds",
                table: "VaultItems",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0]);
        }
    }
}
