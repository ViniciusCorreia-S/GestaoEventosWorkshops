using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Models;
using GestaoEventosWorkshops.Repositories;

namespace GestaoEventosWorkshops.Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _repository;

    public EventoService(IEventoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EventoResponseDto>> ListarTodosAsync()
    {
        var eventos = await _repository.ListarTodosAsync();
        return eventos.Select(MapearEvento).ToList();
    }

    public async Task<EventoResponseDto?> BuscarPorIdAsync(int id)
    {
        var evento = await _repository.BuscarPorIdAsync(id);
        return evento is null ? null : MapearEvento(evento);
    }

    public async Task<EventoResponseDto> CriarAsync(EventoCreateDto dto)
    {
        ValidarPeriodo(dto.DataInicio, dto.DataFim);
        var codigo = NormalizarCodigo(dto.Codigo);

        if (await _repository.ExisteCodigoAsync(codigo))
            throw new InvalidOperationException("Já existe evento cadastrado com este código. Use um código diferente.");

        var evento = new Evento
        {
            Nome = dto.Nome.Trim(),
            Codigo = codigo,
            Local = dto.Local.Trim(),
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim
        };

        await _repository.AdicionarAsync(evento);
        return MapearEvento(evento);
    }

    public async Task<bool> AtualizarAsync(int id, EventoUpdateDto dto)
    {
        var evento = await _repository.BuscarPorIdAsync(id);
        if (evento is null)
            return false;

        ValidarPeriodo(dto.DataInicio, dto.DataFim);
        var codigo = NormalizarCodigo(dto.Codigo);

        if (await _repository.ExisteCodigoAsync(codigo, id))
            throw new InvalidOperationException("Já existe outro evento usando este código. Use um código diferente.");

        evento.Nome = dto.Nome.Trim();
        evento.Codigo = codigo;
        evento.Local = dto.Local.Trim();
        evento.DataInicio = dto.DataInicio;
        evento.DataFim = dto.DataFim;

        await _repository.AtualizarAsync(evento);
        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var evento = await _repository.BuscarPorIdAsync(id);
        if (evento is null)
            return false;

        if (await _repository.PossuiVinculosAsync(id))
            throw new InvalidOperationException("Não é possível excluir este evento porque existem workshops vinculados a ele. Exclua ou mova os workshops antes de remover o evento.");

        await _repository.RemoverAsync(evento);
        return true;
    }

    private static void ValidarPeriodo(DateOnly dataInicio, DateOnly dataFim)
    {
        if (dataFim < dataInicio)
            throw new InvalidOperationException("A data final do evento não pode ser anterior à data inicial. Ajuste o período informado.");
    }

    private static string NormalizarCodigo(string codigo)
    {
        return codigo.Trim().ToUpperInvariant();
    }

    private static EventoResponseDto MapearEvento(Evento evento)
    {
        return new EventoResponseDto
        {
            Id = evento.Id,
            Nome = evento.Nome,
            Codigo = evento.Codigo,
            Local = evento.Local,
            DataInicio = evento.DataInicio,
            DataFim = evento.DataFim
        };
    }
}
