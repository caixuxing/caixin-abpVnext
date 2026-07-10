using Volo.Abp.Domain.Repositories;

namespace CaiXin.NiuMa.Domain.Employees;

/// <summary>
/// 员工聚合仓储接口
/// </summary>
public interface IEmployeeRepository : IRepository<EmployeeAgg, Guid>
{

}
