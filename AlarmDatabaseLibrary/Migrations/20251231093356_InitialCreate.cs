using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlarmDatabaseLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VibrationDetectorStatusLog",
                columns: table => new
                {
                    VibrationDetectorStatusLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionLogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceAction = table.Column<string>(type: "nchar(15)", fixedLength: true, maxLength: 15, nullable: false),
                    OldUserValue = table.Column<int>(type: "int", nullable: false),
                    NewUserValue = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nchar(25)", fixedLength: true, maxLength: 25, nullable: false),
                    Location = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true),
                    AlarmArmed = table.Column<bool>(type: "bit", nullable: false),
                    AlarmTriggered = table.Column<bool>(type: "bit", nullable: false),
                    VibrationLevel = table.Column<int>(type: "int", nullable: false),
                    VibrationLevelThreshold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VibrationDetectorStatusLog", x => x.VibrationDetectorStatusLogId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VibrationDetectorStatusLog");
        }
    }
}
