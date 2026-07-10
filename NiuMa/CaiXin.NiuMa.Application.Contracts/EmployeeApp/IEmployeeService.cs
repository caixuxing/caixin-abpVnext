using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Dto;
using Volo.Abp.Application.Services;

namespace CaiXin.NiuMa.Application.Contracts.EmployeeApp;



/// <summary>
/// 员工应用
/// </summary>
public interface IEmployeeService : IApplicationService
{
    /// <summary>
    /// 创建员工
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<string> Create(CreateEmployeeCmd cmd, CancellationToken token);

    /// <summary>
    /// 按Id查询员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeDto> GetById(Guid id);
}
