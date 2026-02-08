using FormerUrban_Afta.DataAccess.Model.BaseEntity;
using System.Net;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Sakhteman : BaseModel, IBaseFile
    {
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
        public Nullable<double> TedadTabaghe { get; set; }
        public Nullable<double> MasahatZirbana { get; set; }
        public Nullable<double> MasahatArse { get; set; }


        public override string ToString()
        {
            return string.Join("", shop, radif, masahatkol ?? 0, NoeSakhteman?.Trim(), c_NoeSakhteman ?? 0,
                c_NoeSaze ?? 0, noenama?.Trim(), noesaghf?.Trim(), tarakom ?? 0, satheshghal ?? 0, marhaleh?.Trim(),
                 sh_Darkhast ?? 0, Active ?? false, c_noenama ?? 0, c_noesaghf ?? 0, c_marhaleh ?? 0, NoeSaze?.Trim(),
                TarikhEhdas?.Trim(), ArzeshAyan ?? 0, TedadTabaghe ?? 0, MasahatZirbana ?? 0, MasahatArse ?? 0,
                CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();
        }
    }
}
