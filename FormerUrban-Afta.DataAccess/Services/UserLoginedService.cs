using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using Microsoft.Extensions.DependencyInjection;

namespace FormerUrban_Afta.DataAccess.Services;
public class UserLoginedService : IUserLoginedService
{
    private readonly FromUrbanDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHistoryLogService _historyLogService;
    private readonly MyFunctions _myFunctions;
    private readonly IServiceProvider _serviceProvider;


    public UserLoginedService(FromUrbanDbContext context, IMapper mapper, IHistoryLogService historyLogService, MyFunctions myFunctions, IServiceProvider serviceProvider)
    {
        _context = context;
        _mapper = mapper;
        _historyLogService = historyLogService;
        _myFunctions = myFunctions;
        _serviceProvider = serviceProvider;
    }

    public bool CheckHash(UserLogined obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);


    public List<UserLoginedDto> Search(UserLoginedSearchDto search)
    {
        var query = _context.UserLogined.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.UserName))
            query = query.Where(x => x.UserName.Contains(search.UserName));

        if (!string.IsNullOrWhiteSpace(search.Ip))
            query = query.Where(x => x.Ip.Contains(search.Ip));

        if (!string.IsNullOrWhiteSpace(search.FromDateTime))
        {
            var date = _myFunctions.ConvertPersianToGregorian2(search.FromDateTime);
            query = query.Where(x => x.CreateDateTime >= date);
        }
        if (!string.IsNullOrWhiteSpace(search.ToDateTime))
        {
            var date = _myFunctions.ConvertPersianToGregorian2(search.ToDateTime);
            query = query.Where(x => x.CreateDateTime <= date.AddDays(1));
        }
        if (!string.IsNullOrWhiteSpace(search.ArrivalDate))
        {
            var date = _myFunctions.ConvertPersianToGregorian2(search.ArrivalDate);
            query = query.Where(x => x.LoginDateTime >= date);
        }
        if (!string.IsNullOrWhiteSpace(search.DepartureDate))
        {
            var date = _myFunctions.ConvertPersianToGregorian2(search.DepartureDate);
            query = query.Where(x => x.LogoutDatetime >= date);
        }

        switch (search.ReportType)
        {
            case 1:
                {
                    var status = (byte)UserLoginStatus.Login;
                    query = query.Where(x => x.Status == status);
                    break;
                }
            case 2:
                {
                    var status = (byte)UserLoginStatus.Logout;
                    query = query.Where(x => x.Status == status);
                    break;
                }
            case 3:
                {
                    var status = (byte)UserLoginStatus.Failed;
                    query = query.Where(x => x.Status == status);
                    break;
                }
        }

        if (!query.Any())
        {
            _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات جدول لاگین های کاربران", EnumFormName.UserLogined, EnumOperation.Get);
            return [];
        }
        var results = Map(query.ToList());

        var invalidRecords = results.Where(dto => !dto.IsValid).Select(dto => $"رد صحت سنجی داده لاگین های کاربران با نام {dto.FullName}").ToList();
        if (invalidRecords.Any())
        {
            foreach (var message in invalidRecords)
            {
                _historyLogService.PrepareForInsert(message, EnumFormName.UserLogined, EnumOperation.Validate);
            }
        }

        _historyLogService.PrepareForInsert($"مشاهده اطلاعات لاگین های کاربران", EnumFormName.UserLogined, EnumOperation.Get);

        return results;
    }

    public async Task<List<UserLoginedDto>> GetSuccessFullLogin()
    {
        var auth = _serviceProvider.GetRequiredService<IAuthService>();
        var user = auth.GetCurrentUser();
        var status = (byte)UserLoginStatus.Login;
        var model = await _context.UserLogined.Where(x => x.UserCode == user.Id && x.Status == status)
            .OrderByDescending(x => x.Identity).Take(3).ToListAsync();
        return Map(model);
    }

    public async Task<List<UserLoginedDto>> GetLoginFailed()
    {
        var auth = _serviceProvider.GetRequiredService<IAuthService>();
        var user = auth.GetCurrentUser();
        var status = (byte)UserLoginStatus.Login;
        var successfulLogin = await _context.UserLogined.Where(x => x.UserCode == user.Id && x.Status == status)
            .OrderByDescending(x => x.Identity).Take(2).Select(a => a.Identity).ToListAsync();

        List<UserLogined> model = [];
        if (successfulLogin.Count == 2)
        {
            var minId = successfulLogin.Min(a => a);
            var maxId = successfulLogin.Max(a => a);

            status = (byte)UserLoginStatus.Failed;
            model = await _context.UserLogined.Where(x => x.UserCode == user.Id && x.Status == status && x.Identity > minId && x.Identity < maxId)
               .OrderByDescending(x => x.Identity).ToListAsync();
        }
        else
        {
            status = (byte)UserLoginStatus.Failed;
            model = await _context.UserLogined.Where(x => x.UserCode == user.Id && x.Status == status && x.Identity < successfulLogin.FirstOrDefault())
                .OrderByDescending(x => x.Identity).ToListAsync();
        }

        return Map(model);
    }

    public int Insert(UserLogined userLogined)
    {
        _context.UserLogined.Add(userLogined);
        return _context.SaveChanges();
    }

    private List<UserLoginedDto> Map(List<UserLogined> source)
    {
        var final = source.Select(rec => new UserLoginedDto
        {
            Ip = rec.Ip,
            Identity = rec.Identity,
            Hashed = rec.Hashed,
            Method = rec.Method,
            UserCode = rec.UserCode,
            FullName = rec.FullName,
            UserName = rec.UserName,
            UserAgent = rec.UserAgent,
            LoginDateTime = rec.LoginDateTime,
            LogoutDatetime = rec.LogoutDatetime,
            LoginDateTimeString = rec.LoginDateTime.ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
            LogoutDateTimeString = rec.LogoutDatetime.ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
            ViewerOrLoginer = (rec.UserAgent.EndsWith("*") ? " [ورود دومرحله‌ای] " : string.Empty) + (rec.IsViewer ? "مشاهده کننده" : "ورود/خروج کننده"),
            IsValid = CheckHash(rec),
        }).OrderByDescending(x => x.Identity).ToList();

        return final;
    }

    #region Coment

    //public async Task<List<UserLoginedDto>> Search(string userCode, string ip, DateTime fromDateTime = default, DateTime toDateTime = default,
    //    DateTime arrivalDate = default, DateTime departureDate = default)
    //{
    //    var searchData = _context.UserLogined.AsNoTracking().AsQueryable();

    //    if (string.IsNullOrWhiteSpace(userCode))
    //        searchData = _context.UserLogined.Where(x => x.UserCode == userCode);

    //    if (string.IsNullOrWhiteSpace(ip))
    //        searchData = _context.UserLogined.Where(x => x.Ip == ip);

    //    if (fromDateTime > default(DateTime))
    //        searchData = _context.UserLogined.Where(x => x.CreateDateTime >= fromDateTime);

    //    if (toDateTime > default(DateTime))
    //        searchData = _context.UserLogined.Where(x => x.CreateDateTime <= toDateTime);

    //    if (arrivalDate > default(DateTime))
    //        searchData = _context.UserLogined.Where(x => x.LoginDateTime == arrivalDate);

    //    if (departureDate > default(DateTime))
    //        searchData = _context.UserLogined.Where(x => x.LogoutDatetime == departureDate);

    //    var data = await searchData.ToListAsync();

    //    if (!data.Any())
    //    {
    //        _historyLogService.PrepareForInsert($"خطا در دریافت اطلاعات جدول لاگین های کاربران", EnumFormName.UserLogined, EnumOperation.Get);
    //        return new List<UserLoginedDto>();
    //    }

    //    // Map to DTOs and validate
    //    var results = data.Select(item =>
    //    {
    //        var dto = _mapper.Map<UserLoginedDto>(item);
    //        dto.IsValid = CheckHash(item);
    //        return dto;
    //    }).ToList();

    //    // Log invalid records in a batch
    //    var invalidRecords = results.Where(dto => !dto.IsValid)
    //        .Select(dto => $"رد صحت سنجی داده لاگین های کاربران با نام {dto.FullName}").ToList();

    //    if (invalidRecords.Any())
    //    {
    //        foreach (var message in invalidRecords)
    //        {
    //            _historyLogService.PrepareForInsert(message, EnumFormName.UserLogined, EnumOperation.Validate);
    //        }
    //    }

    //    // Log activity
    //    _historyLogService.PrepareForInsert($"دریافت اطلاعات لاگین های کاربران", EnumFormName.UserLogined, EnumOperation.Get);

    //    return results;
    //}

    //public async Task<UserLoginedDto> GetByUserId(string id)
    //{
    //    var data = await _context.UserLogined.FirstOrDefaultAsync(x => x.UserCode == id);
    //    var model = _mapper.Map<UserLoginedDto>(data);
    //    model.IsValid = CheckHash(data);
    //    if (!model.IsValid)
    //        _historyLogService.PrepareForInsert($"رد صحت سنجی داده لاگین های کاربران با نام {model.FullName}", EnumFormName.UserLogined, EnumOperation.Validate);

    //    _historyLogService.PrepareForInsert($"دریافت اطلاعات لاگین های کاربران با نام {model.FullName} ", EnumFormName.UserLogined, EnumOperation.Get);
    //    return model;
    //}

    //public UserLogined GetLastLogin(string userCode) =>
    //    _context.UserLogined.OrderByDescending(x => x.Identity).FirstOrDefault(x => x.UserCode == userCode && x.LoginDateTime != null && !x.Method.Contains(Strings.Persian.Fields.BadLogin));


    //public List<UserLoginedDto> GetActiveSessionByUserCode(string userCode)
    //{
    //    var query = _context.UserLogined.Where(x => x.UserCode == userCode && !x.Method.Contains(Strings.Persian.Fields.BadLogin)).OrderByDescending(x => x.Identity).Take(3).ToList();
    //    return Map(query);
    //}

    //public List<UserLoginedDto> GetDeactiveSessionByUserCode(string userCode)
    //{
    //    var list = GetActiveSessionByUserCode(userCode);
    //    var lastLoginDate = list.Count > 1 ? list[1]?.LoginDateTime ?? DateTime.MinValue : DateTime.MinValue;
    //    var query = _context.UserLogined.Where(x => x.UserCode == userCode && x.Method.Contains(Strings.Persian.Fields.BadLogin) && x.LoginDateTime >= lastLoginDate).OrderByDescending(x => x.Identity).ToList();

    //    return Map(query);
    //}

    //public void RemoveAllBadLogins(string userName)
    //{
    //    var list = _context.UserLogined.Where(x => x.UserName == userName && !x.Method.Contains(Strings.Persian.Fields.BadLogin)).OrderByDescending(x => x.Identity).Take(3).ToList();
    //    var lastLoginDate = list.Count > 1 ? list[1]?.LoginDateTime ?? DateTime.MinValue : DateTime.MinValue;
    //    var loghistories = _context.UserLogined.Where(x => x.UserName == userName && x.Method == Strings.Persian.Fields.BadLogin && x.LoginDateTime >= lastLoginDate).ToList();

    //    if (!loghistories.Any()) return;

    //    _context.UserLogined.RemoveRange(loghistories);
    //    var result = _context.SaveChanges();
    //}

    //public void RemoveBadLoginById(int Id)
    //{
    //    try
    //    {
    //        var entity = new UserLogined { Identity = Id };
    //        _context.Entry(entity).State = EntityState.Deleted;
    //        var result = _context.SaveChanges();
    //    }

    //    catch (Exception)
    //    {

    //    }
    //}

    //public int ObsoleteFailedLogins(string userName)
    //{
    //    var lastSuccessfulLogin = _context.UserLogined.AsNoTracking().OrderByDescending(x => x.Identity).FirstOrDefault(x => x.UserName == userName && x.LoginDateTime != null && x.Method.Contains("تایید دومرحله‌اي"));
    //    if (lastSuccessfulLogin is null) return default;

    //    var failedLogins = _context.UserLogined.AsNoTracking().Where(x =>
    //        x.LoginDateTime != null && x.LoginDateTime > lastSuccessfulLogin.LoginDateTime && x.UserName == userName && x.Method.Equals(Strings.Persian.Fields.BadLogin)).ToList();

    //    failedLogins.ForEach(each =>
    //    {
    //        each.Method += "*";
    //        _context.Entry(each).State = EntityState.Modified;
    //    });

    //    return _context.SaveChanges();
    //}
    #endregion


}