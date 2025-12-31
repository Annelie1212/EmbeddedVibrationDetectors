using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlarmDatabaseLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogMessage",
                table: "VibrationDetectorStatusLog",
                type: "nchar(100)",
                fixedLength: true,
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogMessage",
                table: "VibrationDetectorStatusLog");
        }
    }
}
