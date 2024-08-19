﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAppDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addTypeToRolesTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Roles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Roles");
        }
    }
}
