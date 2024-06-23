namespace Questionnaire.Core;

public interface ITemplateRepository
{
    Task<Template> CreateAsync(TemplateRequestDTO templateRequestDTO);
    Task<Template> UpdateAsync(string id, TemplateRequestDTO templateRequestDTO);
    Task<int> DeleteAsync(string id);
    Task<Template> GetAsync(string id);
    Task<IEnumerable<Template>> GetAsync();

}
