using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.Services;
public class EstelamService : IEstelamService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEncryptionService _encryptionService;
    private readonly IAuditService _auditService;


    public EstelamService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, IEncryptionService encryptionService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _encryptionService = encryptionService;
        _auditService = auditService;
    }

    #region shit

    private async Task<EstelamDto> DecryptInfo2(EstelamDto entity)
    {
        if (entity?.Kharidar != null)
            entity.Kharidar = await _encryptionService.DecryptAsync(entity.Kharidar);

        if (entity?.NoeMalekiat != null)
            entity.NoeMalekiat = await _encryptionService.DecryptAsync(entity.NoeMalekiat);

        return entity;
    }
    private async Task<Estelam> EncryptInfo2(Estelam entity)
    {
        if (entity?.Kharidar != null)
            entity.Kharidar = await _encryptionService.EncryptAsync(entity.Kharidar);

        if (entity?.NoeMalekiat != null)
            entity.NoeMalekiat = await _encryptionService.EncryptAsync(entity.NoeMalekiat);
        return entity;
    }

    public static bool CheckHash(Estelam obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #endregion

    #region Get

    public async Task<EstelamDto> GetByIdAsNoTracking(long id)
    {
        try
        {
            var data = await _context.Estelam.AsNoTracking().FirstOrDefaultAsync(c => c.Identity == id);
            if (data == null)
            {
                _historyLogService.PrepareForInsert("خطا در دریافت اطلاعات استعلام اطلاعات یافت نشد", EnumFormName.Estelam, EnumOperation.Get);
                return null;
            }

            var mapped = _mapper.Map<EstelamDto>(data);
            mapped.IsValid = CheckHash(data);
            mapped = await DecryptInfo2(mapped);

            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات استعلام : {e.Message}", EnumFormName.Estelam, EnumOperation.Get);
            throw;
        }
    }

    public async Task<EstelamDto> GetByRequestNumberAsync(int id)
    {
        try
        {
            var data = await _context.Estelam.Where(c => c.Sh_Darkhast == id).FirstOrDefaultAsync();
            if (data == null)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات استعلام با شماره درخواست {id} اطلاعات یافت نشد", EnumFormName.Estelam, EnumOperation.Get, shod: id);
                return new EstelamDto();
            }

            var mapped = _mapper.Map<EstelamDto>(data);
            mapped.IsValid = CheckHash(data);
            mapped = await DecryptInfo2(mapped);
            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی در دریافت اطلاعات استعلام با شماره درخواست {id}", EnumFormName.Estelam, EnumOperation.Validate, shod: id);
            }

            _historyLogService.PrepareForInsert($"دریافت اطلاعات استعلام با شماره درخواست {id}", EnumFormName.Estelam, EnumOperation.Get, shod: id);

            return mapped;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات استعلام با شماره درخواست {id} : {e.Message}", EnumFormName.Estelam, EnumOperation.Get, shod: id);
            throw;
        }
    }

    #endregion

    public async Task<bool> UpdateAsync(EstelamDto entity)
    {
        try
        {
            var model = _mapper.Map<Estelam>(entity);
            model = await EncryptInfo2(model);

            if (model.Identity == 0)
                _context.Estelam.Add(model);
            else
            {
                var oldModel = await GetByIdAsNoTracking(model.Identity);
                _context.Estelam.Update(model);
                _auditService.GetDifferences<EstelamDto>(oldModel, entity, oldModel.Identity.ToString(), EnumFormName.Estelam, EnumOperation.Update);

            }
            var res = await _context.SaveChangesAsync();
            entity.Identity = model.Identity;
            _historyLogService.PrepareForInsert($"ویرایش اطلاعات استعلام درخواست {entity.Sh_Darkhast}", EnumFormName.Estelam, EnumOperation.Update, shop: entity.shop, shod: entity.Sh_Darkhast);

            return res > 0;
        }
        catch (Exception e)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش اطلاعات استعلام با شماره درخواست {entity.Sh_Darkhast} : {e.Message}",
               EnumFormName.Estelam, EnumOperation.Update, shop: entity.shop, shod: entity.Sh_Darkhast);
            throw;
        }
    }
}