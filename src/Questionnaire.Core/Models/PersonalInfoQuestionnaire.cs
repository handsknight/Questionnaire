using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class PersonalInfoQuestionnaire
{
    public string? templateId { get; set; }
    public List<string>? answers { get; set; }
}
