namespace Questionnaire.Core;

public class Template : BaseModel
{
    public string? typeId { get; set; }

    public string? question { get; set; }

    public string[]? choices { get; set; }

    public int maxChoiceAllowed { get; set; }

    public bool enableOtherOption { get; set; }

}
