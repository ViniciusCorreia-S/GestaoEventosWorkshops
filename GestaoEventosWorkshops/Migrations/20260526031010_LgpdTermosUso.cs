using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoEventosWorkshops.Migrations
{
    /// <inheritdoc />
    public partial class LgpdTermosUso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AceiteTermosLgpd",
                table: "Participantes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAceiteTermosLgpd",
                table: "Participantes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersaoTermosLgpd",
                table: "Participantes",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Participantes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AceiteTermosLgpd", "DataAceiteTermosLgpd", "VersaoTermosLgpd" },
                values: new object[] { true, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), "2026-05-26" });

            migrationBuilder.UpdateData(
                table: "Participantes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AceiteTermosLgpd", "DataAceiteTermosLgpd", "VersaoTermosLgpd" },
                values: new object[] { true, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), "2026-05-26" });

            migrationBuilder.UpdateData(
                table: "Participantes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AceiteTermosLgpd", "DataAceiteTermosLgpd", "VersaoTermosLgpd" },
                values: new object[] { true, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), "2026-05-26" });

            migrationBuilder.UpdateData(
                table: "Participantes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AceiteTermosLgpd", "DataAceiteTermosLgpd", "VersaoTermosLgpd" },
                values: new object[] { true, new DateTime(2026, 5, 26, 0, 0, 0, 0, DateTimeKind.Utc), "2026-05-26" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceiteTermosLgpd",
                table: "Participantes");

            migrationBuilder.DropColumn(
                name: "DataAceiteTermosLgpd",
                table: "Participantes");

            migrationBuilder.DropColumn(
                name: "VersaoTermosLgpd",
                table: "Participantes");
        }
    }
}
