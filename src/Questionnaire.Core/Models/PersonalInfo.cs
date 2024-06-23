using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core;

public class PersonalInfo : BaseModel
{
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public string? nationality { get; set; }
    public string? currentResidence { get; set; }
    public string? idNumber { get; set; }
    public string? dateOfBirth { get; set; }
    public string? gender { get; set; }
    public string? programId { get; set; }
    public List<PersonalInfoQuestionnaire>? PersonalInfoQuestionnaire { get; set; }
}
