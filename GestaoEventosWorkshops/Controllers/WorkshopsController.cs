using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopService _service;

    public WorkshopsController(IWorkshopService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (User.IsInRole("Organizador") && TryGetOrganizadorId(out var organizadorId))
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

    private bool TryGetOrganizadorId(out int organizadorId)
    {
        return int.TryParse(User.FindFirstValue("organizadorId"), out organizadorId);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workshop = await _service.BuscarPorIdAsync(id);
        return workshop is null
            ? NotFound(new { sucesso = false, mensagem = "Workshop não encontrado. Atualize a lista e tente novamente." })
            : Ok(new { sucesso = true, dados = workshop });
    }

    [HttpPost]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Post([FromBody] WorkshopCreateDto dto)
    {
        try
        {
            var workshop = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = workshop.Id }, new
            {
                sucesso = true,
                mensagem = "Workshop cadastrado com sucesso.",
                dados = workshop
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "SomenteAdministrador")]
    public async Task<IActionResult> Put(int id, [FromBody] WorkshopUpdateDto dto)
    {
        try
        {
            var atualizada = await _service.AtualizarAsync(id, dto);
            return atualizada
                ? Ok(new { sucesso = true, mensagem = "Workshop atualizado com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Workshop não encontrado. Atualize a lista e tente novamente." });
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
            var removida = await _service.RemoverAsync(id);
            return removida
                ? Ok(new { sucesso = true, mensagem = "Workshop removido com sucesso." })
                : NotFound(new { sucesso = false, mensagem = "Workshop não encontrado. Atualize a lista e tente novamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }
}

