using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Project_Repository_Pattern.Datas.Migrations
{
    /// <inheritdoc />
    public partial class addIdentityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "Attendances");
        }
    }
}
