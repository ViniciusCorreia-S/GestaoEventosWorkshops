using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class WorkshopUpdateDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Range(1, 400, ErrorMessage = "Informe uma carga horária válida entre 1 e 400 horas.")]
    public int CargaHoraria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione um evento válido para o workshop.")]
    public int EventoId { get; set; }
}

