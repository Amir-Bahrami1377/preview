using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Reports;

public class SearchHistoryDto
{
    public string? Ip { get; set; }
    public string? UserId { get; set; }
    public EnumFormName? TableId { get; set; }
    public EnumOperation? OperationId { get; set; }
    public string? Date { get; set; }
    public int TotalCount { get; set; } = 1000;


    public SelectList Users { get; set; }
    public SelectList Tables { get; set; }
    public SelectList Operations { get; set; }
}