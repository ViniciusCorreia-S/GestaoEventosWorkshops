using GestaoEventosWorkshops.Models;

namespace GestaoEventosWorkshops.Repositories;

public interface IOrganizadorRepository
{
    Task<List<Organizador>> ListarTodosAsync();
    Task<Organizador?> BuscarPorIdAsync(int id);
    Task<Organizador?> BuscarPorCredenciaisAsync(string email, string senha);
    Task<bool> ExisteEmailAsync(string email, int? ignorarOrganizadorId = null);
    Task AdicionarAsync(Organizador organizador);
    Task RemoverAsync(Organizador organizador);
}
