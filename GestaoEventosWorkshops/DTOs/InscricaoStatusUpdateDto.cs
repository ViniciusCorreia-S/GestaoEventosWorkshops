using System.ComponentModel.DataAnnotations;

namespace GestaoEventosWorkshops.DTOs;

public class InscricaoStatusUpdateDto
{
    [Required, MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}
