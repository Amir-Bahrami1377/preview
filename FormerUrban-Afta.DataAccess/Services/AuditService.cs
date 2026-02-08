using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.Reports;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Services;

public class AuditService : IAuditService
{
    private readonly FromUrbanDbContext _context;
    private readonly IAuthService _authService;
    private readonly IIpService _ipService;
    private readonly IDataProtector _protector;
    private readonly MyFunctions _myFunctions;
    private readonly IAuditFilterService _auditFilterService;
    private readonly IEncryptionService _encryptionService;

    public AuditService(FromUrbanDbContext context, IAuthService authService, IIpService ipService,
        IDataProtectionProvider protector, MyFunctions myFunctions, IAuditFilterService auditFilterService, IEncryptionService encryptionService)
    {
        _context = context;
        _authService = authService;
        _ipService = ipService;
        _myFunctions = myFunctions;
        _auditFilterService = auditFilterService;
        _protector = protector.CreateProtector("UserIds");
        _encryptionService = encryptionService;
    }

    #region Shit
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

    public void GetDifferences<T>(T oldObject, T newObject, string identity, EnumFormName enumFormName, EnumOperation enumOperation)
    {
        if (_auditFilterService.ExistsById(enumFormName))
            return;

        if (oldObject == null || newObject == null)
            return;

        var type = typeof(T);
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            // Skip if property is not readable or not writable (optional)
            if (!prop.CanRead) continue;

            var originValue = prop.GetValue(oldObject);
            var currentValue = prop.GetValue(newObject);

            if (prop.Name is "CreateDateTime" or "ModifiedDate" or "CreateUser" or "ModifiedUser" or "Hashed"
                or "ConcurrencyStamp" or "SecurityStamp" or "NormalizedUserName" or "IsValid" or "Id"
                or "UserId" or "SessionId" or "Identity" or "NormalizedEmail" or "CodeMarhale")
                continue;

            bool areEqual = Equals(originValue, currentValue);

            var field = MyFunction2.GetDescriptionFromName<PropertyName>(prop.Name);
            if (!areEqual)
            {
                var audit = new Audit()
                {
                    Action = enumOperation,
                    Field = EncryptInfo(field),
                    EntityId = EncryptInfo(identity),
                    Form = enumFormName,
                };
                switch (prop.Name)
                {
                    case "PasswordHash":
                        {
                            audit.OriginValue = EncryptInfo("تغییر رمز عبور");
                            audit.CurrentValue = EncryptInfo("");
                            break;
                        }
                    default:
                        {
                            audit.OriginValue = EncryptInfo(originValue?.ToString() ?? "");
                            audit.CurrentValue = EncryptInfo(currentValue?.ToString() ?? "");
                            break;
                        }
                }
                Add(audit);
            }
        }
    }

    public int Add(Audit entity)
    {
        entity.IpAddress = _ipService.GetIp();
        entity.ChangedBy = _authService.GetCurrentUser().UserName ?? "";

        _context.Audits.Add(entity);
        CipherService.Hash(entity.ToString());
        return _context.SaveChanges();
    }

    public async Task<List<AuditDto>> GetAllAsync(AuditSearchDto search)
    {
        var query = _context.Audits.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.UserId) && search.UserId != "None")
        {
            search.UserId = _protector.Unprotect(search.UserId);
            var user = await _authService.GetAsync(search.UserId);
            query = query.Where(x => x.ChangedBy.ToString() == user.UserName);
        }

        if (search.TableId != EnumFormName.None && search.TableId != null)
            query = query.Where(x => x.Form == search.TableId);

        if (!string.IsNullOrWhiteSpace(search.Date))
        {
            var date1 = _myFunctions.ConvertPersianToGregorian2(search.Date);
            query = query.Where(x => x.ChangedAt > date1);
        }

        if (!string.IsNullOrWhiteSpace(search.Ip))
            query = query.Where(x => x.IpAddress == search.Ip);

        if (!string.IsNullOrWhiteSpace(search.EntityId))
            query = query.Where(x => x.EntityId == search.EntityId);

        var data = await query
            .OrderByDescending(x => x.ChangedAt)
            .Take(search.TotalCount)
            .AsNoTracking()
            .ToListAsync();

        var users = await _context.Users.AsNoTracking().ToListAsync();
        var userDict = users.ToDictionary(u => u.UserName, u => u);


        var result = data.Select(x =>
        {
            userDict.TryGetValue(x.ChangedBy.ToString(), out var user);
            return new AuditDto
            {
                IpAddress = x.IpAddress,
                TableName = GetEnumFormNameDisplayName(x.Form),
                OperationName = GetEnumOperationDisplayName(x.Action),
                CreationDate = DateTime.SpecifyKind(x.ChangedAt, DateTimeKind.Local)
                    .ToPersianDateTimeString("yyyy/MM/dd HH:mm:ss", true),
                OriginValue = DecryptInfo(x.OriginValue),
                CurrentValue = DecryptInfo(x.CurrentValue),
                FullName = $"{user?.Name} {user?.Family}",
                Field = DecryptInfo(x.Field),
                EntityId = DecryptInfo(x.EntityId),
                IsValid = CipherService.IsEqual(x.ToString(), x.Hashed),
                Id = x.Id
            };
        }).ToList();
        return result;
    }

    public async Task<AuditSearchDto> GetDrp(AuditSearchDto command)
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
}