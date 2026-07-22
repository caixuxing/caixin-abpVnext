using CaiXin.Domain.Shared.Response;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace CaiXin.NiuMa.Domain.Employees.Validations
{
    public class CreateEmployeeValidator : AbstractValidator<EmployeeAgg>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.EmployeeNumber)
                .NotEmpty().WithMessage("工号不能为空")
                .MaximumLength(50).WithMessage("工号长度不能超过50个字符");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("姓名不能为空")
                .MaximumLength(100).WithMessage("姓名长度不能超过100个字符");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("邮箱格式不正确")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^1[3-9]\d{9}$").WithMessage("手机号格式不正确")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.HireDate.Date)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("入职日期不能晚于今天");

            //RuleFor(x => x.Status)
            //    .IsInEnum().WithMessage("员工状态无效");
        }
    }

    public static class EmployeeValidationExtensions
    {
        private static readonly IValidator<EmployeeAgg> Validator = new CreateEmployeeValidator();

        public static void Validate(this EmployeeAgg employee)
        {
            var result = Validator.Validate(employee);
            if (!result.IsValid)
            {
                // 构建详细的验证错误信息
                var validationErrors = result.Errors.Select(e => new ValidationErrorDetail
                {
                    Property = e.PropertyName,
                    Message = e.ErrorMessage,
                    ErrorCode = e.ErrorCode ?? "ValidationError",
                    AttemptedValue = e.AttemptedValue,
                    Severity = e.Severity.ToString()
                }).ToList();

                // ✅ 使用 System.ComponentModel.DataAnnotations.ValidationResult
                var validationResults = result.Errors.Select(e =>
                    new ValidationResult(e.ErrorMessage, new[] { e.PropertyName })
                ).ToList();

                // 创建 AbpValidationException
                var exception = new AbpValidationException(
                    message: "Employee validation failed",
                    validationErrors: validationResults
                );

                // 将详细错误信息附加到 Data（供过滤器使用）
                exception.Data["ValidationErrors"] = validationErrors;
                exception.Data["ErrorCount"] = validationErrors.Count;

                throw exception;
            }
        }
    }
}