using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Linq;
using Volo.Abp.Specifications;

namespace CaiXin.NiuMa.Infrastructure.Repository;




/// <summary>
/// 
/// </summary>
/// <param name="dbContextProvider"></param>
public class EmployeeRepository(IDbContextProvider<CaiXinContext> dbContextProvider,
    IAsyncQueryableExecuter _asyncExecuter

    ) :
    EfCoreRepository<CaiXinContext, EmployeeAgg, Guid>(dbContextProvider),
    IEmployeeRepository,
    ITransientDependency
{

    /// <summary>
    /// 获取员工信息
    /// </summary>
    /// <param name="spec"></param>
    /// <returns></returns>
    public async Task<List<EmployeeAgg>> GetEmployeeAggs(ISpecification<EmployeeAgg> spec)
    {

        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Where(spec.ToExpression())
            .Include(o => o.SysUser)
            .ToListAsync();  // 立即执行，在 DbContext 释放前完成
    }
}

