using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Application.MemberApp;

public class MemberApp(ILocalEventBus localEventBus,
                       IRepository<User, Guid> userRepo,
                       IUserRepository userRepository,
                       IGuidGenerator guidGenerator,
                       IAsyncQueryableExecuter queryableExecuter) : ApplicationService, IMemberApp, ITransientDependency
{
    [UnitOfWork]
    public virtual async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
    {
        var user = User.Create(guidGenerator.Create(), cmd.Name, "123456", "11360847");
        await userRepo.InsertAsync(user, false, token);
        await localEventBus.PublishAsync(new MemberRegistrationEto(1, 1136, "15580808032", 100), false);
        return (new() { Code = 200, Data = "成功", Message = "" });
    }
}
