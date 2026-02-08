using UAParser;

namespace FormerUrban_Afta.DataAccess.Services;

public class BrowserService : IBrowserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Parser _uaParser;

    public BrowserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _uaParser = Parser.GetDefault(); // بارگذاری پیش‌فرض UAParser
    }

    public string GetBrowserDetails()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return "هیچ HttpContext در دسترس نیست";

        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        if (string.IsNullOrEmpty(userAgent))
            return "User-Agent در دسترس نیست";

        // تجزیه User-Agent با استفاده از UAParser
        ClientInfo client = _uaParser.Parse(userAgent);

        // استخراج اطلاعات مرورگر
        string browserName = client.UA.Family ?? "ناشناخته"; // نام مرورگر
        string browserVersion = $"{client.UA.Major ?? "0"}.{client.UA.Minor ?? "0"}"; // نسخه
        string majorVersion = client.UA.Major ?? "0"; // نسخه اصلی
        string minorVersion = client.UA.Minor ?? "0"; // نسخه فرعی
        string platform = client.OS.Family ?? "ناشناخته"; // پلتفرم
        bool isWin32 = platform.ToLower().Contains("windows"); // آیا ویندوز است

        // ساخت رشته جزئیات (مشابه ساختار اصلی)
        var browserDetails = $"نام:{browserName}, نوع:{browserName}, نسخه:{browserVersion}, نسخه اصلی:{majorVersion}, نسخه فرعی:{minorVersion}, پلتفرم:{platform}, ویندوز 32 است:{isWin32}";

        return browserDetails;
    }
}