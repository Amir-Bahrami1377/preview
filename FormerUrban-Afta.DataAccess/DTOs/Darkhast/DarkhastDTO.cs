
using FormerUrban_Afta.DataAccess.Model.Enums;

namespace FormerUrban_Afta.DataAccess.DTOs.Darkhast
{
    public class DarkhastDTO : Model.Darkhast
    {
        public bool IsValid { get; set; } = true;
        public List<EnumDarkhastTypeInfo>? EnumDarkhast { get; set; }
        public List<string>? message { get; set; }
    }
}
