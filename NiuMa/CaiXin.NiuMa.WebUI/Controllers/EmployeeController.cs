using CaiXin.Domain.Shared.Attributes;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CaiXin.NiuMa.WebUI.Controllers
{
    [WrapResult]
    public class EmployeeController(IEmployeeService employeeApp) : AbpController
    {
        [HttpPost, Route("/AddEmployee")]
        public async Task<string> Create([FromBody] CreateEmployeeCmd request, CancellationToken token)
            => await employeeApp.Create(request, token);

        [HttpPost, Route("/Employee/{id}")]
        public async Task<IResult> Create([FromRoute] Guid id, CancellationToken token)
        {
            await employeeApp.GetById(id);
            return Results.Ok("ok");
        }
    }
}