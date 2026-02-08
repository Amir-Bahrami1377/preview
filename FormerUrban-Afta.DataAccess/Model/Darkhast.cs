using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Darkhast : BaseModel
    {
        public int shop { get; set; }
        public int shodarkhast { get; set; }
        public string? noedarkhast { get; set; }
        public int c_noedarkhast { get; set; }
        public string? noemot { get; set; }
        public int c_noemot { get; set; }
        public string address { get; set; }
        public string? tel { get; set; }
        public string mob { get; set; }
        public string moteghazi { get; set; }
        public string? codeposti { get; set; }
        public string? email { get; set; }
        public string c_nosazi { get; set; }
        public string CodeMeli { get; set; }

        public override string ToString()
        {
            return string.Join("",
                shop, shodarkhast, noedarkhast?.Trim(), c_noedarkhast, noemot?.Trim(), c_noemot,
                address?.Trim(), tel?.Trim(), mob?.Trim(), CodeMeli?.Trim(), moteghazi?.Trim(),
                codeposti?.Trim(), email?.Trim(), c_nosazi?.Trim(), CreateDateTime.Ticks,
                ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();
        }
    }
}
