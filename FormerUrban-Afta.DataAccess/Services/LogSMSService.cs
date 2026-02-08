using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.Reports;
using FormerUrban_Afta.DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.Services
{
    public class LogSMSService : ILogSMSService
    {
        private readonly FromUrbanDbContext _context;
        private readonly IHistoryLogService _historyLogService;
        private readonly IAuthService _authService;
        private readonly IDataProtector _protector;
        private readonly MyFunctions _myFunctions;
        private readonly IEncryptionService _encryptionService;

        public LogSMSService(FromUrbanDbContext context, IHistoryLogService historyLogService, IAuthService authService,
            IDataProtectionProvider protector, MyFunctions myFunctions, IEncryptionService encryptionService)
        {
            _context = context;
            _historyLogService = historyLogService;
            _authService = authService;
            _myFunctions = myFunctions;
            _protector = protector.CreateProtector("UserIds");
            _encryptionService = encryptionService;
        }

        #region Shit

        private bool CheckHash(LogSMS obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

        private async Task<string> DecryptInfo(string textSMS)
        {
            if (textSMS != null && textSMS.StartsWith("vault:v"))
                textSMS = await _encryptionService.DecryptAsync(textSMS);

            return textSMS;
        }
        private async Task<string> EncryptInfo(string textSMS)
        {
            if (textSMS != null)
                textSMS = await _encryptionService.EncryptAsync(textSMS);

            return textSMS;
        }

        #endregion


        public async Task<List<LogSmsDto>> SearchAsync(SearchLogSms search)
        {
            var query = _context.LogSMS.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.UserId) && search.UserId != "None")
            {
                search.UserId = _protector.Unprotect(search.UserId);
                query = query.Where(x => x.UserCode.ToString() == search.UserId);
            }

            if (!string.IsNullOrWhiteSpace(search.Date))
            {
                var date1 = _myFunctions.ConvertPersianToGregorian2(search.Date);
                query = query.Where(x => x.DateTimeSMS > date1);
            }

            if (!string.IsNullOrWhiteSpace(search.Mobile))
                query = query.Where(x => x.MobileSMS.Contains(search.Mobile));

            var users = await _context.Users.AsNoTracking().ToListAsync();
            var model = await query.ToListAsync();
            var userDict = users.ToDictionary(u => u.Id, u => u);

            var result = model.Select(x =>
            {
                userDict.TryGetValue(x.UserCode.ToString(), out var user);
                return new LogSmsDto
                {
                    //UserName = user?.UserName,
                    FullName = $"{user?.Name} {user?.Family}",
                    MobileSMS = x.MobileSMS,
                    TextSMS = DecryptInfo(x.TextSMS).GetAwaiter().GetResult(),
                    StatusSMS = x.StatusSMS,
                    DateTimeSMS = x.DateTimeSMS.ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
                    IsValid = CheckHash(x),
                    Id = x.Id
                };
            }).OrderByDescending(x => x.DateTimeSMS).ToList();
            _historyLogService.PrepareForInsert($"نمایش گزارشات پیامک", EnumFormName.LogSMS, EnumOperation.Get);
            return result;
        }

        public LogSMS Insert(string status, string usercode, string text, string mobile, bool doHistoryLog = true)
        {
            var log = new LogSMS
            {
                StatusSMS = status,
                UserCode = usercode,
                DateTimeSMS = DateTime.UtcNow.AddHours(3.5),
                TextSMS = EncryptInfo(text).GetAwaiter().GetResult(),
                MobileSMS = mobile
            };

            
            _context.LogSMS.Add(log);
            _context.SaveChanges();
            return log;
        }

        public async Task<SearchLogSms> GetDrp(SearchLogSms search)
        {
            var users = await _authService.GetAllAsync();
            var userList = users.Select(x => new
            {
                selected = x.Id == search.UserId,
                Id = _protector.Protect(x.Id),
                fullName = $"{x.Name} {x.Family}",
            }).ToList();

            userList.Insert(0, new { selected = false, Id = "", fullName = "کاربر" });
            search.Users = new SelectList(userList, "Id", "fullName");
            search.UserId = userList.FirstOrDefault(x => x.selected)?.Id;

            return search;
        }
    }
}
