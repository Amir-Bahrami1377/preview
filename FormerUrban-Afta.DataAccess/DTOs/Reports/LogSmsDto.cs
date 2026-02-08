namespace FormerUrban_Afta.DataAccess.DTOs.Reports;

public class LogSmsDto
{
    public long Id { get; set; }
    public string TextSMS { get; set; }
    public string MobileSMS { get; set; }
    public string StatusSMS { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string DateTimeSMS { get; set; }
    public bool IsValid { get; set; } = true;
}