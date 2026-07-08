using CaiXin.Domain.Shared.Attributes;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CaiXin.NiuMa.WebUI.Controllers
{
    [WrapResult]
    public class EmployeeController(IEmployeeApp employeeApp) : AbpController
    {
        [HttpPost, Route("/AddEmployee")]
        [WrapResult]
        public async Task<string> Create([FromBody] CreateEmployeeCmd request, CancellationToken token)
            => await employeeApp.AddEmployee(request, token);

        [HttpPost, Route("/Employee/{id}")]
        public async Task<IResult> Create([FromRoute] Guid id, CancellationToken token)
        {
            await employeeApp.GetEmployeeById(id);
            return Results.Ok("ok");
        }
    }
}