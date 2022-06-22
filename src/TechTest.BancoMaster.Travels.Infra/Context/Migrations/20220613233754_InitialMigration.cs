using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechTest.BancoMaster.Travels.Infra.Context.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Travels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    StartingPoint = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    Destination = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VARCHAR(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Travels");
        }
    }
}
