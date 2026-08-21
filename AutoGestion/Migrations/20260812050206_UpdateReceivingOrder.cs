using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceivingOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelLevel",
                table: "ReceivingOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "ReceivingOrders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosticNotes",
                table: "ReceivingOrders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FuelLevelId",
                table: "ReceivingOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ReceivingOrders",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FuelLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingOrders_FuelLevelId",
                table: "ReceivingOrders",
                column: "FuelLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingOrders_FuelLevels_FuelLevelId",
                table: "ReceivingOrders",
                column: "FuelLevelId",
                principalTable: "FuelLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingOrders_FuelLevels_FuelLevelId",
                table: "ReceivingOrders");

            migrationBuilder.DropTable(
                name: "FuelLevels");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingOrders_FuelLevelId",
                table: "ReceivingOrders");

            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "ReceivingOrders");

            migrationBuilder.DropColumn(
                name: "DiagnosticNotes",
                table: "ReceivingOrders");

            migrationBuilder.DropColumn(
                name: "FuelLevelId",
                table: "ReceivingOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReceivingOrders");

            migrationBuilder.AddColumn<string>(
                name: "FuelLevel",
                table: "ReceivingOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
