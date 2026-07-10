using CaiXin.Domain.Shared.Attributes;
using CaiXin.Domain.Shared.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Validation;

namespace CaiXin.NiuMa.Host.Filter
{
    public class ResultExceptionFilter : IFilterMetadata, IAsyncExceptionFilter, ITransientDependency
    {
        private ILogger<ResultExceptionFilter> Logger { get; set; }
        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly AbpExceptionHandlingOptions _exceptionHandlingOptions;

        public ResultExceptionFilter(
            IExceptionToErrorInfoConverter errorInfoConverter,
            IHttpExceptionStatusCodeFinder statusCodeFinder,
            IJsonSerializer jsonSerializer,
            IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
        {
            _errorInfoConverter = errorInfoConverter;
            _statusCodeFinder = statusCodeFinder;
            _jsonSerializer = jsonSerializer;
            _exceptionHandlingOptions = exceptionHandlingOptions.Value;
            Logger = NullLogger<ResultExceptionFilter>.Instance;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }
            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            if (context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.GetCustomAttributes(typeof(WrapResultAttribute), true).Any())
            {
                return true;
            }
            if (context.ActionDescriptor.GetMethodInfo().GetCustomAttributes(typeof(WrapResultAttribute), true).Any())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            // 处理异常信息
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            var statusCode = (int)_statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);
            context.HttpContext.Response.StatusCode = 200;

            var remoteServiceErrorInfo = _errorInfoConverter.Convert(context.Exception, _exceptionHandlingOptions.SendExceptionsDetailsToClients);
            remoteServiceErrorInfo.Code = context.HttpContext.TraceIdentifier;

            // 返回格式统一
            var result = new WrapResult<object>
            {
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            // 根据不同异常类型处理
            HandleExceptionByType(context, result, statusCode);

            // HttpResponse
            context.Result = new ObjectResult(result);

            // 写日志
            var logLevel = context.Exception.GetLogLevel();
            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(_jsonSerializer.Serialize(remoteServiceErrorInfo, indented: true));
            Logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());
            Logger.LogException(context.Exception, logLevel);

            await context.HttpContext
                .RequestServices
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(
                    new ExceptionNotificationContext(context.Exception)
                );

            context.Exception = null; //Handled!
        }

        protected virtual void HandleExceptionByType(ExceptionContext context, WrapResult<object> result, int statusCode)
        {
            var exception = context.Exception;

            switch (exception)
            {
                case AbpValidationException validationEx:
                    HandleValidationException(result, validationEx, statusCode);
                    break;

                case BusinessException businessEx:
                    HandleBusinessException(result, businessEx, statusCode);
                    break;

                case AbpAuthorizationException _:
                    result.SetFail("Authenticate failure！", 401, context.HttpContext.TraceIdentifier);
                    break;

                case EntityNotFoundException _:
                    result.SetFail("Entity not found！", 404, context.HttpContext.TraceIdentifier);
                    break;

                case NotImplementedException _:
                    result.SetFail("Feature not implemented！", 501, context.HttpContext.TraceIdentifier);
                    break;

                default:
                    result.SetFail("Server internal error！", statusCode, context.HttpContext.TraceIdentifier);
                    break;
            }
        }

        protected virtual void HandleValidationException(WrapResult<object> result, AbpValidationException exception, int statusCode)
        {
            // 提取验证错误详情（只使用 ValidationResultInfo 中存在的属性）
            var validationErrors = new List<ValidationErrorDetail>();

            foreach (var error in exception.ValidationErrors)
            {
                var detail = new ValidationErrorDetail
                {
                    Property = error.MemberNames?.FirstOrDefault() ?? "General",
                    Message = error.ErrorMessage
                };
                validationErrors.Add(detail);
            }

            // 检查是否有额外的 Data 信息（从 FluentValidation 传递过来的）
            if (exception.Data.Contains("ValidationErrors"))
            {
                var extraErrors = exception.Data["ValidationErrors"] as List<ValidationErrorDetail>;
                if (extraErrors != null && extraErrors.Any())
                {
                    validationErrors = extraErrors;
                }
            }

            var errorResponse = new ValidationErrorResponse
            {
                Errors = validationErrors,
                ErrorCount = validationErrors.Count,
                Summary = "Validation failed for one or more fields"
            };

            result.SetFailWithData(
                data: errorResponse,
                message: "Request param validate failure！",
                code: statusCode > 0 ? statusCode : 400,
                traceId: result.TraceId
            );
        }

        protected virtual void HandleBusinessException(WrapResult<object> result, BusinessException exception, int statusCode)
        {
            // 检查业务异常是否携带额外数据
            if (exception.Data.Count > 0)
            {
                var dataDict = new Dictionary<string, object>();
                foreach (var key in exception.Data.Keys)
                {
                    dataDict[key.ToString()] = exception.Data[key];
                }

                result.SetFailWithData(
                    data: dataDict,
                    message: exception.Message ?? "Business error",
                    code: statusCode > 0 ? statusCode : 500,
                    traceId: result.TraceId
                );
            }
            else
            {
                result.SetFail(
                    message: exception.Message ?? "Business error",
                    code: statusCode > 0 ? statusCode : 500,
                    traceId: result.TraceId
                );
            }
        }
    }
}