using CaiXin.BackgroundJob;
using CaiXin.NiuMa.Infrastructure.BackgroundJob.JobArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.Infrastructure.BackgroundJob.Handler;

[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IJob<TestJobArgs>))]
internal sealed class TestJobHandler(ILogger<TestJobHandler> _logger) : IScopedDependency, IJob<TestJobArgs>
{
    public async Task<bool> ExecuteAsync(TestJobArgs args)
    {
        //模拟后台任务处理发送邮件耗时操作
        await Task.Delay(5000);
        _logger.LogInformation($"模拟后台邮件通知已发送！");
        return true;
    }
}