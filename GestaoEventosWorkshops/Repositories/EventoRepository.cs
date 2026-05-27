using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Evento>> ListarTodosAsync()
    {
        return await _context.Eventos
            .Include(evento => evento.Organizador)
            .OrderBy(evento => evento.DataInicio)
            .ToListAsync();
    }

    public async Task<List<Evento>> ListarPorOrganizadorAsync(int organizadorId)
    {
        return await _context.Eventos
            .Include(evento => evento.Organizador)
            .Where(evento => evento.OrganizadorId == organizadorId)
            .OrderBy(evento => evento.DataInicio)
            .ToListAsync();
    }

    public async Task<Evento?> BuscarPorIdAsync(int id)
    {
        return await _context.Eventos
            .Include(evento => evento.Organizador)
            .FirstOrDefaultAsync(evento => evento.Id == id);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarEventoId = null)
    {
        return await _context.Eventos.AnyAsync(evento =>
            evento.Codigo == codigo && (!ignorarEventoId.HasValue || evento.Id != ignorarEventoId));
    }

    public async Task<bool> ExisteOrganizadorAsync(int organizadorId)
    {
        return await _context.Organizadores.AnyAsync(organizador => organizador.Id == organizadorId && organizador.Ativo);
    }

    public async Task<bool> PossuiVinculosAsync(int eventoId)
    {
        return await _context.Workshops.AnyAsync(workshop => workshop.EventoId == eventoId);
    }

    public async Task AdicionarAsync(Evento evento)
    {
        await _context.Eventos.AddAsync(evento);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Evento evento)
    {
        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Evento evento)
    {
        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
    }
}
