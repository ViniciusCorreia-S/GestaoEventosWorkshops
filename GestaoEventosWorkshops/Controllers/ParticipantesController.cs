using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "EquipeEventos")]
public class ParticipantesController : ControllerBase
{
    private readonly IParticipanteService _service;
    private readonly ILogger<ParticipantesController> _logger;

    public ParticipantesController(IParticipanteService service, ILogger<ParticipantesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        if (User.IsInRole("Organizador") && int.TryParse(User.FindFirstValue("organizadorId"), out var organizadorId))
        {
            return Ok(new
            {
                sucesso = true,
                dados = await _service.ListarPorOrganizadorAsync(organizadorId)
            });
        }

        return Ok(new
        {
            sucesso = true,
            dados = await _service.ListarTodosAsync()
        });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var participante = await _service.BuscarPorIdAsync(id);
        return participante is null
            ? NotFound(new { sucesso = false, mensagem = "Participante não encontrado. Atualize a lista e tente novamente." })
            : Ok(new { sucesso = true, dados = participante });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] ParticipanteCreateDto dto)
    {
        try
        {
            var participante = await _service.CriarAsync(dto);
            _logger.LogInformation($"Participante criado: {participante.Id} - {participante.Nome}");

            return CreatedAtAction(nameof(GetById), new { id = participante.Id }, new
            {
                sucesso = true,
                mensagem = "Participante cadastrado com sucesso.",
                dados = participante
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ParticipanteUpdateDto dto)
    {
        try
        {
            var atualizado = await _service.AtualizarAsync(id, dto);
            return atualizado
                ? Ok(new { sucesso = true, mensagem = "Participante atualizado com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Participante não encontrado. Atualize a lista e tente novamente." });
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
        var removido = await _service.RemoverAsync(id);
        return removido
            ? Ok(new { sucesso = true, mensagem = "Participante removido com sucesso." })
            : NotFound(new { sucesso = false, mensagem = "Participante não encontrado. Atualize a lista e tente novamente." });
    }
}
