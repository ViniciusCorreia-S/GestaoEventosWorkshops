using GestaoEventosWorkshops.Models;

namespace GestaoEventosWorkshops.Repositories;

public interface IInscricaoRepository
{
    Task<List<InscricaoWorkshop>> ListarTodosAsync();
    Task<List<InscricaoWorkshop>> ListarPorParticipanteAsync(int participanteId);
    Task<InscricaoWorkshop?> BuscarPorIdAsync(int id);
    Task<bool> ExisteInscricaoAsync(int participanteId, int workshopId);
    Task<bool> ExisteWorkshopAsync(int workshopId);
    Task AdicionarAsync(InscricaoWorkshop inscricao);
    Task AtualizarAsync(InscricaoWorkshop inscricao);
    Task RemoverAsync(InscricaoWorkshop inscricao);
}

