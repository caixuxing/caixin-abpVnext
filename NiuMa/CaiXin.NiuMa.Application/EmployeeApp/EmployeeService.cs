using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Dto;
using CaiXin.NiuMa.Domain.Employees;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Guids;
namespace CaiXin.NiuMa.Application.EmployeeApp;


[UnitOfWork]
[ExposeServices(typeof(IEmployeeService))]
internal sealed class EmployeeApp(IGuidGenerator guid,
                                  IConnectionStringResolver _connectionStringResolver,
                                  IEmployeeRepository employeeRepository) : ApplicationService, IEmployeeService
{
    public async Task<string> Create(CreateEmployeeCmd cmd, CancellationToken token)
    {
        string nextSeq = await GenerateNextEmployeeNumberAsync(token);
        EmployeeAgg employee = EmployeeAgg.Create(guid.Create(), nextSeq, cmd.FullName, cmd.Email, cmd.PhoneNumber, cmd.HireDate);
        await employeeRepository.InsertAsync(employee, cancellationToken: token);
        await CurrentUnitOfWork!.SaveChangesAsync(token);
        return employee.Id.ToString();
    }

    /// <summary>
    /// 按Id查询员工信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<EmployeeDto> GetById(Guid id)
    {
        var data = await employeeRepository.FindAsync(id);
        var query = await employeeRepository.GetQueryableAsync();
        var employeeaAll = await query.Include(e => e.SysUser).FirstOrDefaultAsync(e => e.Id == id) ?? throw new ArgumentException("Employee not found");
        var result = employeeaAll.Adapt<EmployeeDto>();
        return result;
    }

    /// <summary>
    /// 生成下一个员工工号
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
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