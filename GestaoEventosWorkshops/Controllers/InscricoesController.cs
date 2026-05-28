using System.Security.Claims;
using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InscricoesController : ControllerBase
{
    private readonly IInscricaoService _service;

    public InscricoesController(IInscricaoService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = "EquipeEventos")]
    public async Task<IActionResult> Get()
    {
        if (User.IsInRole("Organizador"))
        {
            if (!TryGetOrganizadorId(out var organizadorId))
                return Forbid();

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

    private bool TryGetOrganizadorId(out int organizadorId)
    {
        return int.TryParse(User.FindFirstValue("organizadorId"), out organizadorId);
    }

    [HttpGet("minhas")]
    [Authorize(Roles = "Participante")]
    public async Task<IActionResult> Minhas()
    {
        var participanteIdTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(participanteIdTexto, out var participanteId))
        {
            return Unauthorized(new { sucesso = false, mensagem = "Sessao de participante invalida. Entre novamente." });
        }

        return Ok(new
        {
            sucesso = true,
            dados = await _service.ListarPorParticipanteAsync(participanteId)
        });
    }

    [HttpPost]
    [Authorize(Policy = "EquipeEventos")]
    public async Task<IActionResult> Post([FromBody] InscricaoCreateDto dto)
    {
        try
        {
            InscricaoResponseDto inscricao;

            if (User.IsInRole("Organizador"))
            {
                if (!TryGetOrganizadorId(out var organizadorId))
                    return Forbid();

                inscricao = await _service.CriarPorOrganizadorAsync(dto, organizadorId);
            }
            else
            {
                inscricao = await _service.CriarAsync(dto);
            }

            return Created(string.Empty, new
            {
                sucesso = true,
                mensagem = "Inscricao em workshop realizada com sucesso.",
                dados = inscricao
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Policy = "EquipeEventos")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] InscricaoStatusUpdateDto dto)
    {
        try
        {
            InscricaoResponseDto? inscricao;

            if (User.IsInRole("Organizador"))
            {
                if (!TryGetOrganizadorId(out var organizadorId))
                    return Forbid();

                inscricao = await _service.AtualizarStatusPorOrganizadorAsync(id, organizadorId, dto);
            }
            else
            {
                inscricao = await _service.AtualizarStatusAsync(id, dto);
            }

            return inscricao is null
                ? NotFound(new { sucesso = false, mensagem = "Inscricao nao encontrada. Atualize a lista e tente novamente." })
                : Ok(new
                {
                    sucesso = true,
                    mensagem = "Status da inscricao atualizado com sucesso.",
                    dados = inscricao
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var removida = await _service.RemoverAsync(id);
        return removida
            ? Ok(new { sucesso = true, mensagem = "Inscricao removida com sucesso." })
            : NotFound(new { sucesso = false, mensagem = "Inscricao nao encontrada. Atualize a lista e tente novamente." });
    }
}
