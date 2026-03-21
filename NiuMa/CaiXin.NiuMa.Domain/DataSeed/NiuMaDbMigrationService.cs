using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.Domain.DataSeed
{
    public class NiuMaDbMigrationService : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;

        private readonly IEnumerable<INiuMaDbSchemaMigrator> _dbSchemaMigrators;

        public NiuMaDbMigrationService(IDataSeeder dataSeeder, IEnumerable<INiuMaDbSchemaMigrator> dbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;
        }

        public async Task MigrateAsync()
        {
            foreach (var item in _dbSchemaMigrators)
            {
                await item.MigrateAsync();
            }
            await _dataSeeder.SeedAsync();
        }
    }
}