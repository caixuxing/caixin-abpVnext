using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Domain.Employees;
using CaiXin.NiuMa.Domain.Employees.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Application.EmployeeApp
{
    [ExposeServices(typeof(IEmployeeApp))]
    [UnitOfWork]
    internal sealed class EmployeeApp(IRepository<Employee, Guid> employeeRepo,
                                      IRepository<SysUser, Guid> userRepo,
                                      IGuidGenerator guid,
                                      IConnectionStringResolver _connectionStringResolver) : ApplicationService, IEmployeeApp, ITransientDependency
    {
        public async Task<string> AddEmployee(CreateEmployeeCmd cmd, CancellationToken token)
        {

            try
            {

                string nextSeq = await GenerateNextEmployeeNumberAsync(token);

                Employee employee = Employee.Create(guid.Create(), nextSeq, cmd.FullName, cmd.Email, cmd.PhoneNumber, cmd.HireDate);
                await employeeRepo.InsertAsync(employee, cancellationToken: token);
                if (employee.SysUser is not null)
                    await userRepo.InsertAsync(employee.SysUser, cancellationToken: token);
                await CurrentUnitOfWork!.SaveChangesAsync(token);
                return employee.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new BusinessException("添加员工失败");
            }
        }

        public async Task GetEmployeeById(Guid id)
        {
            var employeeQuery = await employeeRepo.GetQueryableAsync();
            var data = await employeeQuery.Include(e => e.SysUser).Where(e => e.Id == id).FirstOrDefaultAsync();
            if (data is null) throw new ArgumentException("Employee not found");
            Console.Write(data.Id);
            await Task.CompletedTask;
        }




        /// <summary>
        /// 生成下一个员工工号
        /// </summary>
        /// <returns>格式化的工号，如 "EMP00010001"</returns>
        /// <exception cref="BusinessException">当数据库操作失败时抛出</exception>
        private async Task<string> GenerateNextEmployeeNumberAsync(CancellationToken token)
        {
            try
            {
                var connectionString = await _connectionStringResolver.ResolveAsync<CaiXinContext>();
                if (string.IsNullOrWhiteSpace(connectionString)) throw new BusinessException("数据库连接字符串未配置，无法生成工号");
                int nextSeq;
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(token);
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT NEXT VALUE FOR EmployeeNumberSeq";
                        var result = await command.ExecuteScalarAsync(token);
                        nextSeq = Convert.ToInt32(result);
                    }
                }
                // var employeeNumber = $"EMP{nextSeq:D8}";
                return nextSeq.ToString();
            }
            catch (SqlException ex)
            {
                throw new BusinessException($"数据库操作失败，无法生成工号: {ex.Message}");
            }
            catch (Exception ex)
            {

                throw new BusinessException($"工号生成失败: {ex.Message}");
            }
        }

    }
}