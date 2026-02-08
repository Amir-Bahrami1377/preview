namespace FormerUrban_Afta.DataAccess.DTOs.Setting;

public class BlockedIPRangeDto
{
    public long Identity { get; set; }
    public string IPRange { get; set; }
    public string Description { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public bool IsValid { get; set; } = true;

    public BlockedIPRangeDto()
    {
        
    }
}

