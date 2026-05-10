using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Modularity;

namespace CaiXin.BackgroundJob
{
    [DependsOn(typeof(AbpBackgroundJobsHangfireModule))]
    public class CaiXinBackgroundJobModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            // 获取 Redis 配置（通常从 appsettings.json 读取）
            var configuration = context.Services.GetConfiguration();
            var redisConnection = configuration["Redis:ConnectionString"];
            var redisDb = int.Parse(configuration["Redis:DatabaseId"] ?? "8");

            context.Services.AddHangfire(config =>
            {
                config.UseRedisStorage(redisConnection, new RedisStorageOptions
                {
                    Prefix = "hangfire:caixin:",      // 键名前缀，可选
                    Db = redisDb                     // 指定 Redis 数据库索引
                });
            });
            
            // context.Services.AddHangfireServer(options =>
            // {
            //     options.Queues = new[] { "default", "critical", "email"  };
            //     options.ServerName = $"Hangfire_DbSync_ChasingHouse";
            //     options.SchedulePollingInterval = TimeSpan.FromMicroseconds(1500);
            //     options.HeartbeatInterval = TimeSpan.FromMicroseconds(5000);
            //     options.WorkerCount = 1;
            //
            //
            // });
            
            
            // 添加 Hangfire 服务器（必须调用）
            context.Services.AddHangfireServer(options =>
            {
                options.ServerName = "CaiXinService";   // 可选，自定义服务器名称
            });
            return base.ConfigureServicesAsync(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            
            // 启用 Hangfire Dashboard（可选）
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                // 如需添加认证，可配置 Authorization 选项
                // Authorization = new[] { new BasicAuthAuthorizationFilter(...) }
            });
            return base.OnApplicationInitializationAsync(context);
        }
    }
}
