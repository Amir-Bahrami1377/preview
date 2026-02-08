using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Melk : BaseModel, IBaseFile
    {
        public int shop { get; set; }
        public int radif { get; set; }
        public string? pelakabi { get; set; }
        public string? codeposti { get; set; }
        public string? tel { get; set; }
        public string? address { get; set; }
        public double? masahat_s { get; set; }
        public double? masahat_m { get; set; }
        public double? masahat_e { get; set; }
        public double? masahat_b { get; set; }
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
        public string? tafkiki { get; set; }
        public string? azFari { get; set; }
        public string? fari { get; set; }
        public string? asli { get; set; }
        public string? bakhsh { get; set; }
        public bool? Active { get; set; }
        public double? ArzeshArse { get; set; }
        public int? C_karbariAsli { get; set; }
        public string? KarbariAsli { get; set; }
        public double? utmx { get; set; }
        public double? utmy { get; set; }
        public override string ToString()
        {
            return $"{shop}{radif}{pelakabi?.Trim()}{codeposti?.Trim()}{tel?.Trim()}{address?.Trim()}{masahat_s ?? 0}{masahat_m ?? 0}{masahat_e ?? 0}{masahat_b ?? 0}" +
                   $"{c_mahdodeh ?? 0}{c_marhaleh ?? 0}{mahdodeh?.Trim()}{c_noemelk ?? 0}{noemelk?.Trim()}{sh_Darkhast ?? 0}{tafkiki?.Trim()}{azFari?.Trim()}" +
                   $"{fari?.Trim()}{sabti?.Trim()}{asli?.Trim()}{bakhsh?.Trim()}{Active ?? false}{ArzeshArse ?? 0}{c_vazmelk ?? 0}{vazmelk?.Trim()}" +
                   $"{C_karbariAsli ?? 0}{KarbariAsli?.Trim()}{utmx ?? 0}{utmy ?? 0}{c_vazsanad ?? 0}{vazsanad?.Trim()}{c_noesanad ?? 0}{noesanad?.Trim()}" +
                   $"{CreateDateTime.Ticks}{ModifiedDate?.Ticks ?? 0}{CreateUser}{ModifiedUser}".Trim();
        }
    }
}
