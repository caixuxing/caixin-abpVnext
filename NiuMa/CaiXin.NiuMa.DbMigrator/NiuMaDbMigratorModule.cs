using CaiXin.DbMigrator;
using CaiXin.NiuMa.Infrastructure;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.DbMigrator
{
    [DependsOn(typeof(CaiXinDbMigratorModule),
        typeof(NiuMaInfrastructureModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class NiuMaDbMigratorModule : AbpModule
    {
    }
}