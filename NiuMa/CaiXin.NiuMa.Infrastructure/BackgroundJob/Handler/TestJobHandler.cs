using CaiXin.BackgroundJob;
using CaiXin.NiuMa.Infrastructure.BackgroundJob.JobArgs;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.Infrastructure.BackgroundJob.Handler;


[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IJob<TestJobArgs>))]
public class TestJobHandler : IScopedDependency, IJob<TestJobArgs>
{
    public async Task<bool> ExecuteAsync(TestJobArgs args)
    {
        //模拟后台任务处理发送邮件耗时操作
        await Task.Delay(10000);
        return true;
    }
}