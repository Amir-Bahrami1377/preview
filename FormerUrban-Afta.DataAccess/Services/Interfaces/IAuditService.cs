using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;

public interface IAuditService
{
    public int Add(Audit entity);
    public void GetDifferences<T>(T? oldObject, T? newObject, string identity, EnumFormName enumFormName, EnumOperation enumOperation);
    public Task<List<AuditDto>> GetAllAsync(AuditSearchDto search);
    Task<AuditSearchDto> GetDrp(AuditSearchDto command);
}

