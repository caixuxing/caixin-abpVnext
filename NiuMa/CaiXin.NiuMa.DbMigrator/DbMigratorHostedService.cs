using CaiXin.NiuMa.Domain.DataSeed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace CaiXin.NiuMa.DbMigrator
{
    internal class DbMigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IServiceScopeFactory serviceScope)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
            _scopeFactory = serviceScope;
        }

        /// <summary>
        /// 启动迁移程序
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("正在启动数据迁移！");
            using var serviceScope = _scopeFactory.CreateScope();

            using var application = await AbpApplicationFactory.CreateAsync<NiuMaDbMigratorModule>(opt =>
              {
                  opt.UseAutofac();
              });

            await application.InitializeAsync();

            await application
                .ServiceProvider
                .GetRequiredService<NiuMaDbMigrationService>()
                .MigrateAsync();

            await application.ShutdownAsync();

            _hostApplicationLifetime.StopApplication();

            Console.WriteLine("正在开始创建数据库！");
            Console.WriteLine("数据库创建已完成！");

            Console.WriteLine("数据迁移已完成！");
        }

        /// <summary>
        /// 结束程序
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"程序结束！！！");
            return Task.CompletedTask;
        }
    }
}