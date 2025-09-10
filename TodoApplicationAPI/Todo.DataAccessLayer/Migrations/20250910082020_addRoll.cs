using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addRoll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Roll",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roll",
                table: "users");
        }
    }
}
