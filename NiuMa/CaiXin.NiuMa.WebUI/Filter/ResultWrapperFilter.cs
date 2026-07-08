using CaiXin.Domain.Shared.Attributes;
using CaiXin.Domain.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;

namespace CaiXin.NiuMa.WebUI.Filter;

public class ResultWrapperFilter : IAsyncActionFilter, ITransientDependency
{
    private readonly ILogger<ResultWrapperFilter> _logger;

    public ResultWrapperFilter(ILogger<ResultWrapperFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionDescriptor = context.ActionDescriptor;

        // 检查是否需要包装结果（使用相同的 WrapResultAttribute 判断）
        var shouldWrap = ShouldWrapResult(context);

        if (!shouldWrap)
        {
            await next();
            return;
        }

        var resultContext = await next();

        // 如果已经处理了异常或已经有结果，跳过
        //if (resultContext.Exception != null || resultContext.Result != null)
        //{
        //    return;
        //}

        // 正常返回时包装结果
        if (resultContext.Result is ObjectResult objectResult)
        {
            var traceIdentifier = context.HttpContext.TraceIdentifier;
            var originalData = objectResult.Value;

            var wrappedResult = new WrapResult<object>();

            // 判断是否已经是 WrapResult 类型，避免双重包装
            if (originalData is WrapResult<object> wrapResult)
            {
                // 如果已经是 WrapResult，直接使用，但补充 TraceId
                resultContext.Result = new ObjectResult(originalData);
                return;
            }

            // 如果是 ActionResult 或基本类型，包装为成功结果
            wrappedResult.SetSuccess(originalData, "Success", 200);

            // ✅ 可以在这里添加 TraceId 等额外信息
            // 注意：如果 WrapResult 没有 TraceId 字段，需要添加

            resultContext.Result = new ObjectResult(wrappedResult)
            {
                StatusCode = 200
            };
        }
    }

    private bool ShouldWrapResult(ActionExecutingContext context)
    {
        var actionDescriptor = context.ActionDescriptor;

        // 检查 Controller 上的 WrapResultAttribute
        if (actionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
        {
            var controllerType = controllerActionDescriptor.ControllerTypeInfo;
            if (controllerType.GetCustomAttributes(typeof(WrapResultAttribute), true).Any())
            {
                return true;
            }

            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(WrapResultAttribute), true).Any())
            {
                return true;
            }
        }

        return false;
    }
}