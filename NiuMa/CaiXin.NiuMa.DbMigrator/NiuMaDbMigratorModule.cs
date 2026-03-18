using CaiXin.DbMigrator;
using CaiXin.NiuMa.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.DbMigrator
{
    [DependsOn(typeof(CaiXinDbMigratorModule),
        typeof(NiuMaInfrastructureModule))]
    public class NiuMaDbMigratorModule : AbpModule
    {
    }
}