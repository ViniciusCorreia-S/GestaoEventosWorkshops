using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class EventoUpdateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Local { get; set; } = string.Empty;

    [Required]
    public DateOnly DataInicio { get; set; }

    [Required]
    public DateOnly DataFim { get; set; }

    public int? OrganizadorId { get; set; }
}
