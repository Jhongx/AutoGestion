using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class TypeAndBrandInventories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryBrandId",
                table: "Inventories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InventoryTypeId",
                table: "Inventories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_InventoryBrandId",
                table: "Inventories",
                column: "InventoryBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_InventoryTypeId",
                table: "Inventories",
                column: "InventoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryBrands_InventoryBrandId",
                table: "Inventories",
                column: "InventoryBrandId",
                principalTable: "InventoryBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryTypes_InventoryTypeId",
                table: "Inventories",
                column: "InventoryTypeId",
                principalTable: "InventoryTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryBrands_InventoryBrandId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryTypes_InventoryTypeId",
                table: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryBrands");

            migrationBuilder.DropTable(
                name: "InventoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_InventoryBrandId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_InventoryTypeId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "InventoryBrandId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "InventoryTypeId",
                table: "Inventories");
        }
    }
}
