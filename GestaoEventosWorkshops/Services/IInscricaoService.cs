using GestaoEventosWorkshops.DTOs;

namespace GestaoEventosWorkshops.Services;

public interface IInscricaoService
{
    Task<List<InscricaoResponseDto>> ListarTodosAsync();
    Task<List<InscricaoResponseDto>> ListarPorOrganizadorAsync(int organizadorId);
    Task<List<InscricaoResponseDto>> ListarPorParticipanteAsync(int participanteId);
    Task<InscricaoResponseDto> CriarAsync(InscricaoCreateDto dto);
    Task<InscricaoResponseDto?> AtualizarStatusAsync(int id, InscricaoStatusUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}

