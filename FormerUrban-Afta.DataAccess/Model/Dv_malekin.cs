using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Dv_malekin : BaseModel
    {
        public int shop { get; set; }
        public int d_radif { get; set; }
        public int id { get; set; }
        public string mtable_name { get; set; }
        public int? c_noemalek { get; set; }
        public string noemalek { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public string? father { get; set; }
        public string? sh_sh { get; set; }
        public string kodemeli { get; set; }
        public string? tel { get; set; }
        public string mob { get; set; }
        public double? sahm_a { get; set; }
        public double? dong_a { get; set; }
        public double? sahm_b { get; set; }
        public double? dong_b { get; set; }
        public string address { get; set; }
        public double? ArzeshArse { get; set; }
        public double? ArzeshAyan { get; set; }
        public double? meghdarsahmarse { get; set; }
        public double? meghdarsahmayan { get; set; }


        public override string ToString()
        {
            return string.Join("",
                shop, d_radif, id, mtable_name?.Trim(), c_noemalek ?? 0, noemalek?.Trim(), name?.Trim(),
                family?.Trim(), father?.Trim(), sh_sh?.Trim(), kodemeli?.Trim(), tel?.Trim(), mob?.Trim(),
                 sahm_a ?? 0, dong_a ?? 0, sahm_b ?? 0, dong_b ?? 0, address?.Trim(), meghdarsahmayan ?? 0,
                 meghdarsahmarse ?? 0, ArzeshArse ?? 0, ArzeshAyan ?? 0,
                 CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();
        }

    }
}
