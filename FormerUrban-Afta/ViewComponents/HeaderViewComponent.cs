namespace FormerUrban_Afta.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public HeaderViewComponent(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _authService.GetCurentUserAsync();
            var model = _mapper.Map<UpdateCostumIdentityUserDto>(user);
            //var modelRole = await _authService.GetRoleByUserIdAsync(model.Id);
            //var roles = new List<string>();
            //foreach (var role in modelRole)
            //{
            //    var roleDetails = await _authService.GetRoleByNameAsync(role);
            //    roles.Add(roleDetails.Description);
            //}
            //model.Role = roles; 
            return View(model);
        }
    }
}
