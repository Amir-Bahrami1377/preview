using DNTPersianUtils.Core;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services;

public class AuditFilterService : IAuditFilterService
{

    private readonly FromUrbanDbContext _context;

    public AuditFilterService(FromUrbanDbContext context)
    {
        _context = context;
    }


    public async Task<List<AuditFilterDto>> GetAllAsync()
    {
        var model = await _context.AuditFilters.ToListAsync();
        var result = model.Select(static x => new AuditFilterDto
        {
            Identity = x.Identity,
            TableName = x.FormId.GetDisplayEnumName(),
            CreationDate = DateTime.SpecifyKind(x.CreateDateTime, DateTimeKind.Local)
                    .ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
            IsValid = CipherService.IsEqual(x.ToString(), x.Hashed),
        }).ToList();
        return result;
    }

    public bool ExistsById(EnumFormName formName) => _context.AuditFilters.Any(x => x.FormId == formName);

    public async Task<ServiceResult<string>> CreateAsync(AuditFilterDto entity)
    {
        var result = new ServiceResult<string>();
        try
        {
            var formDisplayName = GetEnumFormNameDisplayName(entity.FormName);
            if (_context.AuditFilters.Any(x => x.FormId == entity.FormName))
                return result.IsFailed("امکان ثبت رکورد تکراری وجود ندارد.", data: formDisplayName);

            var model = new AuditFilter
            {
                FormId = entity.FormName
            };

            await _context.AuditFilters.AddAsync(model);
            await _context.SaveChangesAsync();
            return result.IsSuccess(message: "ثبت رکورد با موفقیت انجام شد", data: formDisplayName);
        }
        catch (Exception ex)
        {
            return result.IsFailed(ex.Message, data: "");
        }
    }

    public async Task<ServiceResult<string>> DeleteById(long id)
    {
        var result = new ServiceResult<string>();
        try
        {
            var model = await _context.AuditFilters.FirstOrDefaultAsync(x => x.Identity == id);
            if (model == null)
                return result.IsFailed("رکورد موردنظر یافت نشد", data: "");
            var formId = model.FormId;
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return result.IsSuccess(message: "حذف فیلتر تغییرات", data: GetEnumFormNameDisplayName(formId));
        }
        catch (Exception e)
        {
            return result.IsFailed(e.Message, "");
        }
    }

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

    private string GetEnumFormNameDisplayName(EnumFormName obj)
    {
        return obj.GetType().GetMember(obj.ToString())[0].GetCustomAttribute<DisplayAttribute>()?.Name ??
               obj.ToString();
    }
}

