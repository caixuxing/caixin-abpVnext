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
    IBackgroundJobClient _backgroundJobClient,
    IJob<TestJobArgs> _testJob) : ILocalEventHandler<MemberRegistrationEto>, ITransientDependency
{
    /// <summary>
    /// 会员创建事件->推送邮件
    /// </summary>
    /// <param name="eto"></param>
    public Task HandleEventAsync(MemberRegistrationEto eto)
    {
        _logger.LogInformation($"开始推送会员创建成功邮件通知");
        var msg = $"会员ID: {eto.UserId} ，赠送会员积分金额: {eto.TotalAmount}";
        _backgroundJobClient.Enqueue(() => _testJob.ExecuteAsync(new TestJobArgs()
        {
            Id = Guid.NewGuid(),
            Name = Newtonsoft.Json.JsonConvert.SerializeObject(msg)
        }));
        _logger.LogInformation(msg);
        return Task.CompletedTask;
    }
}