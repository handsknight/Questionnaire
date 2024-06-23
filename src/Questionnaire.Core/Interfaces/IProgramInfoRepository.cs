namespace Questionnaire.Core;

public interface IProgramInfoRepository
{
    Task<ProgramInfo> CreateAsync(ProgramInfoRequestDTO programInfoRequestDTO);
    Task<ProgramInfo> UpdateAsync(string id, ProgramInfoRequestDTO programInfoRequestDTO);
    Task<ProgramInfo> GetAsync(string id);
    Task<IEnumerable<ProgramInfo>> GetAsync();
}
