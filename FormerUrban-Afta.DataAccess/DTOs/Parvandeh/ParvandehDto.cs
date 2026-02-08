namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class ParvandehDto
    {
        public long Identity { get; set; }
        public long Index { get; set; }
        public int shop { get; set; }
        public int mantaghe { get; set; }
        public int hoze { get; set; }
        public int blok { get; set; }
        public int Melk { get; set; }
        public int shomelk { get; set; }
        public int sakhteman { get; set; }
        public int apar { get; set; }
        public int senfi { get; set; }
        public int idparent { get; set; }
        public int? code_tree { get; set; }
        public bool sws { get; set; }
        public int? Formol { get; set; }
        public string codeN { get; set; }
        public int AreaId { get; set; }
        public bool locked { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
