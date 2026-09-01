using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_Identification",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompanyId_Identification",
                table: "Clients",
                columns: new[] { "CompanyId", "Identification" },
                unique: true,
                filter: "[IsActive] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_CompanyId_Identification",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Identification",
                table: "Clients",
                column: "Identification",
                unique: true);
        }
    }
}
