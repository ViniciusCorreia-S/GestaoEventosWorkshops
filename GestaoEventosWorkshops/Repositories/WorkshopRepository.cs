using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Repositories;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly AppDbContext _context;

    public WorkshopRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workshop>> ListarTodosAsync()
    {
        return await _context.Workshops
            .Include(workshop => workshop.Evento)
            .OrderBy(workshop => workshop.Nome)
            .ToListAsync();
    }

    public async Task<List<Workshop>> ListarPorOrganizadorAsync(int organizadorId)
    {
        return await _context.Workshops
            .Include(workshop => workshop.Evento)
            .Where(workshop => workshop.Evento!.OrganizadorId == organizadorId)
            .OrderBy(workshop => workshop.Nome)
            .ToListAsync();
    }

    public async Task<Workshop?> BuscarPorIdAsync(int id)
    {
        return await _context.Workshops
            .Include(workshop => workshop.Evento)
            .FirstOrDefaultAsync(workshop => workshop.Id == id);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarWorkshopId = null)
    {
        return await _context.Workshops.AnyAsync(workshop =>
            workshop.Codigo == codigo && (!ignorarWorkshopId.HasValue || workshop.Id != ignorarWorkshopId));
    }

    public async Task<bool> PossuiInscricoesAsync(int workshopId)
    {
        return await _context.InscricoesWorkshops.AnyAsync(inscricao => inscricao.WorkshopId == workshopId);
    }

    public async Task AdicionarAsync(Workshop workshop)
    {
        await _context.Workshops.AddAsync(workshop);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Workshop workshop)
    {
        _context.Workshops.Update(workshop);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Workshop workshop)
    {
        _context.Workshops.Remove(workshop);
        await _context.SaveChangesAsync();
    }
}

