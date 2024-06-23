using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class PersonalInfoQuestionnaireDTO
{
    [Required(ErrorMessage = "Question Template is required.")]
    public string? TemplateId { get; set; }

    [Required(ErrorMessage = "Answer is required.")]
    public List<string>? Answers { get; set; }
}

