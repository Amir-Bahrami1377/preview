using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormerUrban_Afta.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableUserLogined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "UserLogined",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserLogined");
        }
    }
}
