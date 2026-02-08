namespace FormerUrban_Afta.DataAccess.Services.Interfaces;

public interface IAuditFilterService
{
    Task<List<AuditFilterDto>> GetAllAsync();
    Task<ServiceResult<string>> CreateAsync(AuditFilterDto entity);
    Task<ServiceResult<string>> DeleteById(long id);
    List<EnumFormNameInfo> GetEnumFormNameInfo();
    bool ExistsById(EnumFormName formName);
}

