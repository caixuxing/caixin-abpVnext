using CaiXin.NiuMa.Application.Contracts.EmployeeApp;
using CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CaiXin.NiuMa.WebUI.Controllers
{

    public class EmployeeController(IEmployeeApp employeeApp) : AbpController
    {

        [HttpPost, Route("/AddEmployee")]
        public async Task<IResult> Create([FromBody] CreateEmployeeCmd request, CancellationToken token)
            => Results.Ok(await employeeApp.AddEmployee(request, token));



        [HttpPost, Route("/Employee/{id}")]
        public async Task<IResult> Create([FromRoute] Guid id, CancellationToken token)
        {
            await employeeApp.GetEmployeeById(id);
            return Results.Ok("ok");
        }


    }


}
