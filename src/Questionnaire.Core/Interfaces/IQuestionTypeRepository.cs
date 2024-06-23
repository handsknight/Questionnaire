namespace Questionnaire.Core;

public interface IQuestionTypeRepository
{
    Task<QuestionType> CreateAsync(QuestionTypeRequestDTO questionTypeRequestDTO);
    Task<QuestionType> UpdateAsync(string id, QuestionTypeRequestDTO questionTypeRequestDTO);
    Task<int> DeleteAsync(string id);
    Task<QuestionType> GetAsync(string id);
    Task<IEnumerable<QuestionType>> GetAsync();

}
