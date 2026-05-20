using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Models;
using GestaoEventosWorkshops.Repositories;

namespace GestaoEventosWorkshops.Services;

public class WorkshopService : IWorkshopService
{
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IEventoRepository _eventoRepository;

    public WorkshopService(IWorkshopRepository workshopRepository, IEventoRepository eventoRepository)
    {
        _workshopRepository = workshopRepository;
        _eventoRepository = eventoRepository;
    }

    public async Task<List<WorkshopResponseDto>> ListarTodosAsync()
    {
        var workshops = await _workshopRepository.ListarTodosAsync();
        return workshops.Select(MapearWorkshop).ToList();
    }

    public async Task<WorkshopResponseDto?> BuscarPorIdAsync(int id)
    {
        var workshop = await _workshopRepository.BuscarPorIdAsync(id);
        return workshop is null ? null : MapearWorkshop(workshop);
    }

    public async Task<WorkshopResponseDto> CriarAsync(WorkshopCreateDto dto)
    {
        await ValidarEventoAsync(dto.EventoId);

        var codigo = NormalizarCodigo(dto.Codigo);
        if (await _workshopRepository.ExisteCodigoAsync(codigo))
            throw new InvalidOperationException("Já existe workshop cadastrado com este código. Use um código diferente.");

        var workshop = new Workshop
        {
            Nome = dto.Nome.Trim(),
            Codigo = codigo,
            CargaHoraria = dto.CargaHoraria,
            EventoId = dto.EventoId
        };

        await _workshopRepository.AdicionarAsync(workshop);
        var workshopCriada = await _workshopRepository.BuscarPorIdAsync(workshop.Id);

        return MapearWorkshop(workshopCriada!);
    }

    public async Task<bool> AtualizarAsync(int id, WorkshopUpdateDto dto)
    {
        var workshop = await _workshopRepository.BuscarPorIdAsync(id);
        if (workshop is null)
            return false;

        await ValidarEventoAsync(dto.EventoId);

        var codigo = NormalizarCodigo(dto.Codigo);
        if (await _workshopRepository.ExisteCodigoAsync(codigo, id))
            throw new InvalidOperationException("Já existe outro workshop usando este código. Use um código diferente.");

        workshop.Nome = dto.Nome.Trim();
        workshop.Codigo = codigo;
        workshop.CargaHoraria = dto.CargaHoraria;
        workshop.EventoId = dto.EventoId;

        await _workshopRepository.AtualizarAsync(workshop);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var workshop = await _workshopRepository.BuscarPorIdAsync(id);
        if (workshop is null)
            return false;

        if (await _workshopRepository.PossuiInscricoesAsync(id))
            throw new InvalidOperationException("Não é possível excluir este workshop porque existem inscrições vinculadas a ele. Remova as inscrições antes de excluir o workshop.");

        await _workshopRepository.RemoverAsync(workshop);
        return true;
    }

    private async Task ValidarEventoAsync(int eventoId)
    {
        var evento = await _eventoRepository.BuscarPorIdAsync(eventoId);
        if (evento is null)
            throw new InvalidOperationException("O evento selecionado para este workshop não foi encontrado. Atualize a lista de eventos e tente novamente.");
    }

    private static string NormalizarCodigo(string codigo)
    {
        return codigo.Trim().ToUpperInvariant();
    }

    private static WorkshopResponseDto MapearWorkshop(Workshop workshop)
    {
        return new WorkshopResponseDto
        {
            Id = workshop.Id,
            Nome = workshop.Nome,
            Codigo = workshop.Codigo,
            CargaHoraria = workshop.CargaHoraria,
            EventoId = workshop.EventoId,
            EventoNome = workshop.Evento?.Nome ?? string.Empty
        };
    }
}

