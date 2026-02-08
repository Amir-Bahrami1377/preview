using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormerUrban_Afta.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUnblockingUserTimeInTarifha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sms_shomare",
                table: "Tarifha");

            migrationBuilder.AddColumn<string>(
                name: "UnblockingUserTime",
                table: "Tarifha",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnblockingUserTime",
                table: "Tarifha");

            migrationBuilder.AddColumn<string>(
                name: "sms_shomare",
                table: "Tarifha",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
