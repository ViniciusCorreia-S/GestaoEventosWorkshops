using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestaoEventosWorkshops.Migrations
{
    /// <inheritdoc />
    public partial class InicialGestaoEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Local = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFim = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoInscricao = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargaHoraria = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workshops_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InscricoesWorkshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParticipanteId = table.Column<int>(type: "int", nullable: false),
                    WorkshopId = table.Column<int>(type: "int", nullable: false),
                    DataInscricao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscricoesWorkshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscricoesWorkshops_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalTable: "Participantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscricoesWorkshops_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "Id", "Codigo", "DataFim", "DataInicio", "Local", "Nome" },
                values: new object[,]
                {
                    { 1, "TECH2026", new DateOnly(2026, 8, 12), new DateOnly(2026, 8, 10), "Centro de Convencoes", "Tech Week 2026" },
                    { 2, "INOVA26", new DateOnly(2026, 9, 25), new DateOnly(2026, 9, 21), "Campus Principal", "Semana de Inovacao" },
                    { 3, "WKDAY26", new DateOnly(2026, 10, 5), new DateOnly(2026, 10, 5), "Auditorio Central", "Workshop Day" }
                });

            migrationBuilder.InsertData(
                table: "Participantes",
                columns: new[] { "Id", "Ativo", "CodigoInscricao", "DataNascimento", "Email", "Nome" },
                values: new object[,]
                {
                    { 1, true, "EVT20260001", new DateOnly(2008, 3, 17), "viniciuscorreia@eventos.local", "Vinicius Correia" },
                    { 2, true, "EVT20260002", new DateOnly(2002, 4, 12), "ana.souza@eventos.local", "Ana Souza" },
                    { 3, true, "EVT20260003", new DateOnly(2004, 8, 25), "mariana.lima@eventos.local", "Mariana Lima" },
                    { 4, false, "EVT20260004", new DateOnly(2003, 11, 9), "carlos.pereira@eventos.local", "Carlos Pereira" }
                });

            migrationBuilder.InsertData(
                table: "Workshops",
                columns: new[] { "Id", "CargaHoraria", "Codigo", "EventoId", "Nome" },
                values: new object[,]
                {
                    { 1, 4, "TECH-API", 1, "APIs com ASP.NET Core" },
                    { 2, 3, "TECH-SQL", 1, "Banco de Dados MySQL" },
                    { 3, 4, "INOVA-DT", 2, "Design Thinking" },
                    { 4, 2, "INOVA-PITCH", 2, "Pitch de Projetos" },
                    { 5, 5, "WKD-CSHARP", 3, "Introducao ao C#" }
                });

            migrationBuilder.InsertData(
                table: "InscricoesWorkshops",
                columns: new[] { "Id", "DataInscricao", "ParticipanteId", "Status", "WorkshopId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 18, 12, 0, 0, 0, DateTimeKind.Utc), 1, "Inscrito", 1 },
                    { 2, new DateTime(2026, 5, 18, 12, 0, 0, 0, DateTimeKind.Utc), 1, "Inscrito", 2 },
                    { 3, new DateTime(2026, 5, 18, 12, 0, 0, 0, DateTimeKind.Utc), 2, "Compareceu", 3 },
                    { 4, new DateTime(2026, 5, 18, 12, 0, 0, 0, DateTimeKind.Utc), 3, "Inscrito", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Codigo",
                table: "Eventos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InscricoesWorkshops_ParticipanteId_WorkshopId",
                table: "InscricoesWorkshops",
                columns: new[] { "ParticipanteId", "WorkshopId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InscricoesWorkshops_WorkshopId",
                table: "InscricoesWorkshops",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_CodigoInscricao",
                table: "Participantes",
                column: "CodigoInscricao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_Email",
                table: "Participantes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_Codigo",
                table: "Workshops",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_EventoId",
                table: "Workshops",
                column: "EventoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InscricoesWorkshops");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "Eventos");
        }
    }
}
