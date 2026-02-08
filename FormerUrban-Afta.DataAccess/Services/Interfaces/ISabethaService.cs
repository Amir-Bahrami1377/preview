using FormerUrban_Afta.DataAccess.DTOs.Sabetha;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ISabethaService
    {
        public List<SabethaDto> GetRows(string enumName);
    }
}
