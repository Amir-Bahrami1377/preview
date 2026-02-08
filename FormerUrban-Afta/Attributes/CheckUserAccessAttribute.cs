using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace FormerUrban_Afta.Attributes;

public class CheckUserAccessAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string _permissionCode;
    private readonly EnumOperation _type;
    private readonly EnumFormName _table;
    private readonly string _section;

    public CheckUserAccessAttribute(string permissionCode, EnumOperation type, EnumFormName table, string section)
    {
        _permissionCode = permissionCode;
        _type = type;
        _table = table;
        _section = section;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var logEvent = context.HttpContext.RequestServices.GetRequiredService<IEventLogThresholdService>();
        await logEvent.DoCheck(EventLogTableType.Activity);
        //await logEvent.DoCheck(EventLogTableType.Exception);
        await logEvent.DoCheck(EventLogTableType.Login);
        await logEvent.DoCheck(EventLogTableType.Audits);

        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new RedirectToActionResult("Login", "Index", null);
            return;
        }

        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
        var hasAccess = await permissionService.HasPermissionAsync(userId, _permissionCode);

        if (hasAccess)
            await permissionService.CheckHash(userId, context);

        var logHistory = context.HttpContext.RequestServices.GetRequiredService<IHistoryLogService>();
        var userServise = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
        var _cache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var tarifhaService = context.HttpContext.RequestServices.GetRequiredService<ITarifhaService>();
        var tarifha = await tarifhaService.GetTarifhaNoLogAsync();
        if (string.IsNullOrWhiteSpace(tarifha.MaximumAccessDenied))
            tarifha.MaximumAccessDenied = "3";

        var maximumAccessDenied = Convert.ToInt32(tarifha.MaximumAccessDenied);
        if (!hasAccess)
        {
            logHistory.PrepareForInsert(description: $"عدم دسترسی کاربر به بخش {_section}", formName: _table, operation: _type);

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var cacheKey = $"AccessDenied_{ipAddress}";
                int count = _cache.GetOrCreate(cacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                    return 0;
                });

                count++;
                if (count >= maximumAccessDenied)
                {
                    _cache.Remove(cacheKey);
                    await userServise.BlockUserByUserId(userId);
                    context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", AccessDenied = true });
                    return;
                }

                _cache.Set(cacheKey, count, TimeSpan.FromMinutes(1));
            }

            context.Result = new RedirectToActionResult("Error403", "Error", new { area = "" });
        }
    }

}
