namespace Questionnaire.Core;

public interface IPersonalInfoRepository
{
    Task<PersonalInfo> CreateAsync(PersonalInfoRequestDTO personalInfoRequestDTO);
    Task<PersonalInfo> UpdateAsync(string id, PersonalInfoRequestDTO personalInfoRequestDTO);
    Task<int> DeleteAsync(string id);
    Task<PersonalInfo> GetAsync(string id);
    Task<IEnumerable<PersonalInfo>> GetAsync();
}
