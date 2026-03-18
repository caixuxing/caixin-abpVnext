using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CaiXin.DbMigrator
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class CaiXinDbMigratorModule : AbpModule
    {
    }
}