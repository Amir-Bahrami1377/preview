using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Model;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Marahel.Controllers;
[Area("Marahel")]
public class ExpertController : AmardBaseController
{
    private readonly IValidator<ExpertDto> _expertValidator;
    private readonly ILogger<ExpertController> _logger;
    private readonly IExpertService _expertService;
    private readonly IHistoryLogService _historyLogService;

    public ExpertController(ILogger<ExpertController> logger, IValidator<ExpertDto> expertValidator, IExpertService expertService, IHistoryLogService historyLogService)
    {
        _logger = logger;
        _expertValidator = expertValidator;
        _expertService = expertService;
        _historyLogService = historyLogService;
    }

    
    [CheckUserAccess("Visit_Expert", type: EnumOperation.Get, table: EnumFormName.Expert, section: "مشاهده مامور بازدید")]
    public async Task<IActionResult> Index(int shop, int requestNumber, string? message)
    {
        var model = await _expertService.GetByRequestNumberAsync(requestNumber);
        ViewBag.shop = shop;
        ViewBag.requestNumber = requestNumber;
        ViewBag.message = message;
        return PartialView(model: model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Visit_DeleteExpert", type: EnumOperation.Delete, table: EnumFormName.Expert, section: "حذف مامور بازدید")]
    public async Task<IActionResult> Delete(long id, int requestNumber, int shop)
    {
        var model = await _expertService.GetAsync(id);
        var res = await _expertService.DeleteAsync(id);

        _historyLogService.PrepareForInsert(
            res
                ? $"حذف مامور بازدید {model.Name} {model.Family} برای شماره درخواست {requestNumber} با موفقیت انجام شد."
                : $"خطا در حذف مامور بازدید {model.Name} {model.Family} برای شماره درخواست {requestNumber}",
            EnumFormName.Expert, EnumOperation.Delete);
        
        return new JsonResult(new { success = res, message = res ? $"مامور {model.Name + " " + model.Family} برای درخواست {model.RequestNumber} با موفقیت حذف شد!" : "عملیات با خطا مواجه شده است!" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Visit_CreateExpert", type: EnumOperation.Get, table: EnumFormName.Expert, section: "ایجاد مامور بازدید")]
    public async Task<IActionResult> Create(int requestNumber)
    {
        var data = new ExpertDto
        {
            RequestNumber = requestNumber,
        };
        _historyLogService.PrepareForInsert($"مشاهده اطلاعات ایجاد مامور بازدید", EnumFormName.Expert, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Visit_CreateExpert", type: EnumOperation.Post, table: EnumFormName.Expert, section: "ایجاد مامور بازدید")]
    public async Task<IActionResult> CreateSubmit(ExpertDto model)
    {
        try
        {
            var result = await _expertValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در ثیت مامور بازدید درخواست {model.RequestNumber} ", EnumFormName.Expert, EnumOperation.Post);
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = errorMessages });

            }

            await _expertService.AddAsync(model);

            _historyLogService.PrepareForInsert($"ثیت مامور بازدید با شماره درخواست {model.RequestNumber} با موفقیت انجام شد", EnumFormName.Expert, EnumOperation.Post);
            return new JsonResult(new { success = true, message = $"مامور {model.Name + " " + model.Family} برای درخواست {model.RequestNumber} با موفقیت ثبت شد." });
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"ثیت مامور بازدید با شماره درخواست {model.RequestNumber} با خطا مواجه شده است.", EnumFormName.Expert, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "خطایی در ایجاد مامور بازدید رخ داده است!" });
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Visit_EditExpert", type: EnumOperation.Get, table: EnumFormName.Expert, section: "ویرایش مامور بازدید")]
    public async Task<IActionResult> Edit(long identity, int requestNumber)
    {
        var data = await _expertService.GetAsync(identity);
        data.RequestNumber = requestNumber;
        _historyLogService.PrepareForInsert($"مشاهده ویرایش اطلاعات مامور بازدید {data.Name} {data.Family}", EnumFormName.Expert, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Visit_EditExpert", type: EnumOperation.Update, table: EnumFormName.Expert, section: "ویرایش مامور بازدید")]
    public async Task<IActionResult> EditSubmit(ExpertDto model)
    {
        try
        {
            var result = await _expertValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطای اعتبار سنجی در ویرایش مامور بازدید {model.Name} {model.Family} درخواست {model.RequestNumber}", EnumFormName.Expert, EnumOperation.Update);
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = errorMessages });

            }
            await _expertService.UpdateAsync(model);
            _historyLogService.PrepareForInsert($"ویرایش مامور بازدید {model.Name} {model.Family} درخواست {model.RequestNumber} با موفقیت انجام شد.", EnumFormName.Expert, EnumOperation.Update);
            return new JsonResult(new { success = true, message = $"مامور شماره {model.Identity} برای درخواست {model.RequestNumber} با موفقیت ویرایش شد." });
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت ویرایش مامور بازدید {model.Name} {model.Family} درخواست {model.RequestNumber}.", EnumFormName.Expert, EnumOperation.Update);
            return new JsonResult(new { success = false, message = "خطایی در ویرایش مامور بازدید رخ داده است!" });
        }
        
    }
}