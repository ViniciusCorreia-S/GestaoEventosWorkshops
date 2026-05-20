using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.Models;

public class InscricaoWorkshop
{
    public int Id { get; set; }

    public int ParticipanteId { get; set; }
    public Participante? Participante { get; set; }

    public int WorkshopId { get; set; }
    public Workshop? Workshop { get; set; }

    public DateTime DataInscricao { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Inscrito";
}

