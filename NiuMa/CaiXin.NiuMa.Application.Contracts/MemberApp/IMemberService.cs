using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Domain.Shared.Response;
using Volo.Abp.Application.Services;

namespace CaiXin.NiuMa.Application.Contracts.MemberApp;

/// <summary>
/// 会员服务接口
/// 定义会员相关的业务操作契约
/// </summary>
public interface IMemberService : IApplicationService
{
    /// <summary>
    /// 会员注册异步方法
    /// 处理新用户的注册请求，验证并创建会员账户
    /// </summary>
    /// <param name="cmd">会员注册命令对象，包含姓名、密码和盐值等注册所需信息</param>
    /// <param name="token">取消令牌，用于支持异步操作的取消和控制</param>
    /// <returns>返回包含注册结果的API响应对象，成功时Code为200，失败时Code为400</returns>

    Task<ApiResult<string>> CreateAsync(MemberRegistrationDto cmd, CancellationToken token);

}