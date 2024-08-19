using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAppDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addConcurencyStampToRolesTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Roles");
        }
    }
}
