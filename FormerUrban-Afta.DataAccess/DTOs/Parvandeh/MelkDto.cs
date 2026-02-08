namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class MelkDto
    {
        public long Identity { get; set; }
        public int shop { get; set; }
        public int radif { get; set; }
        public int? pelakabi { get; set; }
        public string? codeposti { get; set; }
        public string? tel { get; set; }
        public string? address { get; set; }
        public double masahat_s { get; set; }
        public double masahat_m { get; set; }
        public double masahat_e { get; set; }
        public double masahat_b { get; set; }
        public int? c_vazsanad { get; set; }
        public string? vazsanad { get; set; }
        public int? c_noesanad { get; set; }
        public string? noesanad { get; set; }
        public string? sabti { get; set; }
        public int? c_vazmelk { get; set; }
        public string? vazmelk { get; set; }
        public int? c_mahdodeh { get; set; }
        public string? mahdodeh { get; set; }
        public int? c_noemelk { get; set; }
        public string? noemelk { get; set; }
        public int? c_marhaleh { get; set; }
        public string? marhaleh { get; set; }
        public long? sh_Darkhast { get; set; }
        public int? tafkiki { get; set; }
        public int? azFari { get; set; }
        public int? fari { get; set; }
        public int? asli { get; set; }
        public int? bakhsh { get; set; }
        public bool Active { get; set; }
        public double ArzeshArse { get; set; }
        public int? C_karbariAsli { get; set; }
        public string? KarbariAsli { get; set; }
        public int utmx { get; set; }
        public int utmy { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string>? message { get; set; }
        public int codeMarhaleh { get; set; }
    }
}
