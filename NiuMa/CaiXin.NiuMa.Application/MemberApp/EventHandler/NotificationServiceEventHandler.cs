using CaiXin.BackgroundJob;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Infrastructure.BackgroundJob.JobArgs;
using Hangfire;

namespace CaiXin.NiuMa.Application.MemberApp.EventHandler;

/// <summary>
/// 消息通知处理
/// </summary>
internal sealed class NotificationServiceEventHandler(
    ILogger<NotificationServiceEventHandler> _logger,
    Hangfire.IBackgroundJobClient _backgroundJobClient,
    IAbpLazyServiceProvider _lazyServiceProvider) : ILocalEventHandler<MemberRegistrationEto>, ITransientDependency
{
    /// <summary>
    /// 会员创建事件->推送邮件
    /// </summary>
    /// <param name="eto"></param>
    public async Task HandleEventAsync(MemberRegistrationEto eto)
    {
        _logger.LogInformation($"开始推送会员创建成功邮件通知");
        var msg = $"会员ID: {eto.UserId} ，赠送会员积分金额: {eto.TotalAmount}";

        //模拟耗时  耗时任务  发布到后台作业hangfire
        //await Task.Delay(9000);
        //耗时任务丢到后台任中去处理
        var taskId = _backgroundJobClient.Enqueue(() => _lazyServiceProvider.LazyGetRequiredService<IJob<TestJobArgs>>().ExecuteAsync(new TestJobArgs()
        {
            Id = Guid.NewGuid(),
            Name = Newtonsoft.Json.JsonConvert.SerializeObject(msg)
        }));
        if (string.IsNullOrWhiteSpace(taskId))
        {
            throw new("邮件推送发布到后台服务异常！");
        }
        _logger.LogInformation(msg);
        await Task.CompletedTask;
    }
}