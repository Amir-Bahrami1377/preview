using FormerUrban_Afta.DataAccess.Model.Enums;

namespace FormerUrban_Afta.DataAccess.DTOs.Marahel
{
    public class TaeedErsalDto
    {
        public int shop { get; set; }
        public int shod { get; set; }
        public int codeMarhale { get; set; }
        public string marhale { get; set; }
        public List<EnumMarhalehTypeInfo> MarahelMandeh { get; set; }
    }
}
