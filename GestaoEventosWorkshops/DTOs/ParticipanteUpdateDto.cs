using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class ParticipanteUpdateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateOnly DataNascimento { get; set; }

    public bool Ativo { get; set; } = true;
}
