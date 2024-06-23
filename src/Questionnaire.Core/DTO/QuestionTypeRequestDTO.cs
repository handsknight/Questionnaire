
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Core
{
    public class QuestionTypeRequestDTO
    {
        [Required]
        public string? Type { get; set; }
    }
}
