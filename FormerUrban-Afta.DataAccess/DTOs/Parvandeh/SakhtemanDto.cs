namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class SakhtemanDto
    {
        public long Identity { get; set; }
        public int shop { get; set; }
        public int radif { get; set; }
        public Nullable<double> masahatkol { get; set; }
        public Nullable<int> c_noenama { get; set; }
        public string noenama { get; set; }
        public Nullable<int> c_noesaghf { get; set; }
        public string noesaghf { get; set; }
        public Nullable<double> tarakom { get; set; }
        public Nullable<double> satheshghal { get; set; }
        public Nullable<int> c_marhaleh { get; set; }
        public string marhaleh { get; set; }
        public Nullable<long> sh_Darkhast { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> c_NoeSakhteman { get; set; }
        public string NoeSaze { get; set; }
        public Nullable<int> c_NoeSaze { get; set; }
        public string? TarikhEhdas { get; set; }
        public Nullable<double> ArzeshAyan { get; set; }
        public string NoeSakhteman { get; set; }
        public int? TedadTabaghe { get; set; }
        public Nullable<double> MasahatZirbana { get; set; }
        public Nullable<double> MasahatArse { get; set; }
        public List<string>? message { get; set; }
        public bool IsValid { get; set; } = true;
        public int shod { get; set; }
        public int dShop { get; set; }
    }
}
