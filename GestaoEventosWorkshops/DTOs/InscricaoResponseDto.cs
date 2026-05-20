namespace GestaoEventosWorkshops.DTOs;

public class InscricaoResponseDto
{
    public int Id { get; set; }
    public int ParticipanteId { get; set; }
    public string ParticipanteNome { get; set; } = string.Empty;
    public int WorkshopId { get; set; }
    public string WorkshopNome { get; set; } = string.Empty;
    public int EventoId { get; set; }
    public string EventoNome { get; set; } = string.Empty;
    public DateTime DataInscricao { get; set; }
    public string Status { get; set; } = string.Empty;
}

