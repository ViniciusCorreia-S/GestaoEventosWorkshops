using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoEventosWorkshops.Migrations
{
    /// <inheritdoc />
    public partial class OrganizadoresEventosResponsaveis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizadorId",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizadores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrganizadorId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrganizadorId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrganizadorId",
                value: null);

            migrationBuilder.InsertData(
                table: "Organizadores",
                columns: new[] { "Id", "Ativo", "Email", "Nome", "Senha" },
                values: new object[] { 1, true, "organizador@eventos.local", "Organizador Tech", "123456" });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_OrganizadorId",
                table: "Eventos",
                column: "OrganizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizadores_Email",
                table: "Organizadores",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Organizadores_OrganizadorId",
                table: "Eventos",
                column: "OrganizadorId",
                principalTable: "Organizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Organizadores_OrganizadorId",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Organizadores");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_OrganizadorId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "OrganizadorId",
                table: "Eventos");
        }
    }
}
