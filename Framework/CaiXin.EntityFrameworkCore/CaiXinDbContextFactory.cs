using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CaiXin.EntityFrameworkCore
{
    public class CaiXinDbContextFactory : IDesignTimeDbContextFactory<CaiXinContext>
    {
        public CaiXinContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfguration();

            var builder = new DbContextOptionsBuilder<CaiXinContext>()
                .UseSqlServer(
                configuration.GetConnectionString("Default"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                );
            return new CaiXinContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false);
            return builder.Build();
        }
    }
}