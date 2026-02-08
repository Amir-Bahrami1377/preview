using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IExpertService
{
    Task<List<ExpertDto>> GetAllAsync();
    Task<ExpertDto> GetAsync(long id);
    Task<List<ExpertDto>> GetByRequestNumberAsync(int id);
    Task<ExpertDto> AddAsync(ExpertDto entity);
    Task AddListAsync(List<ExpertDto> entity);
    Task<bool> UpdateAsync(ExpertDto entity);
    Task<bool> DeleteAsync(long id);
}