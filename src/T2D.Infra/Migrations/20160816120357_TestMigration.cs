using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace T2D.Infra.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Version",
                table: "Things",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Things");
        }
    }
}
