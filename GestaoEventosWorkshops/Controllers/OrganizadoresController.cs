using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "SomenteAdministrador")]
public class OrganizadoresController : ControllerBase
{
    private readonly IOrganizadorService _service;

    public OrganizadoresController(IOrganizadorService service)
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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrganizadorCreateDto dto)
    {
        try
        {
            var organizador = await _service.CriarAsync(dto);
            return Created(string.Empty, new
            {
                sucesso = true,
                mensagem = "Organizador cadastrado com sucesso.",
                dados = organizador
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { sucesso = false, mensagem = ex.Message });
        }
    }
}
