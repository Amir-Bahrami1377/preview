using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.Areas.Marahel.Controllers
{
    [Area("Marahel")]
    public class VisitController : Controller
    {
        private readonly IHistoryLogService _historyLogService;
        private readonly IEventLogThresholdService _eventLogThresholdService;

        public VisitController(IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService)
        {
            _historyLogService = historyLogService;
            _eventLogThresholdService = eventLogThresholdService;
        }

        [CheckUserAccess("Darkhast_Detaile", type: EnumOperation.Get, table: EnumFormName.Visit, section: "مرحله بازدید")]
        public async Task<IActionResult> Index(MarahelDto model)
        {

            _historyLogService.PrepareForInsert(description: $"مشاهده مرحله بازدید درخواست {model.shod}", formName: EnumFormName.Visit, operation: EnumOperation.Get);
            model.codeMarhaleh = 1;
            return View(model);
        }
    }
}
