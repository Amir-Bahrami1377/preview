using FormerUrban_Afta.DataAccess.DTOs.Login;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IActivityLogFiltersService
    {
        public ActivityLogFilters GetByFormName(EnumFormName formName);
        public ActivityLogFiltersDto GetById(long id);
        public ActivityLogFiltersDto GetByIdAsNoTracking(long id);
        public List<ActivityLogFiltersDto> GetAll();
        public Task<AuthResponse> Add(ActivityLogFiltersDto activityLogFiltersDto);
        public Task<AuthResponse> Update(ActivityLogFiltersDto activityLogFiltersDto);
        public Task<AuthResponse> Delete(long Identity);
        List<EnumFormNameInfo> GetEnumFormNameInfo();
    }
}
