using Microsoft.Extensions.DependencyInjection;

namespace FormerUrban_Afta.DataAccess.Services
{
    public class EventLogFilterService : IEventLogFilterService
    {
        private readonly FromUrbanDbContext _context;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuditService _auditService;
        private readonly IEncryptionService _encryptionService;

        public EventLogFilterService(FromUrbanDbContext context, IMapper mapper, IServiceProvider serviceProvider,
            IAuditService auditService, IEncryptionService encryptionService)
        {
            _context = context;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _auditService = auditService;
            _encryptionService = encryptionService;
        }

        #region Encryption
        private async Task<EventLogFilter> DecryptInfo(EventLogFilter obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.MustLoginBeLogged))
                obj.MustLoginBeLogged = await _encryptionService.DecryptAsync(obj.MustLoginBeLogged);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar))
                obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar = await _encryptionService.DecryptAsync(obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi))
                obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi = await _encryptionService.DecryptAsync(obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi))
                obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi = await _encryptionService.DecryptAsync(obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeRaddeRamzeObour))
                obj.LogBarayeRaddeRamzeObour = await _encryptionService.DecryptAsync(obj.LogBarayeRaddeRamzeObour);


            return obj;
        }

        private async Task<EventLogFilter> EncryptInfo(EventLogFilter obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.MustLoginBeLogged))
                obj.MustLoginBeLogged = await _encryptionService.EncryptAsync(obj.MustLoginBeLogged);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar))
                obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar = await _encryptionService.EncryptAsync(obj.LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi))
                obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi = await _encryptionService.EncryptAsync(obj.LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi))
                obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi = await _encryptionService.EncryptAsync(obj.LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi);
            if (!string.IsNullOrWhiteSpace(obj.LogBarayeRaddeRamzeObour))
                obj.LogBarayeRaddeRamzeObour = await _encryptionService.EncryptAsync(obj.LogBarayeRaddeRamzeObour);

            return obj;
        }

        public bool CheckHash(EventLogFilter obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

        #endregion

        public EventLogFilterDto Get()
        {
            var _historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();
            var data = _context.EventLogFilter.AsNoTracking().FirstOrDefault();

            if (data == null)
            {
                _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات جدول فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Get);
                return new EventLogFilterDto();
            }

            var isValid = CheckHash(data);
            var decrypted = DecryptInfo(data).GetAwaiter().GetResult();
            var mapped = _mapper.Map<EventLogFilterDto>(decrypted);
            mapped.IsValid = isValid;
            //mapped = DecryptInfo(mapped);
            if (!mapped.IsValid)
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Validate);

            _historyLogService.PrepareForInsert($"دریافت اطلاعات جدول فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Get);

            return mapped;
        }

        public EventLogFilterDto GetAsNoTracking()
        {
            var data = _context.EventLogFilter.AsNoTracking().FirstOrDefault();
            var isValid = CheckHash(data);
            var decrypted = DecryptInfo(data).GetAwaiter().GetResult();
            var mapped = _mapper.Map<EventLogFilterDto>(decrypted);
            mapped.IsValid = isValid;
            return mapped;
        }

        public async Task<bool> Add(EventLogFilterDto obj)
        {
            var _historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();
            var model = _mapper.Map<EventLogFilter>(obj);
            var encrypted = await EncryptInfo(model);

            await _context.EventLogFilter.AddAsync(encrypted);
            var res = await _context.SaveChangesAsync();
            _historyLogService.PrepareForInsert(res > 0 ? "ثبت اطلاعات فیلترهای رویداد ممیزی" : "خطا در ثبت اطلاعات فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Post);

            return Convert.ToBoolean(res);
        }

        public async Task<bool> Update(EventLogFilterDto command)
        {
            var _historyLogService = _serviceProvider.GetRequiredService<IHistoryLogService>();
            var model = _mapper.Map<EventLogFilter>(command);
            var encrypted = await EncryptInfo(model);
            var oldModel = GetAsNoTracking();
            _context.EventLogFilter.Update(encrypted);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                _auditService.GetDifferences<EventLogFilterDto>(oldModel, command, Convert.ToString(oldModel.Identity), EnumFormName.EventLogFilter, EnumOperation.Update);
                _historyLogService.PrepareForInsert($"بروزرسانی جدول فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Update);
                return true;
            }

            _historyLogService.PrepareForInsert($"بروزرسانی جدول فیلترهای رویداد ممیزی", EnumFormName.EventLogFilter, EnumOperation.Update);
            return false;
        }

        public async Task<bool> Exists() => await _context.EventLogFilter.AnyAsync();
    }
}
