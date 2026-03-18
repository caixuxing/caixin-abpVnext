using CaiXin.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.Infrastructure;

[DependsOn(typeof(CaiXinEntityFrameworkCoreModule))]
public class NiuMaInfrastructureModule : AbpModule
{
}