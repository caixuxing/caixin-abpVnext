using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Cmd;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.WebUI.Controllers
{
    /// <summary>
    /// 首页控制器
    /// 处理网站首页相关的请求和会员注册功能
    /// </summary>
    public class HomeController : AbpController
    {
      
        /// <summary>
        /// 构造函数，注入懒加载服务提供者
        /// </summary>
        /// <param name="lazyServiceProvider">ABP框架的懒加载服务提供者</param>
        public HomeController(IAbpLazyServiceProvider lazyServiceProvider) => LazyServiceProvider = lazyServiceProvider;

        /// <summary>
        /// 获取会员服务实例
        /// 通过懒加载方式获取IMemberApp服务，用于处理会员相关业务逻辑
        /// </summary>
        private IMemberApp MemberApp => LazyServiceProvider.GetRequiredService<IMemberApp>();



        /// <summary>
        /// 首页动作方法
        /// 返回网站主页视图
        /// </summary>
        /// <returns>主页视图结果</returns>
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index() => View();


        /// <summary>
        /// 会员注册接口
        /// 接收会员注册信息并调用应用服务完成注册流程
        /// </summary>
        /// <param name="request">会员注册数据传输对象，包含注册所需的信息</param>
        /// <param name="token">取消令牌，用于支持异步操作的取消</param>
        /// <returns>返回注册操作的结果</returns>
        [HttpPost, Route("/create")]
        public async Task<IResult> Create([FromBody] MemberRegistrationDto request, CancellationToken token) => 
            Results.Ok(await MemberApp.MemberRegistrationAsync(request, token));


    }
}

