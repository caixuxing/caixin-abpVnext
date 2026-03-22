using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Mapster;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Linq;
using Volo.Abp.Uow;

namespace CaiXin.NiuMa.Application.MemberApp
{
    public class MemberApp : ApplicationService, IMemberApp, ITransientDependency
    {
        private ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();

        private IRepository<User, Guid> UserRepo => LazyServiceProvider.LazyGetRequiredService<IRepository<User, Guid>>();

        private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();

        private IAsyncQueryableExecuter QueryableExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

        private IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();


        [UnitOfWork(IsDisabled = true)]
        public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
        {

            // 创建手动工作单元
            using (var uow = UnitOfWorkManager.Begin(true, true))
            {
                // 插入用户
                await UserRepo.InsertAsync(cmd.Adapt<User>(), false, cancellationToken: token);


                // 提交
                await uow.SaveChangesAsync(token);
                await uow.CompleteAsync();
            }



            //await CurrentUnitOfWork?.SaveChangesAsync(token);

            await LocalEventBus.PublishAsync(new MemberRegistrationEto
            {
                Name = cmd.Name,
                Password = cmd.Password,
                Salt = cmd.Salt
            }, false);

            return await Task.FromResult(new ApiResult<string>()
            {
                Code = 200,
                Data = "成功",
                Message = ""
            });
        }
    }
}