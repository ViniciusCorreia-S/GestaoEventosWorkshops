using GestaoEventosWorkshops.DTOs;

namespace GestaoEventosWorkshops.Services;

public interface IEventoService
{
    Task<List<EventoResponseDto>> ListarTodosAsync();
    Task<EventoResponseDto?> BuscarPorIdAsync(int id);
    Task<EventoResponseDto> CriarAsync(EventoCreateDto dto);
    Task<bool> AtualizarAsync(int id, EventoUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}

