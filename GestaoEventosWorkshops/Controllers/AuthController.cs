using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoEventosWorkshops.DTOs;
using GestaoEventosWorkshops.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestaoEventosWorkshops.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IParticipanteService _participanteService;
    private readonly IOrganizadorService _organizadorService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, IParticipanteService participanteService, IOrganizadorService organizadorService, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _participanteService = participanteService;
        _organizadorService = organizadorService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var perfil = ValidarUsuario(login.Usuario, login.Senha);
        if (perfil is not null)
        {
            var tokenEquipe = GerarToken(login.Usuario, perfil);

            _logger.LogInformation($"Usuario {login.Usuario} logado com sucesso.");

            return Ok(new
            {
                sucesso = true,
                usuario = login.Usuario,
                perfil,
                token = tokenEquipe
            });
        }

        var organizador = await _organizadorService.BuscarPorCredenciaisAsync(login.Usuario, login.Senha);
        if (organizador is not null)
        {
            var tokenOrganizador = GerarToken(organizador.Email, "Organizador", organizadorId: organizador.Id.ToString());

            _logger.LogInformation($"Organizador {organizador.Id} logado com sucesso.");

            return Ok(new
            {
                sucesso = true,
                usuario = organizador.Email,
                perfil = "Organizador",
                organizador,
                token = tokenOrganizador
            });
        }

        var participante = await _participanteService.BuscarPorCredenciaisAsync(login.Usuario, login.Senha);
        if (participante is null)
        {
            return Unauthorized(new
            {
                sucesso = false,
                mensagem = "Usuario ou senha invalidos. Para participante, use seu e-mail e codigo de inscricao."
            });
        }

        var token = GerarToken(participante.Email, "Participante", participante.Id.ToString());

        _logger.LogInformation($"Participante {participante.Id} logado com sucesso.");

        return Ok(new
        {
            sucesso = true,
            usuario = participante.Email,
            perfil = "Participante",
            participante,
            token
        });
    }

    private static string? ValidarUsuario(string usuario, string senha)
    {
        return (usuario, senha) switch
        {
            ("admin", "123456") => "Administrador",
            ("organizador", "123456") => "Organizador",
            _ => null
        };
    }

    private string GerarToken(string usuario, string perfil, string? participanteId = null, string? organizadorId = null)
    {
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key nao configurada.");

        var key = Encoding.ASCII.GetBytes(jwtKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario),
            new(ClaimTypes.Role, perfil)
        };

        if (!string.IsNullOrWhiteSpace(participanteId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, participanteId));
        }

        if (!string.IsNullOrWhiteSpace(organizadorId))
        {
            claims.Add(new Claim("organizadorId", organizadorId));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
