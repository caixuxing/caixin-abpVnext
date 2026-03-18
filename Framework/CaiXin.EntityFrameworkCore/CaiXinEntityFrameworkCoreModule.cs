using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace CaiXin.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreSqlServerModule)
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
                // 使用Configure方法配置所有的DbContext
                // 这里的configurationContext 实际上是 DbContextOptionsBuilder 类型
                // 这个配置会被应用到应用程序中的所有DbContext（包括CaiXinContext和其他可能的DbContext）
                options.Configure(configurationContext =>
                {
                    // 配置使用SQL Server数据库
                    // 不需要显式传入连接字符串，ABP会自动从配置文件的"ConnectionStrings.Default"读取
                    // UseSqlServer() 是Entity Framework Core提供的扩展方法，用于配置数据库提供程序
                    configurationContext.UseSqlServer();

                    // 如果需要更详细的配置，可以这样写：
                    // configurationContext.UseSqlServer(sqlOptions =>
                    // {
                    //     // 设置命令超时时间为60秒
                    //     sqlOptions.CommandTimeout(60);
                    //     // 设置迁移历史表名称
                    //     sqlOptions.MigrationsHistoryTable("__MyMigrationsHistory");
                    //     // 启用失败重试机制（适用于临时性故障）
                    //     sqlOptions.EnableRetryOnFailure(
                    //         maxRetryCount: 3,               // 最大重试次数
                    //         maxRetryDelay: TimeSpan.FromSeconds(30), // 最大重试延迟时间
                    //         errorNumbersToAdd: null);        // 要额外处理的错误号
                    // });
                });

                // 也可以为特定的DbContext单独配置（取消下面的注释可以看到示例）
                // options.Configure<CaiXinContext>(c =>
                // {
                //     c.UseSqlServer(sqlOptions =>
                //     {
                //         // 为CaiXinContext设置特定的架构
                //         sqlOptions.MigrationsHistoryTable("__CaiXin_Migrations", "caixin");
                //     });
                // });
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