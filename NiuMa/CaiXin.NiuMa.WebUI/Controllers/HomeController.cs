using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace CaiXin.NiuMa.WebUI.Controllers;

public class HomeController(IMemberApp memberApp) : AbpController
{
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    public IActionResult Index() => View();

    [HttpPost, Route("/create")]
    public async Task<IResult> Create([FromBody] MemberRegistrationDto request, CancellationToken token)
        => Results.Ok(await memberApp.MemberRegistrationAsync(request, token));
}