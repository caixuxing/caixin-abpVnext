using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;

namespace CaiXin.NiuMa.Application.Contracts.EmployeeApp
{
    public interface IEmployeeApp
    {
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        Task<string> AddEmployee(CreateEmployeeCmd cmd, CancellationToken token);

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task GetEmployeeById(Guid id);
    }
}
