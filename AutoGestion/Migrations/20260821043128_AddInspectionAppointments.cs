using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class AddInspectionAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScheduledDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InspectionType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReceivingOrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionAppointments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionAppointments_ReceivingOrders_ReceivingOrderId",
                        column: x => x.ReceivingOrderId,
                        principalTable: "ReceivingOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionAppointments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionAppointments_ClientId",
                table: "InspectionAppointments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionAppointments_ReceivingOrderId",
                table: "InspectionAppointments",
                column: "ReceivingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionAppointments_VehicleId",
                table: "InspectionAppointments",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionAppointments");
        }
    }
}
