using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Dto;
using CaiXin.NiuMa.Application.Contracts.Permissions;
using CaiXin.NiuMa.Domain.Employees;
using CaiXin.NiuMa.Domain.Shared.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Security.Claims;
namespace CaiXin.NiuMa.Application.EmployeeApp;


[ExposeServices(typeof(IEmployeeService))]
[Authorize(CorePermissions.Employees.Default)]
internal class EmployeeApp(IGuidGenerator guid,
                           IConnectionStringResolver _connectionStringResolver,
                           IEmployeeRepository employeeRepository,
                           IAuthorizationService _authorizationService,
                           ICurrentPrincipalAccessor _currentPrincipalAccessor,
                           IOptionsSnapshot<PrivateColud> settings) : ApplicationService, IEmployeeService
{

    [AllowAnonymous]
    public async Task<string> Create(CreateEmployeeCmd cmd, CancellationToken token)
    {
        string nextSeq = await GenerateNextEmployeeNumberAsync(token);
        EmployeeAgg employee = EmployeeAgg.Create(guid.Create(), nextSeq, cmd.FullName, cmd.Email, cmd.PhoneNumber, cmd.HireDate);
        await employeeRepository.InsertAsync(employee, cancellationToken: token);
        return employee.Id.ToString();
    }
    [Authorize(CorePermissions.Employees.Update)]
    public async Task Update(Guid id, CreateEmployeeCmd cmd, CancellationToken token)
    {
        var employee = await employeeRepository.FindAsync(id) ?? throw new UserFriendlyException("员工不存在");
        var authorizationResult = await _authorizationService.AuthorizeAsync(
            _currentPrincipalAccessor.Principal,
            employee.Id.ToString(),
            CorePermissions.Employees.Update);
        if (!authorizationResult.Succeeded) throw new AbpAuthorizationException("没有权限更新该员工信息");
        employee.Resign(DateTime.Now);
        await employeeRepository.UpdateAsync(employee, cancellationToken: token);
    }

    [AllowAnonymous]
    public async Task<EmployeeDto> GetById(Guid id)
    {
        var configData = settings.Value;
        var data = await employeeRepository.FindAsync(id);
        var query = await employeeRepository.GetQueryableAsync();
        var employeeaAll = await query.Include(e => e.SysUser).FirstOrDefaultAsync(e => e.Id == id) ?? throw new UserFriendlyException("Employee not found");
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