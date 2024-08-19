using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAppDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addAddressToDoctorTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Doctors");
        }
    }
}
