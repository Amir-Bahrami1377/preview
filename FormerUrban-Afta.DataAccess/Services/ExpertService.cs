using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services;
public class ExpertService : IExpertService
{
    private readonly IEncryptionService _encryptionService;
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly IAuditService _auditService;


    public ExpertService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, IEncryptionService encryptionService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _encryptionService = encryptionService;
        _auditService = auditService;
    }

    #region shit

    private async Task<ExpertDto> DecryptInfo2(ExpertDto entity)
    {
        if (entity?.Name != null)
            entity.Name = await _encryptionService.DecryptAsync(entity.Name);

        if (entity?.Family != null)
            entity.Family = await _encryptionService.DecryptAsync(entity.Family);

        return entity;
    }
    private async Task<Expert> EncryptInfo2(Expert entity)
    {
        if (entity?.Name != null)
            entity.Name = await _encryptionService.EncryptAsync(entity.Name);

        if (entity?.Family != null)
            entity.Family = await _encryptionService.EncryptAsync(entity.Family);
        return entity;
    }

    public static bool CheckHash(Expert obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #endregion

    public async Task<List<ExpertDto>> GetByRequestNumberAsync(int id)
    {
        try
        {
            var data = await _context.Expert.Where(c => c.RequestNumber == id).ToListAsync();

            var tasks = data.Select(async item =>
            {
                var dto = _mapper.Map<ExpertDto>(item);
                dto.IsValid = CheckHash(item);
                return await DecryptInfo2(dto);
            });

            var results = (await Task.WhenAll(tasks)).ToList();

            var invalidRecords = results.Where(dto => !dto.IsValid).Select(dto => $"رد صحت سنجی داده مامور بازدید شماره درخواست {id}").ToList();

            if (invalidRecords.Any())
            {
                foreach (var message in invalidRecords)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.Expert, EnumOperation.Validate);
                }
            }

            _historyLogService.PrepareForInsert($"مشاهده اطلاعات کارشناسان بازدید درخواست {id}", EnumFormName.Expert, EnumOperation.Get, shod: id);

            return results;
        }
        catch (Exception ex)
        {
            // Log exception (use your preferred logging framework)
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کارشناسان بازدید درخواست {id}: {ex.Message}", EnumFormName.Expert, EnumOperation.Get, shod: id);
            throw; // Rethrow or handle as needed
        }
    }


    public async Task<ExpertDto> GetByIdAsNoTracking(long id)
    {
        try
        {
            var data = await _context.Expert.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
            if (data == null)
            {
                _historyLogService.PrepareForInsert("اطلاعات کارشناس بازدید برای نمایش یافت نشد", EnumFormName.Expert, EnumOperation.Get);
                return new ExpertDto();
            }
            var mapped = _mapper.Map<ExpertDto>(data);
            mapped.IsValid = CheckHash(data);

            return await DecryptInfo2(mapped);
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert("خطا در دریافت اطلاعات کارشناس بازدید : {e.Message}", EnumFormName.Expert, EnumOperation.Get);
            throw;
        }
    }

    public async Task<ExpertDto> GetAsync(long id)
    {
        try
        {
            var data = await _context.Expert.FirstOrDefaultAsync(c => c.Identity == id);
            if (data == null)
            {
                _historyLogService.PrepareForInsert("اطلاعات کارشناس بازدید برای نمایش یافت نشد", EnumFormName.Expert, EnumOperation.Get);
                return new ExpertDto();
            }
            var mapped = _mapper.Map<ExpertDto>(data);
            mapped.IsValid = CheckHash(data);
            if (!mapped.IsValid)
                _historyLogService.PrepareForInsert("رد صحت سنجی داده جدول کارشناس بازدید", EnumFormName.Expert, EnumOperation.Validate);

            _historyLogService.PrepareForInsert("دریافت اطلاعات کارشناس بازدید", EnumFormName.Expert, EnumOperation.Get, shod: data.RequestNumber);
            return await DecryptInfo2(mapped);
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات کارشناس بازدید : {e.Message}", EnumFormName.Expert, EnumOperation.Get);
            throw;
        }
    }

    public async Task<List<ExpertDto>> GetAllAsync()
    {
        var data = await _context.Expert.ToListAsync();

        var tasks = data.Select(async item =>
        {
            var dto = _mapper.Map<ExpertDto>(item);
            dto.IsValid = CheckHash(item);
            return await DecryptInfo2(dto);
        });

        return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task<ExpertDto> AddAsync(ExpertDto entity)
    {
        try
        {
            var model = _mapper.Map<Expert>(entity);
            model = await EncryptInfo2(model);
            await _context.Expert.AddAsync(model);
            await _context.SaveChangesAsync();

            _historyLogService.PrepareForInsert($"ثبت اطلاعات کارشناس بازدید درخواست {entity.RequestNumber}", EnumFormName.Expert, EnumOperation.Post, shod: entity.RequestNumber);

            var data = await GetAsync(model.RequestNumber);
            return data;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات کارشناس بازدید درخواست {entity.RequestNumber}", EnumFormName.Expert, EnumOperation.Post, shod: entity.RequestNumber);

            throw;
        }
    }

    public async Task AddListAsync(List<ExpertDto> entity)
    {
        var mapData = _mapper.Map<List<Expert>>(entity);
        var encryptionTasks = mapData.Select(item => EncryptInfo2(item)).ToList();
        var encryptedData = await Task.WhenAll(encryptionTasks);

        await _context.Expert.AddRangeAsync(encryptedData);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(ExpertDto entity)
    {
        try
        {
            var model = _mapper.Map<Expert>(entity);
            model = await EncryptInfo2(model);
            var oldModel = await GetByIdAsNoTracking(model.Identity);

            _context.Expert.Update(model);
            await _context.SaveChangesAsync();
            _auditService.GetDifferences<ExpertDto>(oldModel, entity, oldModel.Identity.ToString(), EnumFormName.Expert, EnumOperation.Update);

            _historyLogService.PrepareForInsert($"بروزرسانی اطلاعات کارشناس بازدید درخواست {entity.RequestNumber}", EnumFormName.Expert, EnumOperation.Update, shod: entity.RequestNumber);

            return true;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات کارشناس بازدید درخواست {entity.RequestNumber} : {e.Message}", EnumFormName.Expert, EnumOperation.Update, shod: entity.RequestNumber);

            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var entity = await _context.Expert.FirstOrDefaultAsync(c => c.Identity == id);
            if (entity == null)
            {
                _historyLogService.PrepareForInsert($"اطلاعات کارشناس بازدید برای حذف یافت نشد", EnumFormName.Expert, EnumOperation.Delete, shod: 0);
                return false;
            }

            _context.Expert.Remove(entity);
            await _context.SaveChangesAsync();

            _historyLogService.PrepareForInsert($" حذف اطلاعات کارشناس بازدید با شماره درخواست {entity.RequestNumber}", EnumFormName.Expert, EnumOperation.Delete, shod: entity.RequestNumber);

            return true;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در حذف اطلاعات کارشناس بازدید : {e.Message}", EnumFormName.Expert, EnumOperation.Delete);

            return false;
        }
    }
}