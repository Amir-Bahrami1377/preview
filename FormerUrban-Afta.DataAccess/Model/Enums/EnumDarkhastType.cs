using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormerUrban_Afta.DataAccess.Model.Enums
{
    public enum EnumDarkhastType
    {
        [Display(Name = " ")]
        None,  //0

        [Display(Name = "پاسخ استعلام")]
        InquiryResponse, //1

        [Display(Name = "صدور پروانه")]
        IssuanceLicense, //2
    }
    public class EnumDarkhastTypeInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string DisplayName { get; set; }
    }
}
