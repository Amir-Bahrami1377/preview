namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class Dv_malekinDTO
    {
        public long Identity { get; set; }
        public int shop { get; set; }
        public int d_radif { get; set; }
        public int id { get; set; }
        public string mtable_name { get; set; }
        public int? c_noemalek { get; set; }
        public string noemalek { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public string father { get; set; }
        public string sh_sh { get; set; }
        public string kodemeli { get; set; }
        public string tel { get; set; }
        public string mob { get; set; }
        public double? sahm_a { get; set; }
        public double? dong_a { get; set; }
        public double? sahm_b { get; set; }
        public double? dong_b { get; set; }
        public string address { get; set; }
        public double? ArzeshArse { get; set; }
        public double? ArzeshAyan { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
