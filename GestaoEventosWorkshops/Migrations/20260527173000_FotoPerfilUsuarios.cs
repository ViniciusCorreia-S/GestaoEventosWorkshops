using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoEventosWorkshops.Migrations
{
    public partial class FotoPerfilUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "Participantes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "Organizadores",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Participantes");

            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Organizadores");
        }
    }
}
