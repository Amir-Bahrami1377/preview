using FormerUrban_Afta.DataAccess.DTOs.Sabetha;

namespace FormerUrban_Afta.DataAccess.DTOs.Parvandeh
{
    public class Dv_karbariDTO : Dv_karbari
    {
        public bool IsValid { get; set; } = true;
        public int CodeMarhale { get; set; }
        public List<SabethaDto> ItemTabagheh { get; set; }
        public List<SabethaDto> ItemKarbari { get; set; }
        public List<SabethaDto> ItemNoeestefadeh { get; set; }
        public List<SabethaDto> ItemNoesakhteman { get; set; }
        public List<SabethaDto> ItemNoesazeh { get; set; }
        public List<SabethaDto> ItemMarhaleh { get; set; }


    }
}
