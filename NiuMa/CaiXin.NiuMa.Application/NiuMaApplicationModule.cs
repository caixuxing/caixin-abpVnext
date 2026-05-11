namespace CaiXin.NiuMa.Application;

[DependsOn(
    typeof(NiuMaInfrastructureModule),
    typeof(NiuMaApplicationContractsModule),
    typeof(NiuMaDomainModule))]
public class NiuMaApplicationModule : AbpModule
{
}