using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Apartman : BaseModel, IBaseFile
    {
        public int shop { get; set; }
        public int radif { get; set; }
        public int pelakabi { get; set; }
        public string codeposti { get; set; }
        public string tel { get; set; }
        public string address { get; set; }
        public int? c_noesanad { get; set; }
        public string? noesanad { get; set; }
        public int? c_vazsanad { get; set; }
        public string? vazsanad { get; set; }
        public int? c_noemalekiyat { get; set; }
        public string? noemalekiyat { get; set; }
        public string sabti { get; set; }
        public double? MasahatKol { get; set; }
        public double? MasahatArse { get; set; }
        public long? sh_Darkhast { get; set; }
        public int? tafkiki { get; set; }
        public int? azFari { get; set; }
        public int? fari { get; set; }
        public int? asli { get; set; }
        public int? bakhsh { get; set; }
        public bool? Active { get; set; }
        public string? NoeSaze { get; set; }
        public int? c_NoeSaze { get; set; }
        public int? C_Jahat { get; set; }
        public string? jahat { get; set; }

        public override string ToString()
        {
            return string.Join("",
                shop, radif, pelakabi, codeposti?.Trim(), tel?.Trim(), address?.Trim(),
                c_noesanad, noesanad?.Trim(), c_vazsanad, vazsanad?.Trim(), c_noemalekiyat,
                noemalekiyat?.Trim(), sabti?.Trim(), MasahatKol, MasahatArse, sh_Darkhast,
                tafkiki, azFari, fari, asli, bakhsh, Active,
                NoeSaze?.Trim(), c_NoeSaze, C_Jahat, jahat?.Trim(),
                CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
                CreateUser, ModifiedUser).Trim();
        }

    }
}
