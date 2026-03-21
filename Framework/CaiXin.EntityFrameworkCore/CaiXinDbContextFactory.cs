using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.EntityFrameworkCore
{
    public class CaiXinDbContextFactory : IDesignTimeDbContextFactory<CaiXinContext>
    {
        public CaiXinContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfguration();

            var builder = new DbContextOptionsBuilder<CaiXinContext>()
                .UseSqlServer(
                configuration.GetConnectionString("ConnectionString"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                );
            return new CaiXinContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appseting.json", false);
            return builder.Build();
        }
    }
}