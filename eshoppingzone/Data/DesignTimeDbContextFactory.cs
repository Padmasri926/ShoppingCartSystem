using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EshoppingZone.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Return new AppDbContext instance
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
