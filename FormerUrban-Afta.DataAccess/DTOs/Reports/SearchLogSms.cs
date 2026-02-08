using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Reports;

public class SearchLogSms
{
    public string? Mobile { get; set; }
    public string? UserId { get; set; }
    public string? Date { get; set; }

    public SelectList Users { get; set; }
}