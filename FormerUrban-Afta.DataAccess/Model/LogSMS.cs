namespace FormerUrban_Afta.DataAccess.Model;
public class LogSMS
{
    public long Id { get; set; }
    public string TextSMS { get; set; }
    public string MobileSMS { get; set; }
    public string StatusSMS { get; set; }
    public string UserCode { get; set; }
    public DateTime? DateTimeSMS { get; set; }
    public string Hashed { get; set; }

    public override string ToString() => string.Join("", TextSMS.Trim() ?? "", MobileSMS.Trim() ?? "", StatusSMS.Trim() ?? "", UserCode.Trim() ?? "", DateTimeSMS?.Ticks ?? 0);
}