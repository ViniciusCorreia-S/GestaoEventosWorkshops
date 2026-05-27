using GestaoEventosWorkshops.DTOs;

namespace GestaoEventosWorkshops.Services;

public interface IOrganizadorService
{
    Task<List<OrganizadorResponseDto>> ListarTodosAsync();
    Task<OrganizadorResponseDto?> BuscarPorCredenciaisAsync(string email, string senha);
    Task<OrganizadorResponseDto> CriarAsync(OrganizadorCreateDto dto);
    Task<bool> RemoverAsync(int id);
}
