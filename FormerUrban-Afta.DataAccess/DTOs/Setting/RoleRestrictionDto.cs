using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Setting;
public class RoleRestrictionDto
{
    public long Identity { get; set; }
    public string RoleId { get; set; }
    public string RoleText { get; set; }
    public string Description { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public bool IsValid { get; set; } = true;

    public SelectList Roles { get; set; }
}