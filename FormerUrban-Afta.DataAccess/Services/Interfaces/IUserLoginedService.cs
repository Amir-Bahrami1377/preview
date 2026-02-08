using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IUserLoginedService
{
    public List<UserLoginedDto> Search(UserLoginedSearchDto search);
    public int Insert(UserLogined userLogined);
    public Task<List<UserLoginedDto>> GetSuccessFullLogin();
    public Task<List<UserLoginedDto>> GetLoginFailed();

    //public Task<UserLoginedDto> GetByUserId(string id);
    //public List<UserLoginedDto> GetActiveSessionByUserCode(string userCode);
    //public Task<List<UserLoginedDto>> Search(string userCode, string ip, DateTime fromDateTime = default, DateTime toDateTime = default,
    //    DateTime arrivalDate = default, DateTime departureDate = default);
}