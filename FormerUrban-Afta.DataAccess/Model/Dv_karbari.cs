using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Dv_karbari : BaseModel
    {
        public int shop { get; set; }
        public int d_radif { get; set; }
        public int id { get; set; }
        public string mtable_name { get; set; }
        public Nullable<int> c_tabagheh { get; set; }
        public string tabagheh { get; set; }
        public Nullable<int> c_karbari { get; set; }
        public string karbari { get; set; }
        public Nullable<int> c_noeestefadeh { get; set; }
        public string noeestefadeh { get; set; }
        public Nullable<double> masahat_k { get; set; }
        public Nullable<int> c_noesakhteman { get; set; }
        public string noesakhteman { get; set; }
        public Nullable<int> c_noesazeh { get; set; }
        public string noesazeh { get; set; }
        public Nullable<int> c_marhaleh { get; set; }
        public string marhaleh { get; set; }
        public string tarikhehdas { get; set; }

        public override string ToString()
        {
            return $"{shop}{d_radif}{id}{mtable_name?.Trim()}{c_tabagheh ?? 0}{tabagheh?.Trim()}{c_karbari ?? 0}{karbari?.Trim()}{c_noeestefadeh ?? 0}" +
                   $"{masahat_k ?? 0}{noeestefadeh?.Trim()}{c_noesakhteman ?? 0}{noesakhteman?.Trim()}{c_noesazeh ?? 0}{noesazeh?.Trim()}{c_marhaleh ?? 0}" +
                   $"{marhaleh?.Trim()}{tarikhehdas?.Trim()}{CreateDateTime.Ticks}{ModifiedDate?.Ticks ?? 0}{CreateUser}{ModifiedUser}".Trim();
        }
    }
}
