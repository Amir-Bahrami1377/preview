using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormerUrban_Afta.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class usernameToEventLogThreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EventLogThreshold",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EventLogThreshold");
        }
    }
}
