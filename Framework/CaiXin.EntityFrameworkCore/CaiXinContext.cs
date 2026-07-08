using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace CaiXin.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public partial class CaiXinContext : AbpDbContext<CaiXinContext>
    {
        public CaiXinContext(DbContextOptions<CaiXinContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // 方式1：直接调用 ApplyConfiguration
            //builder.ApplyConfiguration(new UserConfig());

            // 方式2：如果你有多个配置，可以扫描程序集自动注册（见下文）
            builder.ApplyConfigurationsFromAssembly(typeof(CaiXinContext).Assembly);

            //工号序列
            builder.HasSequence<int>("EmployeeNumberSeq")
           .StartsAt(10001)
           .IncrementsBy(1);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 将 SQL 输出到控制台
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            // 可选：开启敏感数据日志（会显示参数值）
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}