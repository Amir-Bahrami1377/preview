using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;

    public interface IHistoryLogService
    {
        History PrepareForInsert(string description, EnumFormName formName, EnumOperation operation, int? shop = 0, int? shod = 0);
        Task<List<HistoryDto>> SearchAsync(SearchHistoryDto search);
        Task<SearchHistoryDto> GetDrp(SearchHistoryDto command);
    }

