using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAppDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addClaimTypeAndValueToRoleClaimsTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimType",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "RoleClaims");
        }
    }
}
