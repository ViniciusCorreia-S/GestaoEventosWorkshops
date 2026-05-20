namespace GestaoEventosWorkshops.DTOs;

public class WorkshopResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public int EventoId { get; set; }
    public string EventoNome { get; set; } = string.Empty;
}

