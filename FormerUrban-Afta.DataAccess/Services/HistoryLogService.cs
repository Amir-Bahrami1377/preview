using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.Reports;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services;

public class HistoryLogService : IHistoryLogService
{
    private readonly MyFunctions _myFunctions;
    private readonly IAuthService _authService;
    private readonly FromUrbanDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActivityLogFiltersService _activityLogFiltersService;
    private readonly IDataProtector _protector;
    private readonly IEncryptionService _encryptionService;

    public HistoryLogService(MyFunctions myFunctions, IAuthService authService,
        FromUrbanDbContext context, IHttpContextAccessor httpContextAccessor,
        IActivityLogFiltersService activityLogFiltersService, IDataProtectionProvider protector, IEncryptionService encryptionService)
    {
        _myFunctions = myFunctions;
        _authService = authService;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _activityLogFiltersService = activityLogFiltersService;
        _protector = protector.CreateProtector("UserIds");
        _encryptionService = encryptionService;
    }


    #region Shit

    private bool CheckHash(History obj) => CipherService.IsEqual(obj.ToString(), obj.Hashed);

    private string DecryptInfo(string message)
    {
        if (message != null && message.StartsWith("vault:v"))
            message = _encryptionService.DecryptAsync(message).GetAwaiter().GetResult();

        return message;
    }
    private string EncryptInfo(string message)
    {
        if (message != null)
            message = _encryptionService.EncryptAsync(message).GetAwaiter().GetResult();

        return message;
    }

    #endregion

    #region Insert


    public History PrepareForInsert(string description, EnumFormName formName, EnumOperation operation, int? shop = 0, int? shod = 0)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string clientIp = httpContext?.Connection?.RemoteIpAddress?.ToString();

        var operationStatus = true;

        var filter = _activityLogFiltersService.GetByFormName(formName);
        if (filter != null)
            operationStatus = IsOperationAllowed(filter, operation);

        if (!operationStatus)
            return new History();

        var userIdentity = _authService.GetCurrentUser();
        if (userIdentity.UserName is null) return default;

        var history = new History
        {
            shop = shop == 0 ? null : shop,
            shod = shod == 0 ? null : shod,
            name_karbar = userIdentity.Name ?? "",
            tarikh = ClsDate.MiladiToShamsiInt(DateTime.UtcNow.AddHours(3.5)),
            saat = _myFunctions.GetTime()?.Trim() ?? "",
            sharh = EncryptInfo(description?.Trim() ?? ""),
            user_name = userIdentity.UserName?.Trim() ?? "",
            name_form = formName,
            noeamal = operation,
            IPAddress = clientIp?.Trim() ?? "",
            CNosazi = shop == 0 ? "" : _myFunctions.GetCodNosazi((long)shop)
        };

        if (shop > 0) history.CNosazi = _myFunctions.GetCodNosazi((long)shop);

        _context.History.Add(history);
        _context.SaveChanges();
        return history;

    }

    private static bool IsOperationAllowed(ActivityLogFilters filter, EnumOperation operation)
    {
        return operation switch
        {
            EnumOperation.Get => filter.GetStatus,
            EnumOperation.Post => filter.AddStatus,
            EnumOperation.Update => filter.UpdateStatus,
            EnumOperation.Delete => filter.DeleteStatus,
            EnumOperation.Validate => true,
            _ => false
        };
    }
    #endregion

    #region Get

    public async Task<List<HistoryDto>> SearchAsync(SearchHistoryDto search)
    {
        var query = _context.History.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.UserId) && search.UserId != "None")
        {
            search.UserId = _protector.Unprotect(search.UserId);
            query = query.Where(x => x.CreateUser.ToString() == search.UserId);
        }

        if (search.TableId != EnumFormName.None && search.TableId != null)
            query = query.Where(x => x.name_form == search.TableId);

        if (search.OperationId != EnumOperation.None && search.OperationId != null)
            query = query.Where(x => x.noeamal == search.OperationId);

        if (!string.IsNullOrWhiteSpace(search.Date))
        {
            var date1 = _myFunctions.ConvertPersianToGregorian2(search.Date);
            query = query.Where(x => x.CreateDateTime > date1);
        }

        if (!string.IsNullOrWhiteSpace(search.Ip))
            query = query.Where(x => x.IPAddress == search.Ip);


        var data = await query.OrderByDescending(x => x.CreateDateTime).Take(search.TotalCount).ToListAsync();

        var users = await _context.Users.AsNoTracking().ToListAsync();
        var userDict = users.ToDictionary(u => u.UserName, u => u);

        var result = data.Select(x =>
        {
            userDict.TryGetValue(x.user_name.ToString(), out var user);
            return new HistoryDto
            {
                UserName = user?.UserName,
                Description = DecryptInfo(x.sharh),
                TableName = GetEnumFormNameDisplayName(x.name_form),
                OperationName = GetEnumOperationDisplayName(x.noeamal),
                //Table = x.name_form,
                //Operation = x.noeamal,
                Ip = x.IPAddress,
                CreationDate = DateTime.SpecifyKind(x.CreateDateTime, DateTimeKind.Local)
                    .ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
                IsValid = CheckHash(x),
                Identity = x.Identity.ToString()
            };
        }).OrderByDescending(x => x.CreationDate).ToList();
        return result;
    }

    public async Task<SearchHistoryDto> GetDrp(SearchHistoryDto command)
    {
        var users = await _authService.GetAllAsync();
        var userList = users.Select(x => new
        {
            selected = x.Id == command.UserId,
            Id = _protector.Protect(x.Id),
            fullName = $"{x.Name} {x.Family}",
        }).ToList();

        userList.Insert(0, new { selected = false, Id = "", fullName = "کاربر" });
        command.Users = new SelectList(userList, "Id", "fullName");

        command.UserId = userList.FirstOrDefault(x => x.selected)?.Id;

        command.Tables = new SelectList(GetEnumFormNameInfo(), "Name", "DisplayName");
        command.Operations = new SelectList(GetEnumOperationInfo(), "Name", "DisplayName");

        return command;
    }

    private List<EnumFormNameInfo> GetEnumFormNameInfo()
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

    private string GetEnumFormNameDisplayName(EnumFormName value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        if (memberInfo.Length > 0)
        {
            var displayAttr = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
                return displayAttr.Name;
        }

        return value.ToString();
    }

    private List<EnumOperationInfo> GetEnumOperationInfo()
    {
        return Enum.GetValues(typeof(EnumOperation))
            .Cast<EnumOperation>()
            .Select(e => new EnumOperationInfo
            {
                Name = e.ToString(),
                Index = (int)e,
                DisplayName = e.GetType()
                    .GetMember(e.ToString())[0]
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();
    }

    private string GetEnumOperationDisplayName(EnumOperation value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString());
        if (memberInfo.Length > 0)
        {
            var displayAttr = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
                return displayAttr.Name;
        }

        return value.ToString();
    }

    #endregion
}