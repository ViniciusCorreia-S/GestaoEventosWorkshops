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
        return await _context.Eventos.OrderBy(evento => evento.DataInicio).ToListAsync();
    }

    public async Task<Evento?> BuscarPorIdAsync(int id)
    {
        return await _context.Eventos.FindAsync(id);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarEventoId = null)
    {
        return await _context.Eventos.AnyAsync(evento =>
            evento.Codigo == codigo && (!ignorarEventoId.HasValue || evento.Id != ignorarEventoId));
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
