using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class OrganizadorCreateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(80)]
    public string Senha { get; set; } = string.Empty;

    public string? FotoPerfil { get; set; }
}
