using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using Microsoft.AspNetCore.Mvc;
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



        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index() => View();


        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpPost, Route("/create")]
        public async Task<IResult> Create([FromBody] MemberRegistrationDto request, CancellationToken token) => Results.Ok(await MemberApp.MemberRegistrationAsync(request, token));


    }
}
