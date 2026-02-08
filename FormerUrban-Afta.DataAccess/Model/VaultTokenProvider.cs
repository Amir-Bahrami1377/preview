using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace FormerUrban_Afta.DataAccess.Model;
public class VaultTokenProvider
{
    private readonly VaultOptions _options;
    private readonly HttpClient _httpClient;
    private string? _cachedToken;
    private DateTime _tokenExpiry;

    public VaultTokenProvider(IOptions<VaultOptions> options, IHttpClientFactory httpClientFactory)
    {
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient("VaultClient");
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow.AddHours(3.5) < _tokenExpiry)
            return _cachedToken;

        var content = new
        {
            role_id = _options.RoleId,
            secret_id = _options.SecretId
        };

        var response = await _httpClient.PostAsJsonAsync("v1/auth/approle/login", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        _cachedToken = json.GetProperty("auth").GetProperty("client_token").GetString();
        var ttl = json.GetProperty("auth").GetProperty("lease_duration").GetInt32();
        _tokenExpiry = DateTime.UtcNow.AddHours(3.5).AddSeconds(ttl - 30); // Renew 30 seconds early

        return _cachedToken;
    }
}
