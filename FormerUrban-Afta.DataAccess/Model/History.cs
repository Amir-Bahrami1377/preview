using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class History : BaseModel
    {
        public int? shop { get; set; }
        public int? shod { get; set; }
        public string user_name { get; set; }
        public int? tarikh { get; set; }
        public string saat { get; set; }
        public string sharh { get; set; }
        public string name_karbar { get; set; }
        public EnumFormName name_form { get; set; }
        public EnumOperation noeamal { get; set; }
        public string IPAddress { get; set; }
        public string CNosazi { get; set; }

        public override string ToString()
        {
            return string.Join("", shop ?? 0, shod ?? 0, user_name?.Trim(), tarikh ?? 0, saat?.Trim(),
                sharh?.Trim(), name_karbar?.Trim(), name_form, noeamal, IPAddress?.Trim(),
                CNosazi?.Trim(), CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
                CreateUser, ModifiedUser).Trim();
        }

    }
}
