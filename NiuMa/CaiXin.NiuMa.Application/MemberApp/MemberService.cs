//using CaiXin.EventBus;
using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Application.MemberApp;


[UnitOfWork]
public class MemberService(ILocalEventBus localEventBus,
                       IUserRepository userRepository,
                       IGuidGenerator guidGenerator
                       )
    : ApplicationService, IMemberService
{


    public async Task<ApiResult<string>> CreateAsync(MemberRegistrationDto cmd, CancellationToken token)
    {
        //创建用户
        var user = User.Create(guidGenerator.Create(), cmd.Name, "123456");
        //添加到仓储
        await userRepository.InsertAsync(user);
        //发布本地耗时任务事件
        await localEventBus.PublishAsync(new MemberRegistrationEto(1, 1136, "15580808032", 100), false);
        //发布Cap 用户创建事件
        //await distributedEventBus.PublishAsync(new MemberRegistrationEtos() { Id = 336, EventName = "test" });
        //工作单元提交
        await CurrentUnitOfWork!.SaveChangesAsync(token);

        //返回
        return (new() { Code = 200, Data = "成功", Message = "" });
    }


}