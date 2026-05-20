using GestaoEventosWorkshops.Models;

namespace GestaoEventosWorkshops.Repositories;

public interface IParticipanteRepository
{
    Task<List<Participante>> ListarTodosAsync();
    Task<Participante?> BuscarPorIdAsync(int id);
    Task<Participante?> BuscarPorCredenciaisAsync(string email, string codigoInscricao);
    Task<bool> ExisteEmailAsync(string email, int? ignorarParticipanteId = null);
    Task<bool> ExisteInscricaoAsync(string inscricao);
    Task AdicionarAsync(Participante participante);
    Task AtualizarAsync(Participante participante);
    Task RemoverAsync(Participante participante);
}

