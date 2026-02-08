using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FormerUrban_Afta.DataAccess.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FromUrbanDbContext>
    {
        public FromUrbanDbContext CreateDbContext(string[] args)
        {
            // Load configuration manually
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ensure the correct path
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FromUrbanDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new FromUrbanDbContext(optionsBuilder.Options);
        }
    }

}