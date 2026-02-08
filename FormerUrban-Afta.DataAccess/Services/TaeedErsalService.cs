namespace FormerUrban_Afta.DataAccess.Services;

public class TaeedErsalService
{
    private readonly IDarkhastService _darkhastService;
    private readonly IErjaService _erjaService;
    private readonly IHistoryLogService _historyLogService;
    public TaeedErsalService(IDarkhastService darkhastService, IErjaService erjaService, IHistoryLogService historyLogService)
    {
        _darkhastService = darkhastService;
        _erjaService = erjaService;
        _historyLogService = historyLogService;
    }

    public async Task<bool> SendToNextMarhaleh(EnumMarhalehTypeInfo nextMarhale, int shod)
    {
        var oDarkhast = await _darkhastService.GetDataByShod(shod);

        var oErja = _erjaService.GetActiveData(shod);

        oErja.ForEach(c => { c.flag = false; });

        _erjaService.UpdateData(oErja);

        _historyLogService.PrepareForInsert($"تایید و ارسال درخواست {shod} یه مرحله {nextMarhale.DisplayName}", EnumFormName.Erja, EnumOperation.Post, shop: oDarkhast.shop, shod: shod);

        return _erjaService.InsertForNextMarhaleh(oDarkhast, nextMarhale);
    }
}
