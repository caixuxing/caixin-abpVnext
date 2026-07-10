using CaiXin.BackgroundJob;
using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.Employees;
using CaiXin.NiuMa.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.Infrastructure;

[DependsOn(typeof(CaiXinEntityFrameworkCoreModule),
    typeof(CaiXinBackgroundJobModule))]
public class NiuMaInfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // ✅ 注册自定义仓储
        context.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
    }
}