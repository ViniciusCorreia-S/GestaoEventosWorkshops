using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class InscricaoCreateDto
{
    [Range(1, int.MaxValue)]
    public int ParticipanteId { get; set; }

    [Range(1, int.MaxValue)]
    public int WorkshopId { get; set; }
}

