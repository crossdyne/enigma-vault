using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnigmaVault.Secret.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
              sql: @"
                    CREATE COLLATION case_insensitive (
                      PROVIDER = 'icu', 
                      LOCALE = 'und-u-ks-level2', 
                      DETERMINISTIC = FALSE
                    );",

              suppressTransaction: true);

            migrationBuilder.CreateTable(
                name: "IconCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_insensitive")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IconCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Icons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SvgCode = table.Column<string>(type: "text", nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive"),
                    IsCommon = table.Column<bool>(type: "boolean", nullable: false),
                    IconCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Icons_IconCategories_IconCategoryId",
                        column: x => x.IconCategoryId,
                        principalTable: "IconCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Icons_IconCategoryId",
                table: "Icons",
                column: "IconCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icons");

            migrationBuilder.DropTable(
                name: "IconCategories");
        }
    }
}
