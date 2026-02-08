using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Marahel.Controllers;
[Area("Marahel")]
public class BayganiController : AmardBaseController
{
    private readonly ILogger<ParvanehController> _logger;
    private readonly IErjaService _erjaService;

    public BayganiController(ILogger<ParvanehController> logger, IErjaService erjaService)
    {
        _logger = logger;
        _erjaService = erjaService;
    }

    [CheckUserAccess("Menu_Baygani", type: EnumOperation.Get, table: EnumFormName.Baygani, section: "بایگانی اطلاعات پرونده")]
    public async Task<IActionResult> Index()
    {
        var data = await _erjaService.GetBaygani();
        return View(data);
    }
}
