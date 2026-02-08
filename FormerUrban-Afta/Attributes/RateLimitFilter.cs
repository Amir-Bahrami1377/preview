using Microsoft.Extensions.Caching.Memory;


public class RateLimitFilter : IAsyncActionFilter
{
    private readonly IMemoryCache _cache;
    private readonly ITarifhaService _tarifhaService;
    private readonly ILogger<RateLimitFilter> _logger;

    public RateLimitFilter(IMemoryCache cache, ITarifhaService tarifhaService = null, ILogger<RateLimitFilter> logger = null)
    {
        _cache = cache;
        _tarifhaService = tarifhaService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var path = context.HttpContext.Request.Path.Value?.ToLower();

        // اگر مسیر، مسیر یکی از صفحات خطا بود، فیلتر اعمال نشود
        if (path != null && path.StartsWith("/error"))
        {
            await next();
            return;
        }

        int _windowSeconds = 30;
        var _permitLimit = 10;
        var tarifha = await _tarifhaService.GetTarifhaAsync();

        if (string.IsNullOrWhiteSpace(tarifha.RequestRateLimitter))
            _permitLimit = Convert.ToInt32(tarifha.RequestRateLimitter);

        var user = context.HttpContext.User.Identity;
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        string key = user?.IsAuthenticated == true
            ? $"rl:user:{user.Name}"
            : $"rl:ip:{ip}";

        var counter = _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_windowSeconds);
            return new RequestCounter();
        });

        if (counter.Count >= _permitLimit)
        {
            _logger.LogWarning(message: $"کاربر {user.Name} از آستانه تعداد درخواست مجاز در دقیقه عبور کرده است");
            context.HttpContext.Response.Headers["Retry-After"] = _windowSeconds.ToString();
            HandleRateLimitFiltered(context);
            return;
        }

        counter.Count++;
        await next();
    }

    private class RequestCounter
    {
        public int Count { get; set; }
    }

    private void HandleRateLimitFiltered(ActionExecutingContext context)
    {
        var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        if (isAjax)
            context.Result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);
        else
            context.Result = new RedirectToActionResult("Error429", "Error", new { area = "" });
    }
}
