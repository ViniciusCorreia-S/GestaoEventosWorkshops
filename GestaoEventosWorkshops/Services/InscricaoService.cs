using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Models;
using GestaoEventosWorkshops.Repositories;

namespace GestaoEventosWorkshops.Services;

public class InscricaoService : IInscricaoService
{
    private static readonly string[] StatusPermitidos = ["Inscrito", "Compareceu", "Concluido"];

    private readonly IInscricaoRepository _inscricaoRepository;
    private readonly IParticipanteRepository _participanteRepository;

    public InscricaoService(IInscricaoRepository inscricaoRepository, IParticipanteRepository participanteRepository)
    {
        _inscricaoRepository = inscricaoRepository;
        _participanteRepository = participanteRepository;
    }

    public async Task<List<InscricaoResponseDto>> ListarTodosAsync()
    {
        var inscricoes = await _inscricaoRepository.ListarTodosAsync();
        return inscricoes.Select(MapearInscricao).ToList();
    }

    public async Task<List<InscricaoResponseDto>> ListarPorParticipanteAsync(int participanteId)
    {
        var inscricoes = await _inscricaoRepository.ListarPorParticipanteAsync(participanteId);
        return inscricoes.Select(MapearInscricao).ToList();
    }

    public async Task<InscricaoResponseDto> CriarAsync(InscricaoCreateDto dto)
    {
        // Validar se o participante existe e estÃ¡ ativo
        var participante = await _participanteRepository.BuscarPorIdAsync(dto.ParticipanteId);
        if (participante is null)
            throw new InvalidOperationException("O participante selecionado não foi encontrado. Atualize a lista de participantes e tente novamente.");

        // Validar se o participante estÃ¡ ativo
        if (!participante.Ativo)
            throw new InvalidOperationException("Não é possível realizar a inscrição porque o participante está inativo. Ative o participante antes de inscrevê-lo.");

        // Validar se a workshop existe
        if (!await _inscricaoRepository.ExisteWorkshopAsync(dto.WorkshopId))
            throw new InvalidOperationException("O workshop selecionado não foi encontrado. Atualize a lista de workshops e tente novamente.");

        // Validar se o participante ja esta inscrito no workshop
        if (await _inscricaoRepository.ExisteInscricaoAsync(dto.ParticipanteId, dto.WorkshopId))
            throw new InvalidOperationException("Este participante já está inscrito neste workshop. Escolha outro workshop ou outro participante.");

        var inscricao = new InscricaoWorkshop
        {
            ParticipanteId = dto.ParticipanteId,
            WorkshopId = dto.WorkshopId,
            DataInscricao = DateTime.UtcNow,
            Status = "Inscrito"
        };

        await _inscricaoRepository.AdicionarAsync(inscricao);
        var inscricaoCriada = await _inscricaoRepository.BuscarPorIdAsync(inscricao.Id);

        return MapearInscricao(inscricaoCriada!);
    }

    public async Task<InscricaoResponseDto?> AtualizarStatusAsync(int id, InscricaoStatusUpdateDto dto)
    {
        var inscricao = await _inscricaoRepository.BuscarPorIdAsync(id);
        if (inscricao is null)
            return null;

        var status = NormalizarStatus(dto.Status);
        if (!StatusPermitidos.Contains(status))
            throw new InvalidOperationException("Status invalido. Use Inscrito, Compareceu ou Concluido.");

        inscricao.Status = status;
        await _inscricaoRepository.AtualizarAsync(inscricao);

        var inscricaoAtualizada = await _inscricaoRepository.BuscarPorIdAsync(id);
        return MapearInscricao(inscricaoAtualizada!);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var inscricao = await _inscricaoRepository.BuscarPorIdAsync(id);
        if (inscricao is null)
            return false;

        await _inscricaoRepository.RemoverAsync(inscricao);
        return true;
    }

    // MÃ©todo auxiliar para mapear InscricaoWorkshop para InscricaoResponseDto
    private static InscricaoResponseDto MapearInscricao(InscricaoWorkshop inscricao)
    {
        return new InscricaoResponseDto
        {
            Id = inscricao.Id,
            ParticipanteId = inscricao.ParticipanteId,
            ParticipanteNome = inscricao.Participante?.Nome ?? string.Empty,
            WorkshopId = inscricao.WorkshopId,
            WorkshopNome = inscricao.Workshop?.Nome ?? string.Empty,
            EventoId = inscricao.Workshop?.EventoId ?? 0,
            EventoNome = inscricao.Workshop?.Evento?.Nome ?? string.Empty,
            DataInscricao = inscricao.DataInscricao,
            Status = inscricao.Status
        };
    }

    private static string NormalizarStatus(string status)
    {
        var valor = status.Trim();

        return valor.ToLowerInvariant() switch
        {
            "inscrito" => "Inscrito",
            "compareceu" or "confirmado" => "Compareceu",
            "concluido" or "concluiu" => "Concluido",
            _ => valor
        };
    }
}


