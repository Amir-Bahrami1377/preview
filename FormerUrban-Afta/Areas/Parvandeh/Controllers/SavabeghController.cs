using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers
{
    [Area("Parvandeh")]
    public class SavabeghController : AmardBaseController
    {
        private readonly IDv_SavabeghService _dv_savabeghService;
        private readonly IHistoryLogService _logService;

        public SavabeghController(IDv_SavabeghService dvSavabeghService, IHistoryLogService logService)
        {
            _dv_savabeghService = dvSavabeghService;
            _logService = logService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess("Parvandeh_Savabegh", type: EnumOperation.Get, table: EnumFormName.Dv_savabegh, section: "مشاهده اطلاعات سوابق")]
        public IActionResult Index(int shop, int shod, string NoeParvandeh, int dShop, int codeMarhaleh = 0)
        {
            ViewBag.shop = shop;
            ViewBag.shod = shod;
            ViewBag.NoeParvandeh = NoeParvandeh;
            ViewBag.dShop = dShop;
            ViewBag.codeMarhaleh = codeMarhaleh;
            var result = _dv_savabeghService.GetData(shop, shod);
            _logService.PrepareForInsert("نمایش سوابق",EnumFormName.Dv_savabegh,EnumOperation.Get);
            return View(result);
        }
    }
}
