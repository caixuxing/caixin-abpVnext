using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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


        private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();

        private IAsyncQueryableExecuter QueryableExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();


        [UnitOfWork(IsDisabled = true)]
        public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
        {

            if (await UserRepository.AnyAsync(x => x.Name == cmd.Name, token)) return ApiResult<string>.Failure(string.Empty, $"用户名【{cmd.Name}】已存在");

            var user = cmd.Adapt<User>();
            using (var uow = UnitOfWorkManager.Begin(true, true))
            {
                try
                {
                    await UserRepository.InsertAsync(user, false, cancellationToken: token);
                    await uow.SaveChangesAsync(token);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateException ex) when (IsDuplicateKeyException(ex))
                {
                    return ApiResult<string>.Failure($"用户名【{cmd.Name}】已存在");
                }
            }
            await LocalEventBus.PublishAsync(new MemberRegistrationEto
            {
                Name = cmd.Name,
                Password = cmd.Password,
                Salt = cmd.Salt
            }, false);
            return ApiResult<string>.Success(user.Id.ToString(), "注册成功!");
        }
        private bool IsDuplicateKeyException(DbUpdateException ex)
        {
            // SQL Server: 2627 或 2601
            // MySQL: 1062
            // PostgreSQL: 23505
            return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601);
        }
    }
}
