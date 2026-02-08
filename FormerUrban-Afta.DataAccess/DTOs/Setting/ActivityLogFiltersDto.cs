using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Setting;
public class ActivityLogFiltersDto
{
    public long Identity { get; set; }
    public EnumFormName FormName { get; set; }
    public string TableName { get; set; }
    public bool AddStatus { get; set; } = true;
    public bool UpdateStatus { get; set; } = true;
    public bool GetStatus { get; set; } = true;
    public bool DeleteStatus { get; set; } = true;
    public bool IsValid { get; set; } = true;
    public List<string>? message { get; set; }

    public SelectList Tables { get; set; }
}   