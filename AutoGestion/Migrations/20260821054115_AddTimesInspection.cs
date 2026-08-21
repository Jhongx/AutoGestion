using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoGestion.Migrations
{
    /// <inheritdoc />
    public partial class AddTimesInspection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDurationMinutes",
                table: "InspectionAppointments");

            migrationBuilder.RenameColumn(
                name: "ScheduledDateTime",
                table: "InspectionAppointments",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDate",
                table: "InspectionAppointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "InspectionAppointments",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "InspectionAppointments");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "InspectionAppointments");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "InspectionAppointments",
                newName: "ScheduledDateTime");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDurationMinutes",
                table: "InspectionAppointments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
