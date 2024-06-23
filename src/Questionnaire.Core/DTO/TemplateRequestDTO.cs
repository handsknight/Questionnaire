using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class TemplateRequestDTO
{
    [Required]
    public string? TypeId { get; set; }

    [Required]
    public string? Question { get; set; }

    public string[]? Choices { get; set; }

    public int MaxChoiceAllowed { get; set; }

    public bool EnableOtherOption { get; set; }
}
