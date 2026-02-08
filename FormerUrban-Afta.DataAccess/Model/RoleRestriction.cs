using FormerUrban_Afta.DataAccess.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormerUrban_Afta.DataAccess.Model;

public class RoleRestriction:BaseModel
{
    public string RoleId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? FromDate { get; set; }  // Nullable to allow always-active blocks
    public DateTime? ToDate { get; set; }    // Nullable to allow indefinite blocks
    public override string ToString()
    {
        return string.Join("",
            RoleId, Description, FromDate?.Ticks ?? 0, ToDate?.Ticks ?? 0,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser).Trim();
    }
}
