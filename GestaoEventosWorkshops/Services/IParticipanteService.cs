using GestaoEventosWorkshops.DTOs;

namespace GestaoEventosWorkshops.Services;

public interface IParticipanteService
{
    Task<List<ParticipanteResponseDto>> ListarTodosAsync();
    Task<List<ParticipanteResponseDto>> ListarPorOrganizadorAsync(int organizadorId);
    Task<ParticipanteResponseDto?> BuscarPorIdAsync(int id);
    Task<ParticipanteResponseDto?> BuscarPorCredenciaisAsync(string email, string codigoInscricao);
    Task<ParticipanteResponseDto?> RegistrarAceiteTermosLgpdAsync(int id);
    Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto);
    Task<bool> AtualizarAsync(int id, ParticipanteUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}

