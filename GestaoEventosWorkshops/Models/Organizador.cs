using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.Models;

public class Organizador
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Senha { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    public string? FotoPerfil { get; set; }

    public ICollection<Evento> Eventos { get; set; } = [];
}
