using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstructorInformationManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Instructors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructorIdNumber",
                table: "Instructors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Instructors",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "InstructorIdNumber",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Instructors");
        }
    }
}
