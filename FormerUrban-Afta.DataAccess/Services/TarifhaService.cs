namespace FormerUrban_Afta.DataAccess.Services;
public class TarifhaService : ITarifhaService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEncryptionService _encryptionService;
    private readonly IAuditService _auditService;


    public TarifhaService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, IEncryptionService encryptionService, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _encryptionService = encryptionService;
        _auditService = auditService;
    }


    #region Shit
    private async Task<Tarifha> DecryptInfo(Tarifha obj)
    {
        if (!string.IsNullOrEmpty(obj.sms_user))
            obj.sms_user = await _encryptionService.DecryptAsync(obj.sms_user);

        if (!string.IsNullOrEmpty(obj.sms_pass))
            obj.sms_pass = await _encryptionService.DecryptAsync(obj.sms_pass);

        if (!string.IsNullOrEmpty(obj.UnblockingUserTime))
            obj.UnblockingUserTime = await _encryptionService.DecryptAsync(obj.UnblockingUserTime);

        if (!string.IsNullOrEmpty(obj.RetryLoginCount))
            obj.RetryLoginCount = await _encryptionService.DecryptAsync(obj.RetryLoginCount);

        if (!string.IsNullOrEmpty(obj.MaximumSessions))
            obj.MaximumSessions = await _encryptionService.DecryptAsync(obj.MaximumSessions);

        if (!string.IsNullOrEmpty(obj.KhatemeSessionAfterMinute))
            obj.KhatemeSessionAfterMinute = await _encryptionService.DecryptAsync(obj.KhatemeSessionAfterMinute);

        if (!string.IsNullOrEmpty(obj.RequestRateLimitter))
            obj.RequestRateLimitter = await _encryptionService.DecryptAsync(obj.RequestRateLimitter);

        if (!string.IsNullOrEmpty(obj.PasswordLength))
            obj.PasswordLength = await _encryptionService.DecryptAsync(obj.PasswordLength);

        if (!string.IsNullOrEmpty(obj.MaximumAccessDenied))
            obj.MaximumAccessDenied = await _encryptionService.DecryptAsync(obj.MaximumAccessDenied);

        return obj;
    }
    private async Task<Tarifha> EncryptInfo(Tarifha obj)
    {
        if (!string.IsNullOrEmpty(obj.sms_user))
            obj.sms_user = await _encryptionService.EncryptAsync(obj.sms_user);

        if (!string.IsNullOrEmpty(obj.sms_pass))
            obj.sms_pass = await _encryptionService.EncryptAsync(obj.sms_pass);

        if (!string.IsNullOrEmpty(obj.UnblockingUserTime))
            obj.UnblockingUserTime = await _encryptionService.EncryptAsync(obj.UnblockingUserTime);

        if (!string.IsNullOrEmpty(obj.RetryLoginCount))
            obj.RetryLoginCount = await _encryptionService.EncryptAsync(obj.RetryLoginCount);

        if (!string.IsNullOrEmpty(obj.MaximumSessions))
            obj.MaximumSessions = await _encryptionService.EncryptAsync(obj.MaximumSessions);

        if (!string.IsNullOrEmpty(obj.KhatemeSessionAfterMinute))
            obj.KhatemeSessionAfterMinute = await _encryptionService.EncryptAsync(obj.KhatemeSessionAfterMinute);

        if (!string.IsNullOrEmpty(obj.RequestRateLimitter))
            obj.RequestRateLimitter = await _encryptionService.EncryptAsync(obj.RequestRateLimitter);

        if (!string.IsNullOrEmpty(obj.PasswordLength))
            obj.PasswordLength = await _encryptionService.EncryptAsync(obj.PasswordLength);

        if (!string.IsNullOrEmpty(obj.MaximumAccessDenied))
            obj.MaximumAccessDenied = await _encryptionService.EncryptAsync(obj.MaximumAccessDenied);

        return obj;
    }

    private async Task<SmsSettingDto> EncryptSmsSetting(SmsSettingDto obj)
    {
        if (!string.IsNullOrEmpty(obj.sms_user))
            obj.sms_user = await _encryptionService.EncryptAsync(obj.sms_user);

        if (!string.IsNullOrEmpty(obj.sms_pass))
            obj.sms_pass = await _encryptionService.EncryptAsync(obj.sms_pass);

        return obj;
    }

    public static bool CheckHash(Tarifha obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    private async Task<SmsSettingDto> DecryptSmsInfo(SmsSettingDto obj)
    {
        if (!string.IsNullOrEmpty(obj.sms_user))
            obj.sms_user = await _encryptionService.DecryptAsync(obj.sms_user);

        if (!string.IsNullOrEmpty(obj.sms_pass))
            obj.sms_pass = await _encryptionService.DecryptAsync(obj.sms_pass);

        return obj;
    }

    private async Task<LoginSettingDto> DecryptLoginSettingInfo(LoginSettingDto obj)
    {
        if (!string.IsNullOrEmpty(obj.RetryLoginCount))
            obj.RetryLoginCount = await _encryptionService.DecryptAsync(obj.RetryLoginCount);

        if (!string.IsNullOrEmpty(obj.MaximumSessions))
            obj.MaximumSessions = await _encryptionService.DecryptAsync(obj.MaximumSessions);

        if (!string.IsNullOrEmpty(obj.KhatemeSessionAfterMinute))
            obj.KhatemeSessionAfterMinute = await _encryptionService.DecryptAsync(obj.KhatemeSessionAfterMinute);

        if (!string.IsNullOrEmpty(obj.RequestRateLimitter))
            obj.RequestRateLimitter = await _encryptionService.DecryptAsync(obj.RequestRateLimitter);

        if (!string.IsNullOrEmpty(obj.PasswordLength))
            obj.PasswordLength = await _encryptionService.DecryptAsync(obj.PasswordLength);

        if (!string.IsNullOrEmpty(obj.MaximumAccessDenied))
            obj.MaximumAccessDenied = await _encryptionService.DecryptAsync(obj.MaximumAccessDenied);

        if (!string.IsNullOrEmpty(obj.UnblockingUserTime))
            obj.UnblockingUserTime = await _encryptionService.DecryptAsync(obj.UnblockingUserTime);

        return obj;
    }

    #endregion

    public async Task<Tarifha> GetTarifhaAsync()
    {
        var data = await _context.Tarifha.AsNoTracking().OrderBy(c => c.Identity).FirstOrDefaultAsync();
        if (data != null)
        {
            var decryptedData = await DecryptInfo(data);
            _historyLogService.PrepareForInsert($"دریافت اطلاعات تعریف ها", EnumFormName.Tarifha, EnumOperation.Get);
            return decryptedData;
        }

        var newRecord = Seed();
        var encryptedRecord = await EncryptInfo(newRecord);

        await _context.Tarifha.AddAsync(encryptedRecord);
        await _context.SaveChangesAsync();

        data = await _context.Tarifha.AsNoTracking().OrderBy(c => c.Identity).FirstOrDefaultAsync();
        var decryptedData2 = await DecryptInfo(data);
        _historyLogService.PrepareForInsert($"دریافت اطلاعات تعریف ها", EnumFormName.Tarifha, EnumOperation.Get);
        return decryptedData2;
    }

    public async Task<Tarifha> GetTarifhaNoLogAsync()
    {
        var data = await _context.Tarifha.AsNoTracking().OrderBy(c => c.Identity).FirstOrDefaultAsync();
        if (data != null)
        {
            var decryptedData = await DecryptInfo(data);
            return decryptedData;
        }

        var newRecord = Seed();
        var encryptedRecord = await EncryptInfo(newRecord);

        await _context.Tarifha.AddAsync(encryptedRecord);
        await _context.SaveChangesAsync();

        data = await _context.Tarifha.OrderBy(c => c.Identity).FirstOrDefaultAsync();
        var decryptedData2 = await DecryptInfo(data);
        return decryptedData2;
    }

    public async Task<SmsSettingDto> GetSmsSetting()
    {
        var data = await _context.Tarifha.AsNoTracking().OrderBy(c => c.Identity).FirstOrDefaultAsync() ?? Seed();

        _historyLogService.PrepareForInsert($"مشاهده مدیریت پنل پیامکی", EnumFormName.Tarifha, EnumOperation.Get);

        var mapped = _mapper.Map<SmsSettingDto>(data);
        mapped.IsValid = CheckHash(data);
        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول تعریف ها", EnumFormName.Tarifha, EnumOperation.Validate);

        mapped = await DecryptSmsInfo(mapped);
        return mapped;
    }

    public async Task<bool> UpdateSmsSetting(SmsSettingDto command)
    {
        var tarifha = await _context.Tarifha.OrderBy(c => c.Identity).FirstOrDefaultAsync() ?? new Tarifha();

        command = await EncryptSmsSetting(command);
        tarifha.sms_user = command.sms_user;
        tarifha.sms_pass = command.sms_pass;

        var oldModel = await GetSmsSetting();
        _context.Tarifha.Update(tarifha);
        _context.SaveChanges();
        _auditService.GetDifferences<SmsSettingDto>(oldModel, command, tarifha.Identity.ToString(), EnumFormName.Tarifha, EnumOperation.Update);

        _historyLogService.PrepareForInsert($"ویرایش مدیریت پنل پیامکی", EnumFormName.Tarifha, EnumOperation.Update);

        return true;
    }

    public async Task<LoginSettingDto> GetLoginSetting()
    {
        var data = await _context.Tarifha.AsNoTracking().OrderBy(c => c.Identity).FirstOrDefaultAsync() ?? Seed();

        var mapped = _mapper.Map<LoginSettingDto>(data);
        mapped.IsValid = CheckHash(data);
        //        mapped = await DecryptLoginInfo(mapped);
        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول تعریف ها", EnumFormName.Tarifha, EnumOperation.Validate);

        _historyLogService.PrepareForInsert($"مشاهده مدیریت لاگین", EnumFormName.Tarifha, EnumOperation.Get);
        mapped = await DecryptLoginSettingInfo(mapped);
        return mapped;
    }

    public async Task<bool> UpdateLoginSetting(LoginSettingDto command)
    {
        var tarifha = await GetTarifhaAsync();
        tarifha.RetryLoginCount = command.RetryLoginCount;
        tarifha.KhatemeSessionAfterMinute = command.KhatemeSessionAfterMinute;
        tarifha.MaximumSessions = command.MaximumSessions;
        tarifha.MaximumAccessDenied = command.MaximumAccessDenied;
        tarifha.RequestRateLimitter = command.RequestRateLimitter;
        tarifha.PasswordLength = command.PasswordLength;
        tarifha.UnblockingUserTime = command.UnblockingUserTime;
        var encrypted = await EncryptInfo(tarifha);

        var oldModel = await GetLoginSetting();
        _context.Tarifha.Update(encrypted);
        await _context.SaveChangesAsync();
        _auditService.GetDifferences<LoginSettingDto>(oldModel, command, tarifha.Identity.ToString(), EnumFormName.Tarifha, EnumOperation.Update);

        _historyLogService.PrepareForInsert($"ویرایش مدیریت لاگین", EnumFormName.Tarifha, EnumOperation.Update);

        return true;
    }

    private Tarifha Seed()
    {
        return new Tarifha()
        {
            RetryLoginCount = "3",
            KhatemeSessionAfterMinute = "5",
            MaximumSessions = "3",
            MaximumAccessDenied = "3",
            PasswordLength = "12",
            sms_pass = "tk@LNRYxqeaPfe8U",
            sms_user = "09212364470",
            UnblockingUserTime = "1",
        };
    }

}