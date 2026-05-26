using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.Models;

public class Participante
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string CodigoInscricao { get; set; } = string.Empty;

    public DateOnly DataNascimento { get; set; }

    public bool Ativo { get; set; } = true;

    public bool AceiteTermosLgpd { get; set; }

    public DateTime? DataAceiteTermosLgpd { get; set; }

    [MaxLength(20)]
    public string? VersaoTermosLgpd { get; set; }

    public ICollection<InscricaoWorkshop> Inscricoes { get; set; } = [];
}
