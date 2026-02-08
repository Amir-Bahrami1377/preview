using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class EventLogFilter : BaseModel
{
    public string MustLoginBeLogged { get; set; } = "true";
    public string LogBarayeRaddeRamzeObour { get; set; } = "true";
    public string LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi { get; set; } = "true";
    public string LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi { get; set; } = "true";
    public string LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar { get; set; } = "true";



    public override string ToString()
    {
        return string.Join("", MustLoginBeLogged, LogBarayeRaddeRamzeObour, LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi, LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi,
            LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar, CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();
    }
}

