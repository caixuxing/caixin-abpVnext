using CaiXin.NiuMa.Domain.Employees.EventDto;

namespace CaiXin.NiuMa.Application.EmployeeApp.EventHandler;

/// <summary>
/// 本地事件处理器 Demo，用于处理员工创建事件
/// 员工创建事件处理器
/// </summary>
public class EmployeeCreatedEventHandler(ILogger<EmployeeCreatedEventHandler> _logger)
    : ILocalEventHandler<CreateEmployeeEto>, ITransientDependency
{
    public async Task HandleEventAsync(CreateEmployeeEto eventData)
    {
        _logger.LogInformation($"开始处理员工创建事件：员工ID={eventData.Id}, 姓名={eventData.FullName}");

        try
        {
            // 1. 发送欢迎邮件
            // await _emailService.SendWelcomeEmailAsync(eventData.Email, eventData.FullName);

            // 2. 分配默认角色
            // await _userRoleService.AssignDefaultRolesAsync(eventData.Id);

            // 3. 发送系统通知
            // await _notificationService.NotifyHRNewEmployeeAsync(eventData);

            // 4. 初始化员工权限
            // await _permissionService.InitializeEmployeePermissionsAsync(eventData.Id);

            // 5. 记录审计日志
            _logger.LogInformation($"员工 {eventData.FullName}({eventData.EmployeeNumber}) 创建成功");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理员工创建事件失败：员工ID={eventData.Id}");
            throw;
        }
    }
}