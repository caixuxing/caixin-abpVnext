using CaiXin.EntityFrameworkCore;
using CaiXin.EventBus;
using CaiXin.NiuMa.Application.Contracts.MemberApp;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Commands;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Dto;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Qry;
using CaiXin.NiuMa.Domain.Member;
using CaiXin.NiuMa.Domain.Shared.Response;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Application.MemberApp;

[ExposeServices(typeof(IMemberApp))]
[UnitOfWork]
internal sealed class MemberApp(ILocalEventBus localEventBus,
                       IUserRepository userRepository,
                       IGuidGenerator guidGenerator,
                       IAsyncQueryableExecuter queryableExecuter,
                       ICaiXinDistributedEventBus distributedEventBus,
                        IDbContextProvider<CaiXinContext> _dbContextProvider
                       //,
                       //IPageQuery<MemberPageQry, MemberPageDto> pageQuery
                       )
    : ApplicationService, IMemberApp, ITransientDependency
{
    public async Task<ApiResult<string>> MemberRegistrationAsync(MemberRegistrationDto cmd, CancellationToken token)
    {
        //创建用户
        var user = User.Create(guidGenerator.Create(), cmd.Name, "123456");
        //添加到仓储
        await userRepository.InsertAsync(user);

        await userRepository.TestSql(user, (name, pwd, salt) => buildSql(name, pwd, salt));

        //发布本地耗时任务事件
        await localEventBus.PublishAsync(new MemberRegistrationEto(1, 1136, "15580808032", 100), false);
        //发布Cap 用户创建事件
        await distributedEventBus.PublishAsync(new MemberRegistrationEtos() { Id = 336, EventName = "test" });

        //工作单元提交
        await CurrentUnitOfWork!.SaveChangesAsync(token);

        // 1. 获取 DbContext
        var dbContext = await _dbContextProvider.GetDbContextAsync();

        //返回
        return (new() { Code = 200, Data = "成功", Message = "" });
    }

    public async Task<ApiResult<List<MemberPageDto>>> MemberRegistrationAsync(MemberPageQry qry, CancellationToken token)
    {
        // var (data, allCount) = await pageQuery.PageQueryAsync(qry, token);

        return new ApiResult<List<MemberPageDto>>()
        {
            Data = null,
        };
    }

    public async Task<List<MemberPageDto>> QueryAsync(MemberPageQry request, CancellationToken token = default)
    {
        var query = await userRepository.GetQueryableAsync();
        query = query.WhereIf(!request.UserName.IsNullOrWhiteSpace(), b => b.Name.Value.Contains(request.UserName))
            .OrderBy(b => b.Id);

        var data = await query.ToListAsync();
        // 2. 分别执行异步操作
        var pagedBooks = await queryableExecuter.ToListAsync(query.PageBy(1, 10));

        var totalCount = await queryableExecuter.CountAsync(query);
        // 3. 映射返回
        var result = ObjectMapper.Map<List<User>, List<MemberPageDto>>(pagedBooks);

        return result;
    }

    private Task<string> buildSql(string name, string pwd, string salt)
    {
        var sql = $"INSERT INTO `user` (`id`, `name`, `password`, `salt`) VALUES ('{guidGenerator.Create()}', '{name}', '{pwd}', '{salt}');";
        return Task.FromResult(sql);
    }
}