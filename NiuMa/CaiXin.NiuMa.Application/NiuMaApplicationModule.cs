using CaiXin.EventBus;
using CaiXin.EventBus.Implements;

namespace CaiXin.NiuMa.Application;

[DependsOn(
    typeof(NiuMaInfrastructureModule),
    typeof(NiuMaApplicationContractsModule),
    typeof(NiuMaDomainModule),
    typeof(CaiXinEventBusModule)
    )]
public class NiuMaApplicationModule : AbpModule
{
}