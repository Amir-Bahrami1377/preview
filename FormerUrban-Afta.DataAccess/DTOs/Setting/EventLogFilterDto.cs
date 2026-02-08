namespace FormerUrban_Afta.DataAccess.DTOs.Setting
{
    public class EventLogFilterDto
    {

        public long Identity { get; set; }
        public bool MustLoginBeLogged { get; set; }
        public bool LogBarayeRaddeRamzeObour { get; set; }
        public bool LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi { get; set; }
        public bool LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi { get; set; }
        public bool LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string>? message { get; set; }

    }
}
