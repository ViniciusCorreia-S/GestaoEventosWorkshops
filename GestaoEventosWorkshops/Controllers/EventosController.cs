using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    private readonly IEventoService _service;

    public EventosController(IEventoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new
        {
            sucesso = true,
            dados = await _service.ListarTodosAsync()
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _service.BuscarPorIdAsync(id);
        return evento is null
            ? NotFound(new { sucesso = false, mensagem = "Evento não encontrado. Atualize a lista e tente novamente." })
            : Ok(new { sucesso = true, dados = evento });
    }

    [HttpPost]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Post([FromBody] EventoCreateDto dto)
    {
        try
        {
            var evento = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = evento.Id }, new
            {
                sucesso = true,
                mensagem = "Evento cadastrado com sucesso.",
                dados = evento
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Put(int id, [FromBody] EventoUpdateDto dto)
    {
        try
        {
            var atualizado = await _service.AtualizarAsync(id, dto);
            return atualizado
                ? Ok(new { sucesso = true, mensagem = "Evento atualizado com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Evento não encontrado. Atualize a lista e tente novamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var removido = await _service.RemoverAsync(id);
            return removido
                ? Ok(new { sucesso = true, mensagem = "Evento removido com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Evento não encontrado. Atualize a lista e tente novamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }
}