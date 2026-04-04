using CaiXin.BackgroundJob;
using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using CaiXin.NiuMa.Infrastructure.BackgroundJob.JobArgs;
using Hangfire;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace CaiXin.NiuMa.Application.MemberApp.EventHandler;

/// <summary>
/// 消息通知处理
/// </summary>
public class NotificationServiceEventHandler : ILocalEventHandler<MemberRegistrationEto>, ITransientDependency
{
    private readonly ILogger<NotificationServiceEventHandler> _logger;

    private readonly Hangfire.IBackgroundJobClient _backgroundJobClient;

    public readonly IAbpLazyServiceProvider _lazyServiceProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="backgroundJobClient"></param>
    /// <param name="lazyServiceProvider"></param>
    public NotificationServiceEventHandler(
        ILogger<NotificationServiceEventHandler> logger,
        IBackgroundJobClient backgroundJobClient,
        IAbpLazyServiceProvider lazyServiceProvider)
    {
        this._logger = logger;
        this._backgroundJobClient = backgroundJobClient;
        this._lazyServiceProvider = lazyServiceProvider;
    }

    /// <summary>
    /// 会员创建事件->推送邮件
    /// </summary>
    /// <param name="eto"></param>
    public async Task HandleEventAsync(MemberRegistrationEto eto)
    {
        _logger.LogInformation($"开始推送会员创建成功邮件通知");
        var msg = $"会员ID: {eto.Name} ，赠送会员积分金额: {eto.Salt}";

        //模拟耗时  耗时任务  发布到后台作业hangfire
        //await Task.Delay(9000);
        
        //耗时任务丢到后台任中去处理
       var taskId=_backgroundJobClient.Enqueue(() => _lazyServiceProvider.LazyGetRequiredService<IJob<TestJobArgs>>().ExecuteAsync(new TestJobArgs()
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