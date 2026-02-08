namespace FormerUrban_Afta.DataAccess.Services;
public class IpService : IIpService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IpService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetIp()
    {
        var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()?.Trim();
        return ip ?? "آدرس IP در دسترس نیست";
    }
}