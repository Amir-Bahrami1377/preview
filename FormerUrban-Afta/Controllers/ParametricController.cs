namespace FormerUrban_Afta.Controllers
{
    public class ParametricController : AmardBaseController
    {
        private readonly ISabethaService _sabethaService;

        public ParametricController(ISabethaService sabethaService)
        {
            _sabethaService = sabethaService;
        }


        public IActionResult Index(string enumName, string CodeId, string TitleId)
        {
            ViewBag.CodeId = CodeId;
            ViewBag.TitleId = TitleId;
            var result = _sabethaService.GetRows(enumName);
            return PartialView(result);
        }
    }
}
