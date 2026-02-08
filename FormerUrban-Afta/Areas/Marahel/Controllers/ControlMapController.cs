using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Marahel.Controllers
{
    [Area("Marahel")]
    public class ControlMapController : AmardBaseController
    {
        private readonly ControlMapService _controlMapService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IValidator<ControlMapDto> _validator;
        private readonly ILogger<ControlMapController> _logger;
        public ControlMapController(ControlMapService controlMapService, IHistoryLogService historyLogService, IValidator<ControlMapDto> validator, ILogger<ControlMapController> logger)
        {
            _controlMapService = controlMapService;
            _historyLogService = historyLogService;
            _validator = validator;
            _logger = logger;
        }

        [CheckUserAccess("Darkhast_Detaile", type: EnumOperation.Get, table: EnumFormName.ControlMap, section: "مرحله کنترل نقشه")]
        public async Task<IActionResult> Index(MarahelDto model)
        {
            if (model.shod == 0 || model.shop == 0)
            {
                _historyLogService.PrepareForInsert(description: $"خطا در ورود به مرحله کنترل نقشه شماره درخواست {model.shod} یا شماره پرونده {model.shop}",
                    formName: EnumFormName.ControlMap, operation: EnumOperation.Get);
                throw new Exception();
            }

            ControlMapDto data;
            var exsit = _controlMapService.Exists(model.shod);
            if (exsit)
                data = _controlMapService.GetData(model.shod, model.shop);
            else
            {
                data = await _controlMapService.CreateNew(model.shod, model.shop);
                if (data == null)
                {
                    _historyLogService.PrepareForInsert(description: $"خطا در ایجاد کنترل نقشه درخواست {data?.sh_Darkhast}", formName: EnumFormName.ControlMap, operation: EnumOperation.Get);
                    return BadRequest("خطایی رخ داده است لطفا با پشتیبانی تماس بگیرید");
                }
            }

            _historyLogService.PrepareForInsert(description: $"مشاهده مرحله کنترل نقشه شماره درخواست {model.shod} و شماره پرونده {model.shop}", formName: EnumFormName.ControlMap, operation: EnumOperation.Get);
            data.codeMarhaleh = model.codeMarhaleh;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess("ControlMap_Edit", type: EnumOperation.Update, table: EnumFormName.ControlMap, section: "ویرایش اطلاعات مرحله کنترل نقشه")]
        public IActionResult Update(ControlMapDto controlMapDto)
        {
            ValidationResult result = _validator.Validate(controlMapDto);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert(
                    description: $"خطای اعتبار سنجی در ویرایش اطلاعات مرحله کنترل نقشه درخواست {controlMapDto.sh_Darkhast}, پرونده {controlMapDto.shop}",
                    formName: EnumFormName.ControlMap, operation: EnumOperation.Update);

                controlMapDto.message = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View("Index", controlMapDto);
            }
            var res = _controlMapService.UpdateByModel(controlMapDto);
            if (!res)
            {
                _historyLogService.PrepareForInsert(
                    description: $"خطای ویرایش اطلاعات مرحله کنترل نقشه درخواست {controlMapDto.sh_Darkhast}, پرونده {controlMapDto.shop}",
                    formName: EnumFormName.ControlMap, operation: EnumOperation.Update);
                controlMapDto.message = new List<string> { "خطایی رخ داده است" };
                return View("Index", controlMapDto);
            }
            _historyLogService.PrepareForInsert(
                description: $"ویرایش اطلاعات مرحله کنترل نقشه درخواست {controlMapDto.sh_Darkhast}, پرونده {controlMapDto.shop} با موفقیت انجام شد.",
                formName: EnumFormName.ControlMap, operation: EnumOperation.Update);
            TempData["SuccessMessage"] = $"ویرایش مرحله کنترل نقشه درخواست {controlMapDto.sh_Darkhast} با موفقیت انجام شد.";
            return RedirectToAction("Index", new { shop = controlMapDto.shop, shod = controlMapDto.sh_Darkhast, codeMarhaleh = controlMapDto.codeMarhaleh });
        }

    }
}
