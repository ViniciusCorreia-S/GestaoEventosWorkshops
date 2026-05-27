using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.Models;

public class Evento
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Local { get; set; } = string.Empty;

    public DateOnly DataInicio { get; set; }
    public DateOnly DataFim { get; set; }

    public int? OrganizadorId { get; set; }
    public Organizador? Organizador { get; set; }

    public ICollection<Workshop> Workshops { get; set; } = [];
}
