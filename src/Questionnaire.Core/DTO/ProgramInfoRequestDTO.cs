using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class ProgramInfoRequestDTO
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }
}
