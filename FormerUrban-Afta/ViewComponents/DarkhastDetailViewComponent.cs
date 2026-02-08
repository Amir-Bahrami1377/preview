namespace FormerUrban_Afta.ViewComponents
{
    public class DarkhastDetailViewComponent : ViewComponent
    {
        private readonly IDarkhastService _darkhastService;

        public DarkhastDetailViewComponent(IDarkhastService darkhastService)
        {
            _darkhastService = darkhastService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int shod)
        {
            var model = await _darkhastService.GetDataByShod(shod);
            return View(model);
        }
    }
}
