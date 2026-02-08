using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;

[Area("Parvandeh")]
public class ParvandehInfoController : AmardBaseController
{
    private readonly ILogger<ParvandehInfoController> _logger;
    private readonly IParvandehService _parvandehService;
    private readonly IMelkService _melkService;
    private readonly ISakhtemanService _sakhtemanService;
    private readonly IApartmanService _apartmanService;
    private readonly IHistoryLogService _historyLogService;
    private readonly MyFunctions _myFunctions;
    private readonly IValidator<SearchParvandehDto> _searchValidator;
    private readonly IValidator<ParvandehDto> _validator;

    private readonly IEventLogThresholdService _eventLogThresholdService;


    public ParvandehInfoController(ILogger<ParvandehInfoController> logger, IParvandehService parvandehService,
        IMelkService melkService, IApartmanService apartmanService, ISakhtemanService skhtemanService,
        IHistoryLogService historyLogService, MyFunctions myFunctions, IValidator<SearchParvandehDto> searchValidator, IValidator<ParvandehDto> validator)
    {
        _logger = logger;
        _parvandehService = parvandehService;
        _melkService = melkService;
        _apartmanService = apartmanService;
        _sakhtemanService = skhtemanService;
        _historyLogService = historyLogService;
        _myFunctions = myFunctions;
        _searchValidator = searchValidator;
        _validator = validator;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Menu_ParvandehInfo", type: EnumOperation.Get, table: EnumFormName.Parvandeh, section: "اطلاعات پرونده")]
    public IActionResult Index(int shop = 0, string CodeN = "", bool Asnaf = false, long shod = 0)
    {
        var model = shop > 0 ? _parvandehService.GetRow(shop.ToString()) : new ParvandehDto();
        if (shop > 0)
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات پرونده {shop} ", EnumFormName.Parvandeh, EnumOperation.Get, (int)shop, (int)shod);
        else
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات پرونده", EnumFormName.Parvandeh, EnumOperation.Get);
        return PartialView(model);
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Menu_ParvandehInfo", type: EnumOperation.Get, table: EnumFormName.Parvandeh, section: "مشاهده جزئیات پرونده")]
    public IActionResult ParvandehInfo(int shop)
    {
        var model = shop > 0 ? _parvandehService.GetRow(shop.ToString()) : new ParvandehDto();
        if (shop > 0)
            _historyLogService.PrepareForInsert($"نمایش ریز اطلاعات پرونده {shop} ", EnumFormName.Parvandeh, EnumOperation.Get, (int)shop, 0);
        return PartialView("ParvandehInfo", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_CreateParvandeh", type: EnumOperation.Get, table: EnumFormName.Parvandeh, section: "مشاهده ایجاد پرونده")]
    public IActionResult CreateParvandeh(int index, long shop, string codeN)
    {

        if (index == 5)
        {
            var noeParvande = _myFunctions.GetNoeParvandeh(shop);
            if (noeParvande == 4)
            {
                _historyLogService.PrepareForInsert($"خطا در ایجاد پرونده {shop} ", EnumFormName.Parvandeh, EnumOperation.Get, (int)shop, 0);
                return new JsonResult(new { success = false, message = "ساختمان یا آپارتمان را انتخاب نمایید." });
            }
        }

        var parvandeh = new ParvandehDto();

        if (index > 2)
        {
            var code = codeN.Split('-');

            parvandeh.mantaghe = code[0] == "0" ? 0 : Convert.ToInt32(code[0]);
            parvandeh.hoze = code[1] == "0" ? 0 : Convert.ToInt32(code[1]);
            parvandeh.blok = code[2] == "0" ? 0 : Convert.ToInt32(code[2]);
            parvandeh.Melk = code[3] == "0" ? 0 : Convert.ToInt32(code[3]);
            parvandeh.sakhteman = code[4] == "0" ? 0 : Convert.ToInt32(code[4]);
            parvandeh.apar = code[5] == "0" ? 0 : Convert.ToInt32(code[5]);
            parvandeh.senfi = code[6] == "0" ? 0 : Convert.ToInt32(code[6]);
        }

        parvandeh.Index = index;
        parvandeh.shop = (int)shop;
        parvandeh.codeN = codeN;

        _historyLogService.PrepareForInsert($"نمایش فرم ایجاد پرونده با کد شماره {shop}", EnumFormName.Parvandeh, EnumOperation.Get, (int)shop, 0);
        return PartialView("CreateParvandeh", parvandeh);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_CreateParvandeh", type: EnumOperation.Post, table: EnumFormName.Parvandeh, section: "ایجاد پرونده")]
    public async Task<IActionResult> Insert_Parvandeh(ParvandehDto model)
    {
        try
        {
            if (model.Index is < 2 or > 4)
                return new JsonResult(new { success = false, message = "لطفا نوع پرونده را به درستی انتخاب نمایید!" });

            var ruleSet = model.Index switch
            {
                2 => "melk",
                3 => "sakhteman",
                4 => "aparteman"
            };

            if (ruleSet != null)
            {
                var result = await _validator.ValidateAsync(model, options => options.IncludeRuleSets(ruleSet));
                if (!result.IsValid)
                {
                    _historyLogService.PrepareForInsert($"خطای اعتبار سنجی در ایجاد پرونده", EnumFormName.Parvandeh, EnumOperation.Get);
                    return new JsonResult(new { success = false, message = result.Errors.Select(x => x.ErrorMessage).ToArray() });
                }
            }

            var intCount = _parvandehService.CheckCountParvandeh(model.mantaghe, model.hoze, model.blok, model.Melk,
                model.sakhteman, model.apar, model.senfi);

            if (intCount > 0)
            {
                var strError = model.Index switch
                {
                    2 => "شماره ملک قبلاً تعریف شده است",
                    3 => "شماره ساختمان قبلاً تعریف شده است",
                    4 => "شماره آپارتمان قبلاً تعریف شده است",
                    5 => "شماره صنفی قبلاً تعریف شده است",
                    _ => "خطای نامشخص"
                };
                _historyLogService.PrepareForInsert($"خطا در ایجاد پرونده : {strError}", EnumFormName.Parvandeh, EnumOperation.Post);
                return new JsonResult(new { success = false, message = strError });
            }

            var intShoP = _parvandehService.GetMaxShop();
            var parvandeh = new ParvandehDto()
            {
                shop = intShoP,
                mantaghe = model.mantaghe,
                hoze = model.hoze,
                blok = model.blok,
                shomelk = model.Melk,
                sakhteman = model.sakhteman,
                apar = model.apar,
                senfi = model.senfi,
                code_tree = model.shop,
                sws = true,
                AreaId = 1,
                codeN = $"{model.mantaghe}-{model.hoze}-{model.blok}-{model.Melk}-{model.sakhteman}-{model.apar}-{model.senfi}"
            };

            switch (model.Index)
            {
                case 2:
                    parvandeh.idparent = 0;
                    var melkDto = new MelkDto
                    {
                        shop = intShoP,
                        radif = 1,
                        sh_Darkhast = 0,
                        Active = true
                    };
                    await _melkService.InsetByModel(melkDto);
                    _historyLogService.PrepareForInsert(
                        "ایجاد کد نوسازی ملک برای منطقه " + model.mantaghe + "، محله " + model.hoze + "، بلوک " + model.blok +
                        " و شماره ملک " + model.Melk, EnumFormName.Parvandeh, EnumOperation.Post, intShoP, 0);
                    break;
                case 3:
                    parvandeh.idparent = 1;
                    var sakhteman = new SakhtemanDto
                    {
                        shop = intShoP,
                        radif = 1,
                        sh_Darkhast = 0,
                        Active = true,
                        NoeSakhteman = "",
                        NoeSaze = "",
                        marhaleh = "",
                        noenama = "",
                        noesaghf = ""
                    };
                    _sakhtemanService.InsetByModel(sakhteman);
                    _historyLogService.PrepareForInsert(
                        "ایجاد کد نوسازی ساختمان برای منطقه " + model.mantaghe + "، محله " + model.hoze + "، بلوک " +
                        model.blok + " ، شماره ملک " + model.Melk + " و ساختمان " + model.sakhteman, EnumFormName.Parvandeh,
                        EnumOperation.Post, intShoP, 0);
                    break;
                case 4:
                    {
                        var parvandehMelk = _parvandehService.GetRow(model.mantaghe, model.hoze, model.blok, model.Melk, 0, 0, 0);
                        var shopMelk = (int)(parvandehMelk?.shop ?? 0);
                        var melk = await _melkService.GetDataByShop(shopMelk);
                        parvandeh.Formol = _parvandehService.GetRow(model.mantaghe, model.hoze, model.blok, model.Melk, 0, 0, 0).Formol;
                        parvandeh.idparent = 2;
                        var apartman = new ApartmanDto()
                        {
                            shop = intShoP,
                            radif = 1,
                            sh_Darkhast = 0,
                            Active = true,
                            fari = melk.fari,
                            asli = melk.asli,
                            azFari = melk.azFari,
                            bakhsh = melk.bakhsh,
                            tafkiki = melk.tafkiki,
                            vazsanad = "",
                            NoeSaze = "",
                            tel = "",
                            sabti = "",
                            pelakabi = 0,
                            noesanad = "",
                            noemalekiyat = "",
                            codeposti = "",
                            address = "",
                        };
                        await _apartmanService.InsertByModel(apartman);

                        _historyLogService.PrepareForInsert(
                            "ایجاد کد نوسازی آپارتمان برای منطقه " + model.mantaghe + "، محله " + model.hoze +
                            "، بلوک " + model.blok + " ، شماره ملک " + model.Melk + " ، ساختمان " + model.sakhteman +
                            " و آپارتمان " + model.apar, EnumFormName.Parvandeh, EnumOperation.Post, intShoP, 0);
                        break;
                    }
            }

            if (model.senfi == 0 && model.shop > 0)
            {
                var oPar = _parvandehService.GetRow(model.shop.ToString());
                if (oPar == null)
                    return new JsonResult(new { success = false, error = "شماره پرونده پدر نامعتبر است" });

                oPar.sws = false;
                _parvandehService.UpdateByModel(oPar);
            }

            _parvandehService.InsertByModel(parvandeh);
            TempData["SuccessMessage"] = $"پرونده با شماره {intShoP} با موفقیت ایجاد شد.";
            return new JsonResult(new { success = true, Error = "true", shop = intShoP });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, Error = ex.Message });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Menu_SearchParvandeh", type: EnumOperation.Get, table: EnumFormName.Parvandeh, section: "جستجوی پرونده")]
    public async Task<IActionResult> SearchParvandeh(SearchParvandehDto model)
    {
        var ruleSet = model.Code switch
        {
            1 => "shop",
            2 => "codeNosazi",
            _ => null
        };

        if (ruleSet != null)
        {
            var result = await _searchValidator.ValidateAsync(model, options => options.IncludeRuleSets(ruleSet));
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در اعتبارسنجی جستجو بر اساس {(model.Code == 1 ? "شماره پرونده" : "کد نوسازی")}!", EnumFormName.Parvandeh, EnumOperation.Get);
                TempData["ValidateMessage"] = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage));
                return PartialView();
            }
        }

        var parvandeh = model.Code switch
        {
            1 => _parvandehService.GetRow(model.Value),
            2 => _parvandehService.GetRowByCodeN(model.Value),
            _ => null
        };

        _historyLogService.PrepareForInsert($"مشاهده جستجوی پرونده {model.Value}", EnumFormName.Parvandeh, EnumOperation.Get);
        if (model.Code > 0 && parvandeh == null)
            ViewBag.ErrorMessage = $"پرونده مورد نظر یافت نشد!";

        return PartialView(parvandeh);
    }

}
