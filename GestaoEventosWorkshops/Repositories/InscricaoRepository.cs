using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Repositories;

public class InscricaoRepository : IInscricaoRepository
{
    private readonly AppDbContext _context;

    public InscricaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InscricaoWorkshop>> ListarTodosAsync()
    {
        return await _context.InscricoesWorkshops
            // Carrega os dados relacionados de Participante e Workshop para cada inscricao
            .Include(inscricao => inscricao.Participante)
            .Include(inscricao => inscricao.Workshop)
                .ThenInclude(workshop => workshop!.Evento)
            // Ordena as inscricaos pela data de inscricao, da mais recente para a mais antiga
            .OrderByDescending(inscricao => inscricao.DataInscricao)
            .ToListAsync();
    }

    public async Task<List<InscricaoWorkshop>> ListarPorOrganizadorAsync(int organizadorId)
    {
        return await _context.InscricoesWorkshops
            .Include(inscricao => inscricao.Participante)
            .Include(inscricao => inscricao.Workshop)
                .ThenInclude(workshop => workshop!.Evento)
            .Where(inscricao => inscricao.Workshop!.Evento!.OrganizadorId == organizadorId)
            .OrderByDescending(inscricao => inscricao.DataInscricao)
            .ToListAsync();
    }

    public async Task<List<InscricaoWorkshop>> ListarPorParticipanteAsync(int participanteId)
    {
        return await _context.InscricoesWorkshops
            .Include(inscricao => inscricao.Participante)
            .Include(inscricao => inscricao.Workshop)
                .ThenInclude(workshop => workshop!.Evento)
            .Where(inscricao => inscricao.ParticipanteId == participanteId)
            .OrderByDescending(inscricao => inscricao.DataInscricao)
            .ToListAsync();
    }

    public async Task<InscricaoWorkshop?> BuscarPorIdAsync(int id)
    {
        return await _context.InscricoesWorkshops
            .Include(inscricao => inscricao.Participante)
            .Include(inscricao => inscricao.Workshop)
                .ThenInclude(workshop => workshop!.Evento)
            .FirstOrDefaultAsync(inscricao => inscricao.Id == id);
    }

    public async Task<bool> ExisteInscricaoAsync(int participanteId, int workshopId)
    {
        // Verifica se jÃ¡ existe uma inscricao para o participante na workshop informado
        return await _context.InscricoesWorkshops.AnyAsync(inscricao =>
            inscricao.ParticipanteId == participanteId && inscricao.WorkshopId == workshopId);
    }

    public async Task<bool> ExisteWorkshopAsync(int workshopId)
    {
        return await _context.Workshops.AnyAsync(workshop => workshop.Id == workshopId);
    }

    public async Task AdicionarAsync(InscricaoWorkshop inscricao)
    {
        await _context.InscricoesWorkshops.AddAsync(inscricao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(InscricaoWorkshop inscricao)
    {
        _context.InscricoesWorkshops.Update(inscricao);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(InscricaoWorkshop inscricao)
    {
        _context.InscricoesWorkshops.Remove(inscricao);
        await _context.SaveChangesAsync();
    }
}


