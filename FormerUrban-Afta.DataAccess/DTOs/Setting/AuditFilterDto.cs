using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Setting;

public class AuditFilterDto
{
    public long Identity { get; set; }
    public EnumFormName FormName { get; set; }
    public string CreationDate { get; set; }

    public string TableName { get; set; }
    public bool IsValid { get; set; } = true;

    public SelectList FormNameDrp { get; set; }
}

