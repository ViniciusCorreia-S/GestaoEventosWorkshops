using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Models;
using GestaoEventosWorkshops.Repositories;

namespace GestaoEventosWorkshops.Services;

public class OrganizadorService : IOrganizadorService
{
    private readonly IOrganizadorRepository _repository;

    public OrganizadorService(IOrganizadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrganizadorResponseDto>> ListarTodosAsync()
    {
        var organizadores = await _repository.ListarTodosAsync();
        return organizadores.Select(MapearOrganizador).ToList();
    }

    public async Task<OrganizadorResponseDto?> BuscarPorCredenciaisAsync(string email, string senha)
    {
        var organizador = await _repository.BuscarPorCredenciaisAsync(email, senha);
        return organizador is null ? null : MapearOrganizador(organizador);
    }

    public async Task<OrganizadorResponseDto> CriarAsync(OrganizadorCreateDto dto)
    {
        if (await _repository.ExisteEmailAsync(dto.Email))
            throw new InvalidOperationException("Ja existe organizador cadastrado com este e-mail.");

        var organizador = new Organizador
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            Senha = dto.Senha,
            Ativo = true,
            FotoPerfil = NormalizarFotoPerfil(dto.FotoPerfil)
        };

        await _repository.AdicionarAsync(organizador);
        return MapearOrganizador(organizador);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var organizador = await _repository.BuscarPorIdAsync(id);
        if (organizador is null)
            return false;

        await _repository.RemoverAsync(organizador);
        return true;
    }

    private static OrganizadorResponseDto MapearOrganizador(Organizador organizador)
    {
        return new OrganizadorResponseDto
        {
            Id = organizador.Id,
            Nome = organizador.Nome,
            Email = organizador.Email,
            Ativo = organizador.Ativo,
            FotoPerfil = organizador.FotoPerfil
        };
    }

    private static string? NormalizarFotoPerfil(string? fotoPerfil)
    {
        return string.IsNullOrWhiteSpace(fotoPerfil) ? null : fotoPerfil.Trim();
    }
}
