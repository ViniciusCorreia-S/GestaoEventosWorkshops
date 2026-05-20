using GestaoEventosWorkshops.DTOs;

namespace GestaoEventosWorkshops.Services;

public interface IWorkshopService
{
    Task<List<WorkshopResponseDto>> ListarTodosAsync();
    Task<WorkshopResponseDto?> BuscarPorIdAsync(int id);
    Task<WorkshopResponseDto> CriarAsync(WorkshopCreateDto dto);
    Task<bool> AtualizarAsync(int id, WorkshopUpdateDto dto);
    Task<bool> RemoverAsync(int id);
}

