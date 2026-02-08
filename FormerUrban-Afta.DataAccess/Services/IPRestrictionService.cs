
using DNTPersianUtils.Core;

namespace FormerUrban_Afta.DataAccess.Services;

public class IPRestrictionService : IAllowedIPRange, IBlockedIPRange
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;

    public IPRestrictionService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _auditService = auditService;
    }

    public bool AllowedCheckHash(AllowedIPRange obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);
    public bool BlockedCheckHash(BlockedIPRange obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

    #region AllowedIPRange
    public async Task<List<AllowedIPRangeDto>?> GetAllAllowedIPRangeAsync()
    {
        var data = await _context.AllowedIPRange.ToListAsync();
        _historyLogService.PrepareForInsert($"دریافت لیست آیپی های مجاز", EnumFormName.AllowedIPRange, EnumOperation.Get);

        if (!data.Any())
            return null;

        var results = data.Select(item =>
        {
            var dto = _mapper.Map<AllowedIPRangeDto>(item);
            dto.IsValid = AllowedCheckHash(item);
            if (!dto.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول آیپی های مجاز با آیپی {dto.IPRange}", EnumFormName.AllowedIPRange, EnumOperation.Validate);
            return dto;
        }).OrderByDescending(x => x.Identity).ToList();

        return results;
    }

    public async Task<AllowedIPRangeDto?> GetAllowedIPRangeAsync(long id)
    {
        var model = await _context.AllowedIPRange.Where(c => c.Identity == id).FirstOrDefaultAsync();
        if (model == null)
            return null;
        _historyLogService.PrepareForInsert($"دریافت آیپی مجاز با آیپی {model.IPRange}", EnumFormName.AllowedIPRange, EnumOperation.Get);
        var result = _mapper.Map<AllowedIPRangeDto>(model);
        result.IsValid = AllowedCheckHash(model);
        return result;
    }

    public async Task<AllowedIPRangeDto> GetByIdAsNoTracking(long id)
    {
        var model = await _context.AllowedIPRange.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
        var result = _mapper.Map<AllowedIPRangeDto>(model);
        result.IsValid = AllowedCheckHash(model);
        return result;
    }

    public async Task<bool> UpdateAllowedIPRangeAsync(AllowedIPRangeDto obj)
    {
        try
        {
            var model = await _context.AllowedIPRange.Where(c => c.Identity == obj.Identity).FirstOrDefaultAsync();
            if (model == null)
            {
                _historyLogService.PrepareForInsert($"خطا در ویرایش وایت لیست. آیپی  {obj.IPRange} یافت نشد", EnumFormName.AllowedIPRange, EnumOperation.Update);
                return false;
            }

            //var mapped = _mapper.Map<AllowedIPRange>(obj);
            model.IPRange = obj.IPRange;
            model.FromDate = obj.FromDate.ToGregorianDateTime(false, 1300);
            model.ToDate = obj.ToDate.ToGregorianDateTime(false, 1300);
            model.Description = obj.Description;

            var oldModel = await GetByIdAsNoTracking(model.Identity);
            _context.AllowedIPRange.Update(model);

            var res = await _context.SaveChangesAsync();

            _auditService.GetDifferences<AllowedIPRangeDto>(oldModel, obj, oldModel.Identity.ToString(), EnumFormName.AllowedIPRange, EnumOperation.Update);
            _historyLogService.PrepareForInsert($"ویرایش وایت لیست. آیپی {obj.IPRange}", EnumFormName.AllowedIPRange, EnumOperation.Update);

            return res > 0;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش وایت لیست. ذخیره تغییرات در دیتابیس با خطا مواجه شده است", EnumFormName.AllowedIPRange, EnumOperation.Update);

            throw;
        }
    }

    public async Task<bool> DeleteAllowedIPRangeAsync(long id)
    {
        try
        {
            var model = await _context.AllowedIPRange.Where(c => c.Identity == id).FirstOrDefaultAsync();
            if (model == null)
            {
                _historyLogService.PrepareForInsert($"خطا در حذف وایت لیست. آیپی با آیدی {id} یافت نشد", EnumFormName.AllowedIPRange, EnumOperation.Delete);
                return false;
            }
            _context.AllowedIPRange.Remove(model);

            var res = await _context.SaveChangesAsync();
            _historyLogService.PrepareForInsert($"حذف وایت لیست. آیپی {model.IPRange}", EnumFormName.AllowedIPRange, EnumOperation.Delete);
            return res > 0;

        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در حذف وایت لیست. ذخیره تغییرات در دیتابیس با خطا مواجه شده است", EnumFormName.AllowedIPRange, EnumOperation.Delete);
            throw;
        }
    }

    public async Task<bool> AddAllowedIPRangeAsync(AllowedIPRangeDto obj)
    {
        var model = _mapper.Map<AllowedIPRange>(obj);
        await _context.AllowedIPRange.AddAsync(model);
        var res = await _context.SaveChangesAsync();
        _historyLogService.PrepareForInsert($"افزودن آیپی {obj.IPRange} به وایت لیست", EnumFormName.AllowedIPRange, EnumOperation.Post);
        return res > 0;
    }
    #endregion

    #region BlockedIPRange

    public async Task<List<BlockedIPRangeDto>?> GetAllBlockedIPRangeAsync()
    {
        _historyLogService.PrepareForInsert($"مشاهده اطلاعات آیپی های مسدود شده", EnumFormName.BlockedIPRange, EnumOperation.Get);

        var modelList = await _context.BlockedIPRange.ToListAsync();
        if (!modelList.Any())
            return null;

        var results = modelList.Select(item =>
        {
            var dto = _mapper.Map<BlockedIPRangeDto>(item);
            dto.IsValid = CipherService.IsEqual(item.ToString(), item.Hashed);
            if (!dto.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول آیپی های مسدود شده با آیپی {dto.IPRange}", EnumFormName.BlockedIPRange, EnumOperation.Validate);
            return dto;
        }).OrderByDescending(x => x.Identity).ToList();

        return results;
    }

    public async Task<BlockedIPRangeDto?> GetBlockedIPRangeAsync(long identity)
    {
        var model = await _context.BlockedIPRange.Where(c => c.Identity == identity).FirstOrDefaultAsync();

        if (model == null)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت آیپی غیر مجاز : یافت نشد", EnumFormName.BlockedIPRange, EnumOperation.Get);
            return null;
        }
        _historyLogService.PrepareForInsert($"دریافت آیپی غیر مجازی {model.IPRange}", EnumFormName.BlockedIPRange, EnumOperation.Get);

        return _mapper.Map<BlockedIPRangeDto>(model);
    }

    public async Task<BlockedIPRangeDto> GetBlockedIPByIdAsNoTracking(long id)
    {
        var model = await _context.BlockedIPRange.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
        var result = _mapper.Map<BlockedIPRangeDto>(model);
        result.IsValid = BlockedCheckHash(model);
        return result;
    }

    public async Task<bool> UpdateBlockedIPRangeAsync(BlockedIPRangeDto obj)
    {
        try
        {
            var model = await _context.BlockedIPRange
                .Where(c => c.Identity == obj.Identity)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                _historyLogService.PrepareForInsert($"خطا در ویرایش بلاک لیست. آیپی {obj.IPRange} یافت نشد", EnumFormName.BlockedIPRange, EnumOperation.Update);
                return false;
            }

            //var mapped = _mapper.Map<BlockedIPRange>(obj);
            model.IPRange = obj.IPRange;
            model.FromDate = obj.FromDate.ToGregorianDateTime(false, 1300);
            model.ToDate = obj.ToDate.ToGregorianDateTime(false, 1300);
            model.Description = obj.Description;

            var oldModel = await GetBlockedIPByIdAsNoTracking(model.Identity);

            _context.BlockedIPRange.Update(model);
            var res = await _context.SaveChangesAsync();
            _auditService.GetDifferences<BlockedIPRangeDto>(oldModel, obj, oldModel.Identity.ToString(), EnumFormName.BlockedIPRange, EnumOperation.Update);
            _historyLogService.PrepareForInsert($"ویرایش بلاک لیست. آیپی {obj.IPRange}", EnumFormName.BlockedIPRange, EnumOperation.Update);

            return res > 0;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش بلاک لیست. ذخیره تغییرات در دیتابیس با خطا مواجه شده است", EnumFormName.BlockedIPRange, EnumOperation.Update);

            throw;
        }
    }

    public async Task<bool> DeleteBlockedIPRangeAsync(long identity)
    {
        try
        {
            var model = await _context.BlockedIPRange.Where(c => c.Identity == identity).FirstOrDefaultAsync();
            if (model == null)
            {
                _historyLogService.PrepareForInsert($"خطا در حذف از بلاک لیست. آیپی یافت نشد", EnumFormName.BlockedIPRange, EnumOperation.Delete);
                return false;
            }
            _context.BlockedIPRange.Remove(model);
            var res = await _context.SaveChangesAsync();
            _historyLogService.PrepareForInsert($"حذف از بلاک لیست. آیپی {model.IPRange}", EnumFormName.BlockedIPRange, EnumOperation.Delete);

            return res > 0;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در حذف از بلک لیست. ذخیره تغییرات در دیتابیس با خطا مواجه شده است", EnumFormName.BlockedIPRange, EnumOperation.Delete);
            throw;
        }
    }

    public async Task<bool> AddBlockedIPRangeAsync(BlockedIPRangeDto obj)
    {
        var model = _mapper.Map<BlockedIPRange>(obj);
        await _context.BlockedIPRange.AddAsync(model);
        var res = await _context.SaveChangesAsync();
        _historyLogService.PrepareForInsert($"افزودن آیپی {obj.IPRange} به بلاک لیست", EnumFormName.BlockedIPRange, EnumOperation.Post);
        return res > 0;
    }
    #endregion

}
