namespace FormerUrban_Afta.DataAccess.DTOs.Reports;

public class HistoryDto
{
    public string Identity { get; set; }
    public string UserName { get; set; }
    public string Description { get; set; }
    public EnumFormName Table { get; set; }
    public EnumOperation Operation { get; set; }
    public string TableName { get; set; }
    public string OperationName { get; set; }
    public string Ip { get; set; }
    public string CreationDate { get; set; }
    public bool IsValid { get; set; } = true;
}