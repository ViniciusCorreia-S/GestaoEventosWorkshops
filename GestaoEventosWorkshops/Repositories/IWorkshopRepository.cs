using GestaoEventosWorkshops.Models;

namespace GestaoEventosWorkshops.Repositories;

public interface IWorkshopRepository
{
    Task<List<Workshop>> ListarTodosAsync();
    Task<List<Workshop>> ListarPorOrganizadorAsync(int organizadorId);
    Task<Workshop?> BuscarPorIdAsync(int id);
    Task<bool> ExisteCodigoAsync(string codigo, int? ignorarWorkshopId = null);
    Task<bool> PossuiInscricoesAsync(int workshopId);
    Task AdicionarAsync(Workshop workshop);
    Task AtualizarAsync(Workshop workshop);
    Task RemoverAsync(Workshop workshop);
}

