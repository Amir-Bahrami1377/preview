using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public class UserLogined : BaseModel
{
    public string UserCode { get; set; }
    public string Ip { get; set; }
    public System.DateTime? LoginDateTime { get; set; }
    public System.DateTime? LogoutDatetime { get; set; }
    public string UserAgent { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public bool IsViewer { get; set; }
    public string Method { get; set; }
    public byte Status { get; set; }

    public override string ToString()
    {
        return string.Join("",
            UserCode, Ip, LoginDateTime?.Ticks ?? 0, LogoutDatetime?.Ticks ?? 0,
            UserAgent, FullName, UserName, IsViewer, Method,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser, Status).Trim();
    }
}