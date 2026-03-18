using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace CaiXin.EntityFrameworkCore
{
    [DependsOn(
         typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule)

           )]
    public class CaiXinEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 从服务容器中获取配置对象（用于读取appsettings.json等配置文件）
            var configuration = context.Services.GetConfiguration();

            // 配置ABP框架的数据库上下文选项
            // AbpDbContextOptions 是ABP用来统一管理所有DbContext配置的选项类
            Configure<AbpDbContextOptions>(options =>
            {

                options.UseSqlServer();

            });

            // 向依赖注入容器注册CaiXinContext（ABP风格的DbContext注册方式）
            // AddAbpDbContext 是ABP提供的扩展方法，用于注册DbContext并自动集成ABP的功能
            context.Services.AddAbpDbContext<CaiXinContext>(options =>
            {
                // 添加默认的仓储实现
                // includeAllEntities: true 表示包括所有实体（包括聚合根之外的实体）都会自动创建仓储
                // 这意味着你可以直接通过仓储访问所有实体，而不需要为每个实体手动创建仓储接口和实现
                options.AddDefaultRepositories(includeAllEntities: true);

                // 如果需要为特定实体自定义仓储，可以这样配置：
                // options.AddRepository<YourEntity, ICustomRepository>();

                // 如果需要替换默认的仓储实现，可以这样配置：
                // options.ReplaceDbContext<ICaiXinDbContext>();
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