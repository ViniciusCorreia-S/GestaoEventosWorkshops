using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Repositories;

public class ParticipanteRepository : IParticipanteRepository
{
    private readonly AppDbContext _context;

    public ParticipanteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Participante>> ListarTodosAsync()
    {
        return await _context.Participantes
            .OrderBy(participante => participante.Nome)
            .ToListAsync();
    }

    public async Task<Participante?> BuscarPorIdAsync(int id)
    {
        return await _context.Participantes.FirstOrDefaultAsync(participante => participante.Id == id);
    }

    public async Task<Participante?> BuscarPorCredenciaisAsync(string email, string codigoInscricao)
    {
        var emailNormalizado = email.Trim().ToLowerInvariant();
        var codigoNormalizado = codigoInscricao.Trim();

        return await _context.Participantes.FirstOrDefaultAsync(participante =>
            participante.Email == emailNormalizado && participante.CodigoInscricao == codigoNormalizado && participante.Ativo);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? ignorarParticipanteId = null)
    {
        return await _context.Participantes.AnyAsync(participante =>
            participante.Email == email && (!ignorarParticipanteId.HasValue || participante.Id != ignorarParticipanteId));
    }

    public async Task<bool> ExisteInscricaoAsync(string inscricao)
    {
        return await _context.Participantes.AnyAsync(participante => participante.CodigoInscricao == inscricao);
    }

    public async Task AdicionarAsync(Participante participante)
    {
        await _context.Participantes.AddAsync(participante);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Participante participante)
    {
        _context.Participantes.Update(participante);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Participante participante)
    {
        _context.Participantes.Remove(participante);
        await _context.SaveChangesAsync();
    }
}
