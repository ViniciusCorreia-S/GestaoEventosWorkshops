using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Models;
using GestaoEventosWorkshops.Repositories;

namespace GestaoEventosWorkshops.Services;

public class ParticipanteService : IParticipanteService
{
    private const string VersaoAtualTermosLgpd = "2026-05-26";

    private readonly IParticipanteRepository _participanteRepository;

    public ParticipanteService(IParticipanteRepository participanteRepository)
    {
        _participanteRepository = participanteRepository;
    }

    public async Task<List<ParticipanteResponseDto>> ListarTodosAsync()
    {
        var participantes = await _participanteRepository.ListarTodosAsync();
        return participantes.Select(MapearParticipante).ToList();
    }

    public async Task<List<ParticipanteResponseDto>> ListarPorOrganizadorAsync(int organizadorId)
    {
        var participantes = await _participanteRepository.ListarPorOrganizadorAsync(organizadorId);
        return participantes.Select(MapearParticipante).ToList();
    }

    public async Task<ParticipanteResponseDto?> BuscarPorIdAsync(int id)
    {
        var participante = await _participanteRepository.BuscarPorIdAsync(id);
        return participante is null ? null : MapearParticipante(participante);
    }

    public async Task<ParticipanteResponseDto?> BuscarPorCredenciaisAsync(string email, string codigoInscricao)
    {
        var participante = await _participanteRepository.BuscarPorCredenciaisAsync(email, codigoInscricao);
        return participante is null ? null : MapearParticipante(participante);
    }

    public async Task<ParticipanteResponseDto?> RegistrarAceiteTermosLgpdAsync(int id)
    {
        var participante = await _participanteRepository.BuscarPorIdAsync(id);
        if (participante is null)
            return null;

        participante.AceiteTermosLgpd = true;
        participante.DataAceiteTermosLgpd = DateTime.UtcNow;
        participante.VersaoTermosLgpd = VersaoAtualTermosLgpd;

        await _participanteRepository.AtualizarAsync(participante);
        return MapearParticipante(participante);
    }

    public async Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto)
    {
        if (!dto.AceiteTermosLgpd)
            throw new InvalidOperationException("E necessario aceitar os Termos de Uso e a Politica de Privacidade para criar a conta.");

        if (dto.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("A data de nascimento não pode ser futura. Informe uma data igual ou anterior ao dia de hoje.");

        if (await _participanteRepository.ExisteEmailAsync(dto.Email))
            throw new InvalidOperationException("Já existe participante cadastrado com este e-mail. Informe outro e-mail ou edite o cadastro existente.");

        if (await _participanteRepository.ExisteInscricaoAsync(dto.CodigoInscricao))
            throw new InvalidOperationException("Já existe participante cadastrado com este código de inscrição. Use um código diferente.");

        var participante = new Participante
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            CodigoInscricao = dto.CodigoInscricao.Trim(),
            DataNascimento = dto.DataNascimento,
            Ativo = true,
            FotoPerfil = NormalizarFotoPerfil(dto.FotoPerfil),
            AceiteTermosLgpd = true,
            DataAceiteTermosLgpd = DateTime.UtcNow,
            VersaoTermosLgpd = VersaoAtualTermosLgpd
        };

        await _participanteRepository.AdicionarAsync(participante);
        return MapearParticipante(participante);
    }

    public async Task<bool> AtualizarAsync(int id, ParticipanteUpdateDto dto)
    {
        var participante = await _participanteRepository.BuscarPorIdAsync(id);
        if (participante is null)
            return false;

        if (dto.DataNascimento > DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("A data de nascimento não pode ser futura. Informe uma data igual ou anterior ao dia de hoje.");

        if (await _participanteRepository.ExisteEmailAsync(dto.Email, id))
            throw new InvalidOperationException("Já existe outro participante usando este e-mail. Informe outro e-mail para continuar.");

        participante.Nome = dto.Nome.Trim();
        participante.Email = dto.Email.Trim().ToLowerInvariant();
        participante.DataNascimento = dto.DataNascimento;
        participante.Ativo = dto.Ativo;

        await _participanteRepository.AtualizarAsync(participante);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var participante = await _participanteRepository.BuscarPorIdAsync(id);
        if (participante is null)
            return false;

        await _participanteRepository.RemoverAsync(participante);
        return true;
    }

    private static ParticipanteResponseDto MapearParticipante(Participante participante)
    {
        return new ParticipanteResponseDto
        {
            Id = participante.Id,
            Nome = participante.Nome,
            Email = participante.Email,
            CodigoInscricao = participante.CodigoInscricao,
            DataNascimento = participante.DataNascimento,
            Ativo = participante.Ativo,
            FotoPerfil = participante.FotoPerfil,
            AceiteTermosLgpd = participante.AceiteTermosLgpd,
            DataAceiteTermosLgpd = participante.DataAceiteTermosLgpd,
            VersaoTermosLgpd = participante.VersaoTermosLgpd
        };
    }

    private static string? NormalizarFotoPerfil(string? fotoPerfil)
    {
        return string.IsNullOrWhiteSpace(fotoPerfil) ? null : fotoPerfil.Trim();
    }
}
