namespace GestaoEventosWorkshops.DTOs;

public class ParticipanteResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CodigoInscricao { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public bool Ativo { get; set; }
}
