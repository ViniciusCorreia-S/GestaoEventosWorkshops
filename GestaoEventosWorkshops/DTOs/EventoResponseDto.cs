namespace GestaoEventosWorkshops.DTOs;

public class EventoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Local { get; set; } = string.Empty;
    public DateOnly DataInicio { get; set; }
    public DateOnly DataFim { get; set; }
    public int? OrganizadorId { get; set; }
    public string OrganizadorNome { get; set; } = string.Empty;
}
