using CaiXin.BackgroundJob;
using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Cmd;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Linq;

namespace CaiXin.NiuMa.Application.MemberApp
{
    /// <summary>
    /// 会员应用服务实现类
    /// 处理会员相关的业务逻辑，包括注册、查询等操作
    /// </summary>
    public class MemberApp : ApplicationService, IMemberApp
    {
        /// <summary>
        /// 获取本地事件总线服务
        /// 用于发布领域事件，实现模块间解耦
        /// </summary>
        private ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetRequiredService<ILocalEventBus>();
        
        /// <summary>
        /// 获取用户仓储服务
        /// 用于执行用户数据的持久化操作
        /// </summary>
        private IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();
        
        /// <summary>
        /// 获取异步查询执行器
        /// 用于执行LINQ查询的异步操作
        /// </summary>
        private IAsyncQueryableExecuter QueryableExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

        /// <summary>
        /// 获取后台任务管理器
        /// 用于管理和调度后台任务
        /// </summary>
        private IBackgroundJobManager BackgroundJobClient => LazyServiceProvider.LazyGetRequiredService<IBackgroundJobManager>();

        /// <summary>
        /// 会员注册异步方法
        /// 验证用户名唯一性，创建新用户并发布注册事件
        /// </summary>
        /// <param name="cmd">会员注册命令对象，包含姓名、密码和盐值</param>
        /// <param name="token">取消令牌，用于支持异步操作的取消</param>
        /// <returns>返回注册结果，成功时包含用户ID，失败时包含错误信息</returns>
        public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
        {
            if (await UserRepository.AnyAsync(x => x.Name == cmd.Name, token))
                return ApiResult<string>.Failure(string.Empty, $"用户名【{cmd.Name}】已存在");
            
            var user = cmd.Adapt<User>();
            await UserRepository.InsertAsync(user, cancellationToken: token);
            
            await LocalEventBus.PublishAsync(new MemberRegistrationEto
            {
                Name = cmd.Name,
                Password = cmd.Password,
                Salt = cmd.Salt
            }, false);
            
            return ApiResult<string>.Success(user.Id.ToString(), "注册成功!");
        }

        /// <summary>
        /// 判断是否为数据库唯一键冲突异常
        /// 主要用于检测SQL Server中的主键或唯一索引冲突
        /// </summary>
        /// <param name="ex">数据库更新异常对象</param>
        /// <returns>如果是唯一键冲突返回true，否则返回false</returns>
        private bool IsDuplicateKeyException(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601);
        }
    }
}