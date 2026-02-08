using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;

public interface IParvanehService
{
    Task<bool> Exist(int shod);
    Task<ParvanehDto> GetData(int shod);
    Task<ParvanehDto> AddFirstTime(int shod);
    Task<bool> UpdateModel(ParvanehDto parvanehDto);
}
