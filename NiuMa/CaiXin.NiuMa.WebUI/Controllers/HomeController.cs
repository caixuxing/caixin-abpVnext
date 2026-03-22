using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.WebUI.Controllers
{
    public class HomeController : AbpController
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lazyServiceProvider"></param>
        public HomeController(IAbpLazyServiceProvider lazyServiceProvider) => LazyServiceProvider = lazyServiceProvider;
        /// <summary>
        /// 懒加载服务提供者
        /// </summary>
        private IAbpLazyServiceProvider LazyServiceProvider { get; }

        private IMemberApp MemberApp => LazyServiceProvider.GetRequiredService<IMemberApp>();




        public async Task<IActionResult> Index()
        {

            var result = await MemberApp.MemberRegistrationAsync(new MemberRegistrationDto()
            {

                Name = "张三",
                Password = "123123",
                Salt = "11360847"
            }, default);

            return View();
        }


        [HttpPost, Route("/create")]
        public async Task<IResult> Create([FromBody] MemberRegistrationDto request, CancellationToken token)
        {
            var result = await MemberApp.MemberRegistrationAsync(request, token);
            return Results.Ok(result);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
