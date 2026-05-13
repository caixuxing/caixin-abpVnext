using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace CaiXin.EventBus
{
    //[DataSeedContributor(Int32.MaxValue)]
    public class CaiXinCAPContributor : IDataSeedContributor, ITransientDependency
    {
        private IBootstrapper _bootstrapper;

        public CaiXinCAPContributor(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // 启动CAP
            await _bootstrapper.BootstrapAsync();
        }
    }
}