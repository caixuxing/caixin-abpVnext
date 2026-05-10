using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;

namespace CaiXin.NiuMa.Application.MemberApp;

internal sealed class MemberApp(ILocalEventBus localEventBus,
                       IRepository<User, Guid> userRepo,
                       IUserRepository userRepository,
                       IAsyncQueryableExecuter queryableExecuter) : ApplicationService, IMemberApp, ITransientDependency
{
    [UnitOfWork]
    public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
    {
        Logger.LogInformation("会员 {MemberName} 创建成功", cmd.Name);

        var user = cmd.Adapt<User>();

        await userRepo.InsertAsync(user, false, token);

        await localEventBus.PublishAsync(new MemberRegistrationEto
        {
            OrderId = 1,
            UserId = 1136,
            UserPhone = "15580808032",
            TotalAmount = 100
        }, false);

        Logger.LogInformation("会员注册事件已发布,ID:{Id} ", cmd.Id);

        return (new() { Code = 200, Data = "成功", Message = "" });
    }
}