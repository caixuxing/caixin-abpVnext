using CaiXin.BackgroundJob;
using CaiXin.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.Infrastructure;

[DependsOn(typeof(CaiXinEntityFrameworkCoreModule),
    typeof(CaiXinBackgroundJobModule))]
public class NiuMaInfrastructureModule : AbpModule
{
}