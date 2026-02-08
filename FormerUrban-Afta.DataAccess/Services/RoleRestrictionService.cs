using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.Login;
using System.Data;

namespace FormerUrban_Afta.DataAccess.Services;
public class RoleRestrictionService : IRoleRestrictionService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;
    private readonly RoleManager<CostumIdentityRole> _roleManager;
    private readonly UserManager<CostumIdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public RoleRestrictionService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, IAuditService auditService, RoleManager<CostumIdentityRole> roleManager, UserManager<CostumIdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _auditService = auditService;
        _roleManager = roleManager;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool CheckHash(RoleRestriction obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

    public async Task<AuthResponse> Add(RoleRestrictionDto roleRestrictionDto)
    {
        var response = new AuthResponse();
        try
        {
            if (await _context.RoleRestrictions.AnyAsync(x => x.RoleId == roleRestrictionDto.RoleId))
                return response.IsFailed("امکان ثبت رکورد تکراری وجود ندارد.");

            var data = _mapper.Map<RoleRestriction>(roleRestrictionDto);
            await _context.RoleRestrictions.AddAsync(data);
            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                _historyLogService.PrepareForInsert($"نقش {roleRestrictionDto.RoleText} با موفقیت مسدود شد.", EnumFormName.RoleRestriction, EnumOperation.Post);
                return response.IsSuccess();
            }

            _historyLogService.PrepareForInsert($"عملیات مسدود سازی نقش {roleRestrictionDto.RoleText}  انجام نشد.", EnumFormName.RoleRestriction, EnumOperation.Post);
            return response.IsFailed("عملیات با خطا مواجه شد.");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public async Task<AuthResponse> Delete(long id)
    {
        var response = new AuthResponse();
        try
        {
            var model = await _context.RoleRestrictions.FirstOrDefaultAsync(x => x.Identity == id);
            if (model == null)
                return response.IsFailed("رکورد مورد نظر یافت نشد");

            _context.RoleRestrictions.Remove(model);
            var res = await _context.SaveChangesAsync();
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(c => c.Id == model.RoleId);
            if (res > 0)
            {
                _historyLogService.PrepareForInsert($"نقش {role?.Description ?? ""} با موفقیت از مسدودی خارج شد.", EnumFormName.RoleRestriction, EnumOperation.Delete);
                return response.IsSuccess();
            }

            _historyLogService.PrepareForInsert($"عملیات رفع مسدودی نقش {role?.Description ?? ""} با خطا مواجه شده است.", EnumFormName.RoleRestriction, EnumOperation.Delete);
            return response.IsFailed("عملیات با خطا مواجه شد.");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public async Task<List<RoleRestrictionDto>> GetAllActiveRows()
    {
        var currentDate = DateTime.UtcNow.AddHours(3.5);
        var dataList = await _context.RoleRestrictions.AsNoTracking()
            .Where(r => (r.FromDate == null || r.FromDate <= currentDate) &&
                        (r.ToDate == null || r.ToDate >= currentDate))
            .ToListAsync();

        return dataList.Select(item =>
        {
            var dto = _mapper.Map<RoleRestrictionDto>(item);
            dto.IsValid = CheckHash(item);
            return dto;
        }).OrderByDescending(x => x.Identity).ToList();
    }

    public async Task<List<RoleRestrictionDto>> GetAll()
    {
        var dataList = await _context.RoleRestrictions.AsNoTracking().ToListAsync();
        return dataList.Select(item =>
        {
            var dto = _mapper.Map<RoleRestrictionDto>(item);
            dto.IsValid = CheckHash(item);
            var text = _context.Roles.AsNoTracking().FirstOrDefault(x => x.Id == dto.RoleId);
            dto.RoleText = text?.Description ?? "";
            if (!dto.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول نقش های مسدود شده  {dto.RoleText}", EnumFormName.RoleRestriction, EnumOperation.Validate);
            return dto;
        }).ToList();
    }

    public async Task<RoleRestrictionDto> GetById(long id)
    {
        var data = await _context.RoleRestrictions.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
        var mapped = _mapper.Map<RoleRestrictionDto>(data);
        mapped.IsValid = CheckHash(data);
        var text = _context.Roles.AsNoTracking().FirstOrDefault(x => x.Id == mapped.RoleId);
        mapped.RoleText = text?.Description ?? "";
        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول نقش های مسدود شده {mapped.RoleText}", EnumFormName.RoleRestriction, EnumOperation.Validate);
        return mapped;
    }

    public async Task<RoleRestrictionDto> GetByIdAsNoTracking(long id)
    {
        var data = await _context.RoleRestrictions.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
        var mapped = _mapper.Map<RoleRestrictionDto>(data);
        mapped.IsValid = CheckHash(data);
        var text = _context.Roles.AsNoTracking().FirstOrDefault(x => x.Id == mapped.RoleId);
        mapped.RoleText = text?.Description ?? "";
        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول نقش های مسدود شده {mapped.RoleText}", EnumFormName.RoleRestriction, EnumOperation.Validate);
        return mapped;
    }

    public async Task<RoleRestrictionDto> GetByRoleIdActive(string roleId)
    {
        var currentDate = DateTime.UtcNow.AddHours(3.5);
        var data = await _context.RoleRestrictions.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId &&
        (r.FromDate == null || r.FromDate <= currentDate) &&
                        (r.ToDate == null || r.ToDate >= currentDate));
        var mapped = _mapper.Map<RoleRestrictionDto>(data);
        return mapped;
    }

    public async Task<AuthResponse> Update(RoleRestrictionDto roleRestrictionDto)
    {
        var response = new AuthResponse();

        try
        {
            if (await _context.RoleRestrictions.AnyAsync(x => x.Identity != roleRestrictionDto.Identity && x.RoleId == roleRestrictionDto.RoleId))
                return response.IsFailed("امکان ثبت رکورد تکراری وجود ندارد.");

            //var data = _mapper.Map<RoleRestriction>(roleRestrictionDto);

            var oldModel = await GetByIdAsNoTracking(roleRestrictionDto.Identity);
            var oldModelRoleText = _roleManager.Roles.FirstOrDefault(x => x.Id == oldModel.RoleId)?.Description;
            if (oldModelRoleText != null)
                oldModel.RoleText = oldModelRoleText;

            var data = await _context.RoleRestrictions.FirstOrDefaultAsync(x => x.Identity == roleRestrictionDto.Identity);
            data.RoleId = roleRestrictionDto.RoleId;
            data.ToDate = roleRestrictionDto.ToDate.ToGregorianDateTime(false, 1300);
            data.FromDate = roleRestrictionDto.FromDate.ToGregorianDateTime(false, 1300);
            data.Description = roleRestrictionDto.Description;

            _context.RoleRestrictions.Update(data);
            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                _auditService.GetDifferences<RoleRestrictionDto>(oldModel, roleRestrictionDto, oldModel.Identity.ToString(), EnumFormName.RoleRestriction, EnumOperation.Update);
                _historyLogService.PrepareForInsert($"نقش {roleRestrictionDto.RoleText}  با موفقیت ویرایش شد.", EnumFormName.RoleRestriction, EnumOperation.Update);
                return response.IsSuccess();
            }

            _historyLogService.PrepareForInsert($"عملیات ویرایش مسدود سازی نقش {roleRestrictionDto.RoleText} انجام نشد.", EnumFormName.RoleRestriction, EnumOperation.Update);
            return response.IsFailed("عملیات با خطا مواجه شد.");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public async Task<bool> IsUserRestricted(string userId)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userObject = await _userManager.GetUserAsync(user);

        if (userObject?.UserName == null)
            userObject = await _userManager.FindByIdAsync(userId);

        if (userObject?.UserName is "developer")
            return false;

        // Get restricted role IDs as a HashSet for O(1) lookup
        var restrictedRoleIds = (await GetAllActiveRows())
        .Select(r => r.RoleId)
        .ToHashSet();

        if (!restrictedRoleIds.Any())
            return false;

        // Check if user has any restricted roles using database-level filtering
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId && restrictedRoleIds.Contains(ur.RoleId))
            .AnyAsync();
    }
}
