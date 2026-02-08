using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.Services;

public class EventLogThresholdService : IEventLogThresholdService
{
    private readonly IHistoryLogService _historyLogService;
    private readonly ISqlService _sqlService;
    private readonly FromUrbanDbContext _context;
    private readonly IEventLogFilterService _eventLogFilterService;
    private readonly IMapper _mapper;
    private readonly ISendSmsService _sendSmsService;
    private readonly IAuthService _authService;
    private readonly IEncryptionService _encryptionService;
    private readonly IDataProtector _protector;
    private readonly IAuditService _auditService;

    public EventLogThresholdService(IHistoryLogService historyLogService, ISqlService sqlService, FromUrbanDbContext context,
        IEventLogFilterService eventLogFilterService, IMapper mapper, ISendSmsService sendSmsService,
        IAuthService authService, IEncryptionService encryptionService, IDataProtectionProvider provider, IAuditService auditService)
    {
        _historyLogService = historyLogService;
        _sqlService = sqlService;
        _context = context;
        _eventLogFilterService = eventLogFilterService;
        _mapper = mapper;
        _sendSmsService = sendSmsService;
        _authService = authService;
        _encryptionService = encryptionService;
        _auditService = auditService;
        _protector = provider.CreateProtector("UserIds");
    }

    #region Encryption

    private async Task<EventLogThresholdDto> DecryptInfo(EventLogThresholdDto obj)
    {
        if (!string.IsNullOrWhiteSpace(obj.UserId))
            obj.UserId = await _encryptionService.DecryptAsync(obj.UserId);
        if (!string.IsNullOrWhiteSpace(obj.UsersActivityLogCritical))
            obj.UsersActivityLogCritical = await _encryptionService.DecryptAsync(obj.UsersActivityLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersActivityLogWarning))
            obj.UsersActivityLogWarning = await _encryptionService.DecryptAsync(obj.UsersActivityLogWarning);
        if (!string.IsNullOrWhiteSpace(obj.UsersAuditsLogCritical))
            obj.UsersAuditsLogCritical = await _encryptionService.DecryptAsync(obj.UsersAuditsLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersAuditsLogWarning))
            obj.UsersAuditsLogWarning = await _encryptionService.DecryptAsync(obj.UsersAuditsLogWarning);
        if (!string.IsNullOrWhiteSpace(obj.UsersLoginLogCritical))
            obj.UsersLoginLogCritical = await _encryptionService.DecryptAsync(obj.UsersLoginLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersLoginLogWarning))
            obj.UsersLoginLogWarning = await _encryptionService.DecryptAsync(obj.UsersLoginLogWarning);

        return obj;
    }

    private async Task<EventLogThreshold> EncryptInfo(EventLogThreshold obj)
    {
        if (obj.UserId != null)
            obj.UserId = await _encryptionService.EncryptAsync(obj.UserId);
        if (!string.IsNullOrWhiteSpace(obj.UsersActivityLogCritical))
            obj.UsersActivityLogCritical = await _encryptionService.EncryptAsync(obj.UsersActivityLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersActivityLogWarning))
            obj.UsersActivityLogWarning = await _encryptionService.EncryptAsync(obj.UsersActivityLogWarning);
        if (!string.IsNullOrWhiteSpace(obj.UsersAuditsLogCritical))
            obj.UsersAuditsLogCritical = await _encryptionService.EncryptAsync(obj.UsersAuditsLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersAuditsLogWarning))
            obj.UsersAuditsLogWarning = await _encryptionService.EncryptAsync(obj.UsersAuditsLogWarning);
        if (!string.IsNullOrWhiteSpace(obj.UsersLoginLogCritical))
            obj.UsersLoginLogCritical = await _encryptionService.EncryptAsync(obj.UsersLoginLogCritical);
        if (!string.IsNullOrWhiteSpace(obj.UsersLoginLogWarning))
            obj.UsersLoginLogWarning = await _encryptionService.EncryptAsync(obj.UsersLoginLogWarning);
        return obj;
    }

    public static bool CheckHash(EventLogThreshold obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);


    #endregion

    private Semaphore Semaphore { get; set; } = new(1, 1);

    public string ToCustomString(EventLogTableType self) => self switch
    {
        EventLogTableType.Activity => "History",
        EventLogTableType.Login => "UserLogined",
        EventLogTableType.Validation => "Validation",
        EventLogTableType.Audits => "Audits",
        _ => string.Empty
    };

    public string ToPersianString(EventLogTableType self) => self switch
    {
        EventLogTableType.Activity => "فعالیت کاربران",
        EventLogTableType.Login => "ورود/خروج کاربران",
        //EventLogTableType.Exception => "پیام‌های سیستم",
        EventLogTableType.Validation => "اعتبار سنجی داده",
        EventLogTableType.Audits => "ثبت تغییرات داده",
        _ => string.Empty
    };

    public string ToPersianString(EventLogTableUsedSpaceLevel self) => self switch
    {
        EventLogTableUsedSpaceLevel.Normal => "طبیعی",
        EventLogTableUsedSpaceLevel.Warning => "هشدار",
        EventLogTableUsedSpaceLevel.Critical => "بحران",
        _ => string.Empty
    };

    //public async Task<int> EventLogBulkRemove(EventLogTableType tableType, int percentage)
    //{
    //    Semaphore.WaitOne();

    //    var changesCount = 0;
    //    var status = await HasEnoughSpace(tableType);
    //    var info = _sqlService.GetUsedSpaceByTableName(ToCustomString(tableType));

    //    var count = info.Rows / 100 * percentage;
    //    if (count > 0 && status is EventLogTableUsedSpaceLevel.Critical)
    //    {
    //        var command = $"delete top ({count}) from {ToCustomString(tableType)}";
    //        changesCount = await _context.Database.ExecuteSqlRawAsync(command);
    //        Semaphore.Release();
    //        changesCount += await EventLogBulkRemove(tableType, percentage);
    //        return changesCount;
    //    }

    //    Semaphore.Release();
    //    return changesCount;
    //}

    public async Task<int> EventLogBulkRemove(EventLogTableType tableType, int percentage)
    {
        Semaphore.WaitOne();

        var changesCount = 0;
        var status = await HasEnoughSpace(tableType);

        if (status is EventLogTableUsedSpaceLevel.Critical)
        {
            var info = _sqlService.GetUsedSpaceByTableName(ToCustomString(tableType));
            var targetMB = info.DataMB * percentage / 100;

            // Delete in batches until target MB is freed
            var batchSize = 100; // Adjust based on your needs
            var currentMB = info.DataMB;

            while (currentMB > (info.DataMB - targetMB))
            {
                var command = $"DELETE TOP ({batchSize}) FROM {ToCustomString(tableType)} ORDER BY Identity"; // Add appropriate ordering
                var deleted = await _context.Database.ExecuteSqlRawAsync(command);

                if (deleted == 0) break; // No more rows to delete

                changesCount += deleted;
                var newInfo = _sqlService.GetUsedSpaceByTableName(ToCustomString(tableType));
                currentMB = newInfo.DataMB;
            }
        }

        Semaphore.Release();
        return changesCount;
    }

    public async Task<EventLogTableUsedSpaceLevel> DoCheck(EventLogTableType tableType)
    {
        var spaceLevel = await HasEnoughSpace(tableType);

        switch (spaceLevel)
        {
            case EventLogTableUsedSpaceLevel.Warning:
                await HandleWarningLevel(tableType);
                break;

            case EventLogTableUsedSpaceLevel.Critical:
                await HandleCriticalLevel(tableType, spaceLevel);
                break;
        }

        return spaceLevel;
    }

    private async Task HandleWarningLevel(EventLogTableType tableType)
    {
        if (await GetSmsSendStatus(tableType) != ThresholdSmsSendStatus.NotSent)
            return;

        var eventLog = await GetAsync();
        var message = $"حجم جدول ثبت {ToPersianString(tableType)} از آستانه هشدار عبور کرده است.";
        var alert = $"{ToPersianString(tableType)}";
        var phoneNumbers = await _sendSmsService.SendMessageSmsWithRespondToSuperusers(alert, message, eventLog.UserId, bodyId: 340824);
        var shouldLog = _eventLogFilterService.Get().LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi;

        if (phoneNumbers.Any())
        {
            if (shouldLog)
            {
                var phoneNumbersList = string.Join(",", phoneNumbers);
                _historyLogService.PrepareForInsert($"ارسال پیامک حاوی ({alert}) به شماره‌های {phoneNumbersList}", EnumFormName.EventLogThreshold, EnumOperation.Get);
            }
            await SetSmsSendStatus(tableType, ThresholdSmsSendStatus.Sent);
        }
        else if (shouldLog)
            _historyLogService.PrepareForInsert($"عدم توانایی ارسال پیامک حاوی ({alert}). هیچ شماره‌ موبایلی یافت نشد.", EnumFormName.EventLogThreshold, EnumOperation.Get);
    }

    private async Task HandleCriticalLevel(EventLogTableType tableType, EventLogTableUsedSpaceLevel spaceLevel)
    {
        const int cleanupPercentage = 10;
        var removedCount = await EventLogBulkRemove(tableType, cleanupPercentage);

        if (removedCount <= 0)
            return;

        var eventLog = await GetAsync();
        var message = $"حجم جدول ثبت {ToPersianString(tableType)} از آستانه بحران عبور کرده است. اقدام لازم برای کاهش حجم بطور خودکار صورت خواهد گرفت.";
        var alert = $"{ToPersianString(tableType)}";
        var phoneNumbers = await _sendSmsService.SendMessageSmsWithRespondToSuperusers(alert, message, eventLog.UserId, bodyId: 340825);

        var shouldLog = _eventLogFilterService.Get().LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi;

        if (phoneNumbers.Any())
        {
            var phoneNumbersList = string.Join(",", phoneNumbers);
            _historyLogService.PrepareForInsert($"ارسال پیامک حاوی ({alert}) به شماره‌های {phoneNumbersList}", EnumFormName.EventLogThreshold, EnumOperation.Get);
            await TryResetSmsSendStatus(tableType, shouldLog);
        }
        _historyLogService.PrepareForInsert($"عدم توانایی ارسال پیامک حاوی ({alert}). هیچ شماره‌ موبایلی یافت نشد.", EnumFormName.EventLogThreshold, EnumOperation.Get);

        if (shouldLog)
            _historyLogService.PrepareForInsert($"حذف {cleanupPercentage} درصد از رکوردهای قدیمی بمنظور آزادسازی فضای ذخیره‌سازی", EnumFormName.EventLogThreshold, EnumOperation.Delete);
    }

    private async Task TryResetSmsSendStatus(EventLogTableType tableType, bool shouldLog)
    {
        try
        {
            await SetSmsSendStatus(tableType, ThresholdSmsSendStatus.NotSent);
        }
        catch (Exception)
        {
            if (shouldLog)
                _historyLogService.PrepareForInsert("خطا در زمان ارسال پیام عبور از حد آستانه به ادمین", EnumFormName.EventLogThreshold, EnumOperation.Get);
        }
    }

    public async Task<EventLogThresholdDto> Get()
    {
        var data = await _context.EventLogThreshold.AsNoTracking().FirstOrDefaultAsync();
        if (data == null)
        {
            var model = new EventLogThreshold();
            var map = _mapper.Map<EventLogThresholdDto>(model);
            await AddAsync(map);
            return map;
        }

        var mapped = _mapper.Map<EventLogThresholdDto>(data);
        mapped = await DecryptInfo(mapped);
        mapped.IsValid = CheckHash(data);

        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert("رد صحت سنجی داده حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Validate);

        return mapped;
    }

    public async Task<EventLogThresholdDto> GetAsNoTracking()
    {
        var data = await _context.EventLogThreshold.AsNoTracking().FirstOrDefaultAsync();
        var mapped = _mapper.Map<EventLogThresholdDto>(data);
        mapped.IsValid = CheckHash(data);
        mapped = await DecryptInfo(mapped);
        return mapped;
    }

    public async Task<EventLogThresholdDto> GetAsync()
    {
        var data = await _context.EventLogThreshold.FirstOrDefaultAsync();
        if (data == null)
        {
            var model = new EventLogThreshold();
            var map = _mapper.Map<EventLogThresholdDto>(model);
            await AddAsync(map);
            return map;
        }

        var mapped = _mapper.Map<EventLogThresholdDto>(data);
        mapped = await DecryptInfo(mapped);
        mapped.IsValid = CheckHash(data);

        if (!mapped.IsValid)
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Validate);

        _historyLogService.PrepareForInsert($"نمایش اطلاعات حد آستانه رویداد های ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Get);

        mapped = await GetUserDrp(mapped);
        return mapped;
    }

    public async Task<EventLogThresholdDto> GetUserDrp(EventLogThresholdDto model)
    {
        var users = await _authService.GetAllAsync();
        var userList = users.Select(x => new
        {
            selected = x.Id == model.UserId,
            Id = _protector.Protect(x.Id),
            fullName = $"{x.Name} {x.Family}",
        }).ToList();

        userList.Insert(0, new { selected = false, Id = "", fullName = "نامشخص" });
        model.Users = new SelectList(userList, "Id", "fullName");

        model.UserId = userList.FirstOrDefault(x => x.selected)?.Id ?? "";

        return model;
    }

    public async Task<ThresholdSmsSendStatus> GetSmsSendStatus(EventLogTableType tableType)
    {
        var policy = await Get();
        return tableType switch
        {
            EventLogTableType.Login => policy.IsUserLoginLogWarningSmsSent,
            EventLogTableType.Activity => policy.IsUserActivityLogWarningSmsSent,
            EventLogTableType.Audits => policy.IsAuditsLogWarningSmsSent,
            _ => ThresholdSmsSendStatus.NotSpecific,
        };
    }

    public async Task<EventLogTableUsedSpaceLevel> HasEnoughSpace(EventLogTableType tableType)
    {
        var policy = await Get();
        var dataMB = _sqlService.GetUsedSpaceByTableName(ToCustomString(tableType)).DataMB;

        var (warning, critical) = tableType switch
        {
            EventLogTableType.Activity => (Convert.ToInt32(policy.UsersActivityLogWarning), Convert.ToInt32(policy.UsersActivityLogCritical)),
            EventLogTableType.Login => (Convert.ToInt32(policy.UsersLoginLogWarning), Convert.ToInt32(policy.UsersLoginLogCritical)),
            EventLogTableType.Audits => (Convert.ToInt32(policy.UsersAuditsLogWarning), Convert.ToInt32(policy.UsersAuditsLogCritical)),
            _ => (int.MaxValue, int.MaxValue)
        };

        return dataMB >= critical ? EventLogTableUsedSpaceLevel.Critical :
               dataMB >= warning ? EventLogTableUsedSpaceLevel.Warning :
               EventLogTableUsedSpaceLevel.Normal;
    }

    public async Task SetSmsSendStatus(EventLogTableType tableType, ThresholdSmsSendStatus status)
    {
        var policy = await Get();

        _ = tableType switch
        {
            EventLogTableType.Login => policy.IsUserLoginLogWarningSmsSent = status,
            EventLogTableType.Activity => policy.IsUserActivityLogWarningSmsSent = status,
            EventLogTableType.Audits => policy.IsAuditsLogWarningSmsSent = status,
            _ => ThresholdSmsSendStatus.NotSpecific,
        };

        await Update(policy);
    }

    public async Task<bool> AddAsync(EventLogThresholdDto obj)
    {
        var model = _mapper.Map<EventLogThreshold>(obj);
        model = await EncryptInfo(model);
        await _context.EventLogThreshold.AddAsync(model);
        var res = await _context.SaveChangesAsync();
        if (res > 0)
            _historyLogService.PrepareForInsert($"ثبت اطلاعات حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Post);
        else
            _historyLogService.PrepareForInsert($"خطا در ثبت اطلاعات حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Post);

        return Convert.ToBoolean(res);
    }

    public async Task<bool> Update(EventLogThresholdDto command)
    {
        if (!string.IsNullOrWhiteSpace(command.UserId))
        {
            try
            {
                command = await DecryptInfo(command);
            }
            catch (Exception e)
            {
                command.UserId = _protector.Unprotect(command.UserId);
            }
        }

        var model = _mapper.Map<EventLogThreshold>(command);
        var oldModel = await GetAsNoTracking();
        var existingEntity = await _context.EventLogThreshold.FirstOrDefaultAsync(e => e.Identity == model.Identity);
        model = await EncryptInfo(model);

        if (existingEntity != null)
            _context.Entry(existingEntity).CurrentValues.SetValues(model);
        else
            _context.Update(model);

        var res = await _context.SaveChangesAsync();


        if (res > 0)
        {
            _auditService.GetDifferences<EventLogThresholdDto>(oldModel, command, oldModel.Identity.ToString(), EnumFormName.EventLogThreshold, EnumOperation.Update);
            _historyLogService.PrepareForInsert($"بروزرسانی جدول حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Update);
            return true;
        }

        _historyLogService.PrepareForInsert($"خطا در بروزرسانی جدول حد آستانه ی رویدادهای ممیزی", EnumFormName.EventLogThreshold, EnumOperation.Update);
        return false;
    }

    public async Task<bool> ExistsAsync() => await _context.EventLogThreshold.AnyAsync();
}

