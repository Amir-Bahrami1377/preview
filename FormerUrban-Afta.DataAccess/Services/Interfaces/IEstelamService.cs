using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IEstelamService
{
    Task<EstelamDto> GetByRequestNumberAsync(int id);
    Task<bool> UpdateAsync(EstelamDto entity);
}
