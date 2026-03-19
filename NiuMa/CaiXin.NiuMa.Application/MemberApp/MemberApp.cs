using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Mapster;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Linq;
using Volo.Abp.Uow;

namespace CaiXin.NiuMa.Application.MemberApp
{
    public class MemberApp : IMemberApp, ITransientDependency
    {
        public MemberApp(IAbpLazyServiceProvider lazyServiceProvider) => LazyServiceProvider = lazyServiceProvider;

        private IAbpLazyServiceProvider LazyServiceProvider { get; }

        private ILogger<MemberApp> Logger => LazyServiceProvider.LazyGetRequiredService<ILogger<MemberApp>>();

        private ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();

        private IRepository<User, Guid> UserRepo => LazyServiceProvider.LazyGetRequiredService<IRepository<User, Guid>>();


        private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();


        private IAsyncQueryableExecuter QueryableExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

        private IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();


        [UnitOfWork]
        public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
        {
            Logger.LogInformation("会员 {MemberName} 创建成功", cmd.Name);

            var user = cmd.Adapt<User>();

            await UserRepo.InsertAsync(user, false, token);


            await UserRepository.InsertManyAsync(new List<User> { user }, cancellationToken: token);


            var query = await UserRepository.GetQueryableAsync();
            var data = await QueryableExecuter.FirstOrDefaultAsync(query.Where(w => w.Name == cmd.Name));


            using var db = await UserRepository.GetDbContextAsync();




            var transaction = await db.Database.BeginTransactionAsync();


            await transaction.CommitAsync();


            await transaction.RollbackAsync();



            await LocalEventBus.PublishAsync(new MemberRegistrationEto
            {
                OrderId = 1,
                UserId = 1136,
                UserPhone = "15580808032",
                TotalAmount = 100
            }, false);

            Logger.LogInformation("会员注册事件已发布,ID:{Id} ", cmd.Id);

            return await Task.FromResult(new ApiResult<string>()
            {
                Code = 200,
                Data = "成功",
                Message = ""
            });
        }
    }
}