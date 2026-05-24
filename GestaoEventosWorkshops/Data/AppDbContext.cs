using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<Organizador> Organizadores => Set<Organizador>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<InscricaoWorkshop> InscricoesWorkshops => Set<InscricaoWorkshop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participante>()
            .HasIndex(participante => participante.Email)
            .IsUnique();

        modelBuilder.Entity<Participante>()
            .HasIndex(participante => participante.CodigoInscricao)
            .IsUnique();

        modelBuilder.Entity<Organizador>()
            .HasIndex(organizador => organizador.Email)
            .IsUnique();

        modelBuilder.Entity<Evento>()
            .HasIndex(evento => evento.Codigo)
            .IsUnique();

        modelBuilder.Entity<Evento>()
            .HasOne(evento => evento.Organizador)
            .WithMany(organizador => organizador.Eventos)
            .HasForeignKey(evento => evento.OrganizadorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Workshop>()
            .HasIndex(workshop => workshop.Codigo)
            .IsUnique();

        modelBuilder.Entity<InscricaoWorkshop>()
            .HasIndex(inscricao => new { inscricao.ParticipanteId, inscricao.WorkshopId })
            .IsUnique();

        modelBuilder.Entity<Organizador>().HasData(
            new Organizador { Id = 1, Nome = "Organizador Tech", Email = "organizador@eventos.local", Senha = "123456", Ativo = true }
        );

        modelBuilder.Entity<Evento>().HasData(
            new Evento { Id = 1, Nome = "Tech Week 2026", Codigo = "TECH2026", Local = "Centro de Convencoes", DataInicio = new DateOnly(2026, 8, 10), DataFim = new DateOnly(2026, 8, 12), OrganizadorId = 1 },
            new Evento { Id = 2, Nome = "Semana de Inovacao", Codigo = "INOVA26", Local = "Campus Principal", DataInicio = new DateOnly(2026, 9, 21), DataFim = new DateOnly(2026, 9, 25) },
            new Evento { Id = 3, Nome = "Workshop Day", Codigo = "WKDAY26", Local = "Auditorio Central", DataInicio = new DateOnly(2026, 10, 5), DataFim = new DateOnly(2026, 10, 5) }
        );

        modelBuilder.Entity<Workshop>().HasData(
            new Workshop { Id = 1, Nome = "APIs com ASP.NET Core", Codigo = "TECH-API", CargaHoraria = 4, EventoId = 1 },
            new Workshop { Id = 2, Nome = "Banco de Dados MySQL", Codigo = "TECH-SQL", CargaHoraria = 3, EventoId = 1 },
            new Workshop { Id = 3, Nome = "Design Thinking", Codigo = "INOVA-DT", CargaHoraria = 4, EventoId = 2 },
            new Workshop { Id = 4, Nome = "Pitch de Projetos", Codigo = "INOVA-PITCH", CargaHoraria = 2, EventoId = 2 },
            new Workshop { Id = 5, Nome = "Introducao ao C#", Codigo = "WKD-CSHARP", CargaHoraria = 5, EventoId = 3 }
        );

        modelBuilder.Entity<Participante>().HasData(
            new Participante { Id = 1, Nome = "Vinicius Correia", Email = "viniciuscorreia@eventos.local", CodigoInscricao = "EVT20260001", DataNascimento = new DateOnly(2008, 3, 17), Ativo = true },
            new Participante { Id = 2, Nome = "Ana Souza", Email = "ana.souza@eventos.local", CodigoInscricao = "EVT20260002", DataNascimento = new DateOnly(2002, 4, 12), Ativo = true },
            new Participante { Id = 3, Nome = "Mariana Lima", Email = "mariana.lima@eventos.local", CodigoInscricao = "EVT20260003", DataNascimento = new DateOnly(2004, 8, 25), Ativo = true },
            new Participante { Id = 4, Nome = "Carlos Pereira", Email = "carlos.pereira@eventos.local", CodigoInscricao = "EVT20260004", DataNascimento = new DateOnly(2003, 11, 9), Ativo = false }
        );

        modelBuilder.Entity<InscricaoWorkshop>().HasData(
            new InscricaoWorkshop { Id = 1, ParticipanteId = 1, WorkshopId = 1, DataInscricao = new DateTime(2026, 5, 18, 12, 0, 0, DateTimeKind.Utc), Status = "Inscrito" },
            new InscricaoWorkshop { Id = 2, ParticipanteId = 1, WorkshopId = 2, DataInscricao = new DateTime(2026, 5, 18, 12, 0, 0, DateTimeKind.Utc), Status = "Inscrito" },
            new InscricaoWorkshop { Id = 3, ParticipanteId = 2, WorkshopId = 3, DataInscricao = new DateTime(2026, 5, 18, 12, 0, 0, DateTimeKind.Utc), Status = "Compareceu" },
            new InscricaoWorkshop { Id = 4, ParticipanteId = 3, WorkshopId = 5, DataInscricao = new DateTime(2026, 5, 18, 12, 0, 0, DateTimeKind.Utc), Status = "Inscrito" }
        );
    }
}

