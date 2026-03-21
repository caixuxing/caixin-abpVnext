using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.DataSeed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.Infrastructure
{
    public class NiuMaDbSchemaMigrator : INiuMaDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public NiuMaDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<CaiXinContext>()
                .Database
                .MigrateAsync();
        }
    }
}