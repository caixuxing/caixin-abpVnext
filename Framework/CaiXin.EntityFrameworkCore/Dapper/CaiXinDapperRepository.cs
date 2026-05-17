using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace CaiXin.EntityFrameworkCore.Dapper
{
    public abstract class CaiXinDapperRepository<TDbContext> : DapperRepository<TDbContext> where TDbContext : AbpDbContext<TDbContext>
    {
        protected CaiXinDapperRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}