using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class ParticipanteCreateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string CodigoInscricao { get; set; } = string.Empty;

    [Required]
    public DateOnly DataNascimento { get; set; }

    public bool AceiteTermosLgpd { get; set; }
}
