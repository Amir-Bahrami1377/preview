using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Reports
{
    public class AuditDto
    {
        public long Id { get; set; }
        public string? IpAddress { get; set; }
        public string? FullName { get; set; }
        public string? OriginValue { get; set; }
        public string? CurrentValue { get; set; }
        public EnumFormName Table { get; set; }
        public EnumOperation Operation { get; set; }
        public string TableName { get; set; }
        public string OperationName { get; set; }
        public string CreationDate { get; set; }
        public string Field { get; set; }
        public string EntityId { get; set; }
        public bool IsValid { get; set; } = true;
    }

    public class AuditSearchDto
    {
        public string? Ip { get; set; }
        public string? UserId { get; set; }
        public EnumFormName? TableId { get; set; }
        public string? Date { get; set; }
        public int TotalCount { get; set; } = 1000;
        public string EntityId { get; set; }

        public SelectList Users { get; set; }
        public SelectList Tables { get; set; }
    }

    public class AuditViewDto
    {
        public List<AuditDto> Audit { get; set; }
        public AuditSearchDto Search { get; set; }
    }
}
