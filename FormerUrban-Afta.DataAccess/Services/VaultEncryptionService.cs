using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace FormerUrban_Afta.DataAccess.Services;
public class VaultEncryptionService : IEncryptionService
{
    private readonly VaultOptions _options;
    private readonly VaultTokenProvider _tokenProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _scopeFactory;

    public VaultEncryptionService(IOptions<VaultOptions> options, VaultTokenProvider tokenProvider, IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory)
    {
        _options = options.Value;
        _tokenProvider = tokenProvider;
        _httpClientFactory = httpClientFactory;
        _scopeFactory = scopeFactory;
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var token = await _tokenProvider.GetTokenAsync();
        var client = _httpClientFactory.CreateClient("VaultClient");
        client.DefaultRequestHeaders.Remove("X-Vault-Token");
        client.DefaultRequestHeaders.Add("X-Vault-Token", token);
        return client;
    }

    public async Task<string> EncryptAsync(string plaintext)
    {
        try
        {
            var client = await GetClientAsync();
            var content = new { plaintext = Convert.ToBase64String(Encoding.UTF8.GetBytes(plaintext)) };

            var response = await client.PostAsJsonAsync($"v1/transit/encrypt/{_options.KeyName}", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.GetProperty("data").GetProperty("ciphertext").GetString();
        }
        catch (HttpRequestException ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزنگاری : مشکل در برقراری ارتباط در شبکه", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Encryption service temporarily unavailable", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزنگاری : خطای تایم اوت", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Encryption operation timed out", ex);
        }
        catch (JsonException ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزنگاری : خطا در پاسخ دریافتی از سرویس", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Invalid response from encryption service", ex);
        }
        catch (Exception ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزنگاری : عدم امکان رمزنگاری داده", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Invalid response from encryption service", ex);
        }

    }

    public async Task<string> DecryptAsync(string ciphertext)
    {
        try
        {
            if (!ciphertext.StartsWith("vault:"))
            {
                using var scope = _scopeFactory.CreateScope();
                var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
                historyLog.PrepareForInsert("خطای سرویس رمزگشایی : فرمت نامعتبر برای رمزگشایی داده", EnumFormName.Vault, EnumOperation.Post);
            }

            var client = await GetClientAsync();
            var content = new { ciphertext };

            var response = await client.PostAsJsonAsync($"v1/transit/decrypt/{_options.KeyName}", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var base64 = result.GetProperty("data").GetProperty("plaintext").GetString();
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }

        catch (HttpRequestException ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزگشایی : مشکل در برقراری ارتباط در شبکه", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Encryption service temporarily unavailable", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزگشایی : خطای تایم اوت", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Encryption operation timed out", ex);
        }
        catch (JsonException ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزگشایی : خطا در پاسخ دریافتی از سرویس", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Invalid response from encryption service", ex);
        }
        catch (Exception ex)
        {
            using var scope = _scopeFactory.CreateScope();
            var historyLog = scope.ServiceProvider.GetRequiredService<IHistoryLogService>();
            historyLog.PrepareForInsert("خطای سرویس رمزگشایی : عدم امکان رمزگشایی داده", EnumFormName.Vault, EnumOperation.Post);
            throw new InvalidOperationException("Invalid response from encryption service", ex);
        }
    }
}
