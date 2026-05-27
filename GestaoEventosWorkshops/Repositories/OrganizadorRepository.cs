using GestaoEventosWorkshops.Data;
using GestaoEventosWorkshops.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventosWorkshops.Repositories;

public class OrganizadorRepository : IOrganizadorRepository
{
    private readonly AppDbContext _context;

    public OrganizadorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Organizador>> ListarTodosAsync()
    {
        return await _context.Organizadores
            .OrderBy(organizador => organizador.Nome)
            .ToListAsync();
    }

    public async Task<Organizador?> BuscarPorIdAsync(int id)
    {
        return await _context.Organizadores.FirstOrDefaultAsync(organizador => organizador.Id == id);
    }

    public async Task<Organizador?> BuscarPorCredenciaisAsync(string email, string senha)
    {
        var emailNormalizado = email.Trim().ToLowerInvariant();
        return await _context.Organizadores.FirstOrDefaultAsync(organizador =>
            organizador.Email == emailNormalizado && organizador.Senha == senha && organizador.Ativo);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? ignorarOrganizadorId = null)
    {
        var emailNormalizado = email.Trim().ToLowerInvariant();
        return await _context.Organizadores.AnyAsync(organizador =>
            organizador.Email == emailNormalizado && (!ignorarOrganizadorId.HasValue || organizador.Id != ignorarOrganizadorId));
    }

    public async Task AdicionarAsync(Organizador organizador)
    {
        await _context.Organizadores.AddAsync(organizador);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Organizador organizador)
    {
        _context.Organizadores.Remove(organizador);
        await _context.SaveChangesAsync();
    }
}
