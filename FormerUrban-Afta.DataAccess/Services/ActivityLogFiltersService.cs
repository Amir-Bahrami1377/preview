using FormerUrban_Afta.DataAccess.DTOs.Login;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services;

public class ActivityLogFiltersService : IActivityLogFiltersService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;

    public ActivityLogFiltersService(FromUrbanDbContext context, IMapper mapper, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
    }

    private bool CheckHash(ActivityLogFilters obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

    public async Task<AuthResponse> Add(ActivityLogFiltersDto activityLogFiltersDto)
    {
        var response = new AuthResponse();
        try
        {
            if (activityLogFiltersDto.FormName == EnumFormName.None)
                return response.IsFailed("لطفا یک فرم را انتخاب کنید!");

            if (await _context.ActivityLogFilters.AnyAsync(x => x.FormName == activityLogFiltersDto.FormName))
                return response.IsFailed("امکان ثبت رکورد تکراری وجود ندارد");

            var mapped = _mapper.Map<ActivityLogFilters>(activityLogFiltersDto);
            await _context.ActivityLogFilters.AddAsync(mapped);
            var res = _context.SaveChangesAsync();

            return await res > 0 ? response.IsSuccess() : response.IsFailed("عملیات با خطا مواجه شده است.");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public async Task<AuthResponse> Delete(long Identity)
    {
        var response = new AuthResponse();
        try
        {
            var model = await _context.ActivityLogFilters.FirstOrDefaultAsync(x => x.Identity == Identity);
            if (model == null)
                return response.IsFailed("رکورد موردنظر یافت نشد");

            _context.ActivityLogFilters.Remove(model);
            var res = await _context.SaveChangesAsync();
            return res > 0 ? response.IsSuccess() : response.IsFailed("عملیات با خطا مواجه شده است.");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public List<ActivityLogFiltersDto> GetAll()
    {
        var data = _context.ActivityLogFilters.AsNoTracking().ToList();

        return data.Select(item =>
        {
            var dto = _mapper.Map<ActivityLogFiltersDto>(item);
            dto.IsValid = CheckHash(item);
            dto.TableName = GetEnumFormNameDisplayName(dto.FormName);
            return dto;
        }).OrderByDescending(x => x.Identity).ToList();
    }

    public ActivityLogFiltersDto GetById(long id)
    {
        var data = _context.ActivityLogFilters.FirstOrDefault(x => x.Identity == id);
        if (data == null)
            return new ActivityLogFiltersDto();

        var mapped = _mapper.Map<ActivityLogFiltersDto>(data);
        mapped.Tables = new SelectList(GetEnumFormNameInfo(), "Name", "DisplayName");
        mapped.IsValid = CheckHash(data);
        return mapped;
    }

    public ActivityLogFiltersDto GetByIdAsNoTracking(long id)
    {
        var data = _context.ActivityLogFilters.AsNoTracking().FirstOrDefault(x => x.Identity == id);
        var mapped = _mapper.Map<ActivityLogFiltersDto>(data);
        mapped.IsValid = CheckHash(data);
        return mapped;
    }

    public ActivityLogFilters GetByFormName(EnumFormName formName)
    {
        var data = _context.ActivityLogFilters.AsNoTracking().FirstOrDefault(c => c.FormName == formName);
        return data;
    }

    public async Task<AuthResponse> Update(ActivityLogFiltersDto activityLogFiltersDto)
    {
        var response = new AuthResponse();
        try
        {
            if (activityLogFiltersDto.FormName == EnumFormName.None)
                return response.IsFailed("لطفا یک فرم را انتخاب کنید!");

            if (await _context.ActivityLogFilters.AnyAsync(x => x.FormName == activityLogFiltersDto.FormName && x.Identity != activityLogFiltersDto.Identity))
                return response.IsFailed("امکان ثبت رکورد تکراری وجود ندارد");

            var data = _mapper.Map<ActivityLogFilters>(activityLogFiltersDto);
            var oldModel = GetByIdAsNoTracking(activityLogFiltersDto.Identity);
            oldModel.TableName = GetEnumFormNameDisplayName(oldModel.FormName);

            _context.ActivityLogFilters.Update(data);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
                _auditService.GetDifferences<ActivityLogFiltersDto>(oldModel, activityLogFiltersDto, oldModel.Identity.ToString(), EnumFormName.ActivityLogFilters, EnumOperation.Update);

            return res > 0 ? response.IsSuccess() : response.IsFailed("عملیات با خطا مواجه شد");
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }
    public int SaveChanges() => _context.SaveChanges();

    public List<EnumFormNameInfo> GetEnumFormNameInfo()
    {
        return Enum.GetValues(typeof(EnumFormName))
            .Cast<EnumFormName>()
            .Select(e => new EnumFormNameInfo
            {
                Name = e.ToString(),
                Index = (int)e,
                DisplayName = e.GetType()
                    .GetMember(e.ToString())[0]
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();
    }

    private string GetEnumFormNameDisplayName(EnumFormName value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        if (memberInfo.Length > 0)
        {
            var displayAttr = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
                return displayAttr.Name;
        }

        return value.ToString();
    }
}

