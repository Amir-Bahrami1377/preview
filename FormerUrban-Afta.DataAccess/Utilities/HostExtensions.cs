using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FormerUrban_Afta.DataAccess.Utilities;
public static class HostExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
        return host;
    }
}

