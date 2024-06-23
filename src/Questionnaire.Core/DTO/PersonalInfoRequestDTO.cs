
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class PersonalInfoRequestDTO
{
    [Required(ErrorMessage = "First name is required.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required]
    public string? Phone { get; set; }
    public string? Nationality { get; set; }
    public string? CurrentResidence { get; set; }
    public string? IdNumber { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Program is required.")]
    public string? ProgramId { get; set; }

    [Required(ErrorMessage = "Other Info is required.")]
    public List<PersonalInfoQuestionnaireDTO>? PersonalInfoQuestionnaire { get; set; }
}
