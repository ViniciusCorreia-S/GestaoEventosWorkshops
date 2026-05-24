using GestaoEventosWorkshops.Models;

namespace GestaoEventosWorkshops.Repositories;

public interface IEventoRepository
{
    Task<List<Evento>> ListarTodosAsync();
    Task<List<Evento>> ListarPorOrganizadorAsync(int organizadorId);
    Task<Evento?> BuscarPorIdAsync(int id);
    Task<bool> ExisteCodigoAsync(string codigo, int? ignorarEventoId = null);
    Task<bool> ExisteOrganizadorAsync(int organizadorId);
    Task<bool> PossuiVinculosAsync(int eventoId);
    Task AdicionarAsync(Evento evento);
    Task AtualizarAsync(Evento evento);
    Task RemoverAsync(Evento evento);
}

