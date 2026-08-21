using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class AddDocTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocTypeId",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DocTypeId",
                table: "Clients",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocTypes_Code",
                table: "DocTypes",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_DocTypes_DocTypeId",
                table: "Clients",
                column: "DocTypeId",
                principalTable: "DocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_DocTypes_DocTypeId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "DocTypes");

            migrationBuilder.DropIndex(
                name: "IX_Clients_DocTypeId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DocTypeId",
                table: "Clients");
        }
    }
}
