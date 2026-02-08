namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class Dv_savabeghDTO
    {
        public long Identity { get; set; }
        public int shop { get; set; }
        public int d_radif { get; set; }
        public string mtable_name { get; set; }
        public Nullable<int> c_noe { get; set; }
        public string noe { get; set; }
        public int sh_darkhast { get; set; }
        public string? CreateDateTime { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
