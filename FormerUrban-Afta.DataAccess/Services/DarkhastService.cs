namespace FormerUrban_Afta.DataAccess.Services;

public class DarkhastService : IDarkhastService
{
    private static byte[] _shit = Encoding.UTF8.GetBytes("pT51reLhSK58kistBeEx35tly75Jh69R");
    private readonly FromUrbanDbContext _context;
    private readonly IHistoryLogService _historyLogService;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;

    public DarkhastService(FromUrbanDbContext context, IHistoryLogService historyLogService, IMapper mapper, IEncryptionService encryptionService)
    {
        _context = context;
        _historyLogService = historyLogService;
        _mapper = mapper;
        _encryptionService = encryptionService;
    }

    #region Shit

    private async Task<DarkhastDTO> DecryptInfo2(DarkhastDTO obj)
    {
        if (!string.IsNullOrWhiteSpace(obj.address))
            obj.address = await _encryptionService.DecryptAsync(obj.address);

        if (!string.IsNullOrWhiteSpace(obj.CodeMeli))
            obj.CodeMeli = await _encryptionService.DecryptAsync(obj.CodeMeli);

        if (!string.IsNullOrWhiteSpace(obj.tel))
            obj.tel = await _encryptionService.DecryptAsync(obj.tel);

        if (!string.IsNullOrWhiteSpace(obj.mob))
            obj.mob = await _encryptionService.DecryptAsync(obj.mob);

        if (!string.IsNullOrWhiteSpace(obj.codeposti))
            obj.codeposti = await _encryptionService.DecryptAsync(obj.codeposti);

        return obj;
    }
    private async Task<Darkhast> EncryptInfo2(Darkhast obj)
    {
        if (obj.address != null)
            obj.address = await _encryptionService.EncryptAsync(obj.address);

        if (obj.CodeMeli != null)
            obj.CodeMeli = await _encryptionService.EncryptAsync(obj.CodeMeli);

        if (obj.tel != null)
            obj.tel = await _encryptionService.EncryptAsync(obj.tel);

        if (obj.mob != null)
            obj.mob = await _encryptionService.EncryptAsync(obj.mob);

        if (obj.codeposti != null)
            obj.codeposti = await _encryptionService.EncryptAsync(obj.codeposti);
        return obj;
    }


    public static bool CheckHash(Darkhast obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }
    #endregion

    #region Get
    public async Task<DarkhastDTO> GetDataByShod(int shod)
    {
        try
        {
            var model = _context.Darkhast.Where(c => c.shodarkhast == shod).AsEnumerable().FirstOrDefault();
            if (model == null)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات درخواست با شماره درخواست {shod} درخواست یافت نشد", EnumFormName.Darkhast, EnumOperation.Get, shod: shod);
                return null;
            }

            var mapped = _mapper.Map<DarkhastDTO>(model);

            mapped.IsValid = CheckHash(model);

            mapped = await DecryptInfo2(mapped);

            if (!mapped.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات درخواست با شماره درخواست {model.shodarkhast} صحت سنجی داده رد شد",
                    EnumFormName.Darkhast, EnumOperation.Validate, shop: model.shop, shod: shod);
            }

            _historyLogService.PrepareForInsert($" دریافت اطلاعات درخواست با شماره درخواست {model.shodarkhast}", EnumFormName.Darkhast, EnumOperation.Get, shop: model.shop, shod: shod);

            return mapped;
        }
        catch (Exception ex)
        {
            _historyLogService.PrepareForInsert($" خطا در دریافت اطلاعات درخواست با شماره درخواست {shod}", EnumFormName.Darkhast, EnumOperation.Get, shod: shod);
            throw new InvalidOperationException("Failed to retrieve data", ex);
        }
    }
    public bool IsExistRequestNumber(int shDarkhast)
    {
        return _context.Darkhast.Any(c => c.shodarkhast == shDarkhast);
    }
    public int GetLastShod()
    {
        return _context.Darkhast
            .Where(c => c.shodarkhast != null)
            .Select(c => c.shodarkhast)
            .ToList()
            .DefaultIfEmpty(0)
            .Max() + 1;
    }
    #endregion

    #region Add
    public async Task<bool> AddDarkhast(DarkhastDTO darkhast)
    {
        var mapped = _mapper.Map<Darkhast>(darkhast);
        var encrypted = await EncryptInfo2(mapped);
        _context.Darkhast.Add(encrypted);
        var res = await _context.SaveChangesAsync();
        if (res > 0)
        {
            _historyLogService.PrepareForInsert(
                $"ثبت درخواست برای پرونده {darkhast.shop} و شماره درخواست {mapped.shodarkhast}", EnumFormName.Darkhast, EnumOperation.Post, shop: darkhast.shop, shod: darkhast.shodarkhast);
            return true;
        }
        _historyLogService.PrepareForInsert(
            $"خطا در ثبت درخواست برای پرونده {darkhast.shop} و شماره درخواست {mapped.shodarkhast}", EnumFormName.Darkhast, EnumOperation.Post, shop: darkhast.shop, shod: darkhast.shodarkhast);
        return false;
    }
    #endregion
}
