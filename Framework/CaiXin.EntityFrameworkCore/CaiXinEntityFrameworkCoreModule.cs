using CaiXin.NiuMa.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace CaiXin.EntityFrameworkCore
{
    [DependsOn(
        typeof(NiuMaDomainModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)

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