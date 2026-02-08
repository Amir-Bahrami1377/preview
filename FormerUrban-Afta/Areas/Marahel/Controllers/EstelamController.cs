using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Marahel.Controllers
{
    [Area("Marahel")]
    public class EstelamController : AmardBaseController
    {
        private readonly IValidator<EstelamDto> _validatorEstelam;
        private readonly ILogger<EstelamController> _logger;
        private readonly IEstelamService _estelamService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IEventLogThresholdService _eventLogThresholdService;

        public EstelamController(IHistoryLogService historyLogService, IEstelamService estelamService, ILogger<EstelamController> logger, IValidator<EstelamDto> validatorEstelam, IEventLogThresholdService eventLogThresholdService)
        {
            _historyLogService = historyLogService;
            _estelamService = estelamService;
            _logger = logger;
            _validatorEstelam = validatorEstelam;
            _eventLogThresholdService = eventLogThresholdService;
        }

        [CheckUserAccess("Darkhast_Detaile", type: EnumOperation.Get, table: EnumFormName.Estelam, section: "مرحله پاسخ استعلام")]
        public async Task<IActionResult> Index(MarahelDto model)
        {
            var data = await _estelamService.GetByRequestNumberAsync(model.shod);
            data.shop = model.shop;
            data.Sh_Darkhast = model.shod;
            data.codeMarhaleh = model.codeMarhaleh;
            _historyLogService.PrepareForInsert(description: $"مشاهده مرحله پاسخ استعلام شماره درخواست {model.shod} و شماره پرونده {model.shop}", formName: EnumFormName.Estelam, operation: EnumOperation.Get);
            return View(model: data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess("Estelam_Edit", type: EnumOperation.Update, table: EnumFormName.Estelam, section: "ویرایش اطلاعات مرحله پاسخ استعلام")]
        public async Task<IActionResult> Update(EstelamDto estelam)
        {
            ValidationResult result = _validatorEstelam.Validate(estelam);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert(
                    description: $"خطا در ویرایش اطلاعات مرحله پاسخ اسعلام شماره درخواست  {estelam.Sh_Darkhast} و شماره پرونده {estelam.shop}",
                    formName: EnumFormName.Estelam, operation: EnumOperation.Update);
                estelam.message = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View("Index", estelam);
            }

            await _estelamService.UpdateAsync(estelam);
            _historyLogService.PrepareForInsert(
                description: $"ویرایش اطلاعات مرحله پاسخ اسعلام شماره درخواست {estelam.Sh_Darkhast} و شماره پرونده {estelam.shop} با موفقیت انجام شد.",
                formName: EnumFormName.Estelam, operation: EnumOperation.Update);
            TempData["SuccessMessage"] = $"ویرایش مرحله پاسخ استعلام درخواست {estelam.Sh_Darkhast} با موفقیت انجام شد.";
            return RedirectToAction("Index", new { shop = estelam.shop, shod = estelam.Sh_Darkhast, codeMarhaleh = estelam.codeMarhaleh });
        }

    }
}
