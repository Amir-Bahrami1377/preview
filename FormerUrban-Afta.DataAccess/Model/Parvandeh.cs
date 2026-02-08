using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Parvandeh : BaseModel
    {
        public double shop { get; set; }
        public int mantaghe { get; set; }
        public int hoze { get; set; }
        public int blok { get; set; }
        public int shomelk { get; set; }
        public int sakhteman { get; set; }
        public int apar { get; set; }
        public int senfi { get; set; }
        public int idparent { get; set; }
        public int code_tree { get; set; }
        public bool sws { get; set; }
        public int? Formol { get; set; }
        public string codeN { get; set; }
        public int AreaId { get; set; }
        public bool locked { get; set; }

        public override string ToString()
        {
            return string.Join("",
                shop, mantaghe, hoze, blok, shomelk, sakhteman, apar, senfi,
                idparent, code_tree, sws, Formol, codeN?.Trim(), AreaId, locked,
                CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
                CreateUser, ModifiedUser).Trim();
        }
    }
}