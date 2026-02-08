using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Darkhast.Controllers
{
    [Area("Darkhast")]
    public class SabtDarkhastController : AmardBaseController
    {
        private readonly IValidator<DarkhastDTO> _darkhastValidator;
        private readonly IDarkhastService _darkhastService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IErjaService _erja;
        private readonly IParvandehService _parvandehService;

        public SabtDarkhastController(IValidator<DarkhastDTO> darkhastValidator, IDarkhastService darkhastService,
             IHistoryLogService historyLogService, IErjaService erja, IParvandehService parvandehService)
        {
            _darkhastValidator = darkhastValidator;
            _darkhastService = darkhastService;
            _historyLogService = historyLogService;
            _erja = erja;
            _parvandehService = parvandehService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "SearchParvandeh_SabtDarkhast", type: EnumOperation.Get, table: EnumFormName.Darkhast, section: "ثبت درخواست")]
        public IActionResult Index(int shop, string codeNosazi)
        {
            var model = new DarkhastDTO
            {
                shop = shop,
                shodarkhast = _darkhastService.GetLastShod(),
                c_nosazi = codeNosazi,
                EnumDarkhast = GetDarkhastType(),
            };
            _historyLogService.PrepareForInsert($"مشاهده ایجاد درخواست برای پرونده {shop} با کد نوسازی {codeNosazi}", EnumFormName.Darkhast, EnumOperation.Get);
            return View(model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "SearchParvandeh_SabtDarkhast", type: EnumOperation.Post, table: EnumFormName.Darkhast, section: "ثبت درخواست")]
        public async Task<IActionResult> AddDarkhast(DarkhastDTO darkhast)
        {
            var result = _darkhastValidator.Validate(darkhast);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا اعتبارسنجی در ثبت درخواست برای پرونده {darkhast.shop}", EnumFormName.Darkhast,EnumOperation.Post);
                TempData["ValidateMessage"] = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage));
                darkhast.EnumDarkhast = GetDarkhastType();
                return View("Index", darkhast);
            }

            var er = _erja.GetDataWithShopNoeDarkhastVaziatErja(darkhast.shop, (int)darkhast.c_noedarkhast, 1);
            if (er > 0)
            {
                ViewBag.ErrorMessage = $"برای این پرونده درخواست {er} یا همین نوع درخواست در جریان است";
                darkhast.EnumDarkhast = GetDarkhastType();
                _historyLogService.PrepareForInsert($"خطا در ثبت درخواست شماره {darkhast.shodarkhast} برای پرونده {darkhast.shop} به علت موجود بودن نوع درخواست مشترک.", EnumFormName.Darkhast, EnumOperation.Post);
                return View("Index", darkhast);
            }

            if (_darkhastService.IsExistRequestNumber(darkhast.shodarkhast))
            {
                ViewBag.ErrorMessage = $"این شماره درخواست قبلا استفاده شده است";
                _historyLogService.PrepareForInsert($"تکراری بودن شماره درخواست {darkhast.shodarkhast} در نرم افزار", EnumFormName.Darkhast, EnumOperation.Post);
                darkhast.EnumDarkhast = GetDarkhastType();
                return View("Index", darkhast);
            }

            var shod_mojud = _parvandehService.GetShodMojud(darkhast.shop);
            _parvandehService.copyForSabtDarkhast(darkhast.shop, shod_mojud, darkhast.shodarkhast);

            var darkhastType = GetDarkhastType();
            darkhast.noedarkhast = darkhastType.Find(e => e.Index == darkhast.c_noedarkhast)?.DisplayName;

            var resDarkhast = await _darkhastService.AddDarkhast(darkhast);
            if (!resDarkhast)
            {
                ViewBag.ErrorMessage = $"خطایی در ثیت درخواست رخ داده است";
                darkhast.EnumDarkhast = GetDarkhastType();
                return View("Index", darkhast);
            }

            var resErja = _erja.InsertForSabtDarkhast(darkhast);
            if (!resErja)
            {
                ViewBag.ErrorMessage = $"خطایی در ثیت ارجاع رخ داده است";
                darkhast.EnumDarkhast = GetDarkhastType();
                return View("Index", darkhast);
            }

            _historyLogService.PrepareForInsert(
                $"نوع درخواست {darkhast.noedarkhast} با شماره درخواست {darkhast.shodarkhast} برای پرونده {darkhast.shop} برای متقاضی {darkhast.moteghazi} با موفقیت ثبت شد.",
                EnumFormName.Darkhast,EnumOperation.Post);

            TempData["SuccessMessage"] = $"درخواست {darkhast.shodarkhast} با موفقیت برای پرونده {darkhast.shop} ثبت شد.";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private List<EnumDarkhastTypeInfo> GetDarkhastType()
        {
            var type = Enum.GetValues(typeof(EnumDarkhastType))
                .Cast<EnumDarkhastType>()
                .Select(e => new EnumDarkhastTypeInfo
                {
                    Name = e.ToString(),
                    Index = (int)e,
                    DisplayName = e.GetType()
                        .GetMember(e.ToString())[0]
                        .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                })
                .ToList();
            return type;
        }

    }
}
