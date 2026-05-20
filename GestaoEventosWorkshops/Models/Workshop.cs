using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.Models;

public class Workshop
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    public int CargaHoraria { get; set; }

    public int EventoId { get; set; }
    public Evento? Evento { get; set; }

    // Relacionamento muitos-para-muitos com Participante via InscricaoWorkshop
    public ICollection<InscricaoWorkshop> Inscricoes { get; set; } = [];
}

