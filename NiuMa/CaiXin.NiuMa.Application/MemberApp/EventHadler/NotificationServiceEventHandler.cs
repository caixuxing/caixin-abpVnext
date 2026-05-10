using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;

namespace CaiXin.NiuMa.Application.MemberApp.EventHadler;

internal sealed class NotificationServiceEventHandler(ILogger<NotificationServiceEventHandler> _logger)
    : ILocalEventHandler<MemberRegistrationEto>, ITransientDependency
{
    /// <summary>
    /// 会员创建事件->推送邮件
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public async Task HandleEventAsync(MemberRegistrationEto eto)
    {
        _logger.LogInformation($"开始推送会员创建成功邮件通知");

        var message = $"会员ID: {eto.OrderId} ，赠送会员积分金额: {eto.TotalAmount:C}";

        //模拟耗时  耗时任务  发布到后台作业hangfire
        //await Task.Delay(1000);

        _logger.LogInformation(message);
    }
}