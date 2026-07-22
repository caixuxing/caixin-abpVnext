using CaiXin.Domain.Shared.Attributes;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CaiXin.NiuMa.Host.Controllers;


/// <summary>
/// 测试
/// </summary>
[Route("api/[controller]")]
[ApiController]
[WrapResult]
public class TestController(IEmployeeService employeeService) : AbpController, IEmployeeService
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("AddEmployee")]
    public Task<string> Create(CreateEmployeeCmd cmd, CancellationToken token) => employeeService.Create(cmd, token);
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetEmployee/{id}")]
    public Task<EmployeeDto> GetById(Guid id) => employeeService.GetById(id);
}
