using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace CaiXin.NiuMa.Application.MemberApp.EventHandler;

public class NotificationServiceEventHandler : ILocalEventHandler<MemberRegistrationEto>, ITransientDependency
{
    private readonly ILogger<NotificationServiceEventHandler> _logger;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public NotificationServiceEventHandler(ILogger<NotificationServiceEventHandler> logger) => _logger = logger;

    /// <summary>
    /// 会员创建事件->推送邮件
    /// </summary>
    /// <param name="eto"></param>
    public async Task HandleEventAsync(MemberRegistrationEto eto)
    {
        _logger.LogInformation($"开始推送会员创建成功邮件通知");

        var msg = $"会员ID: {eto.Name} ，赠送会员积分金额: {eto.Salt}";

        //模拟耗时  耗时任务  发布到后台作业hangfire
        await Task.Delay(9000);

        //throw new Exception("订阅事件处理异常！");

        _logger.LogInformation(msg);
    }
}