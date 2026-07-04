using CaiXin.NiuMa.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Dapper;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace CaiXin.EntityFrameworkCore
{
    [DependsOn(
        typeof(NiuMaDomainModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
         typeof(AbpDapperModule)

           )]
    public class CaiXinEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            // 配置连接字符串
            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configuration.GetConnectionString("Default");
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            context.Services.AddAbpDbContext<CaiXinContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // 配置 CAP 使用现有的 SQL Server 连接和 DbContext
            context.Services.AddCap(x =>
            {
                x.UseEntityFramework<CaiXinContext>();
                x.UseSqlServer(configuration.GetConnectionString("Default") ?? string.Empty);
                // 暂时不在这里配置 Transport，因为 Transport 通常在 Web 层配置
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var dbContext = context.ServiceProvider.GetService<CaiXinContext>();
            if (dbContext != null)
            {
                dbContext.Database.Migrate();
            }
        }
    }
}