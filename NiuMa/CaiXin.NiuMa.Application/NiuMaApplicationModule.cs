using CaiXin.NiuMa.Application.Contracts;
using CaiXin.NiuMa.Domain;
using CaiXin.NiuMa.Infrastructure;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.Application
{
    [DependsOn(
      typeof(NiuMaInfrastructureModule),
      typeof(NiuMaApplicationContractsModule),
      typeof(NiuMaDomainModule)
    )]
    public class NiuMaApplicationModule : AbpModule
    {
    }
}