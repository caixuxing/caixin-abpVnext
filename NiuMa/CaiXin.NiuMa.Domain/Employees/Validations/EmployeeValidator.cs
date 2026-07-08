using FluentValidation;

namespace CaiXin.NiuMa.Domain.Employees.Validations
{
    public class CreateEmployeeValidator : AbstractValidator<Employee>
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

            RuleFor(x => x.HireDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("入职日期不能晚于今天");

            //RuleFor(x => x.Status)
            //    .IsInEnum().WithMessage("员工状态无效");
        }
    }



    public static class EmployeeValidationExtensions
    {
        private static readonly IValidator<Employee> Validator = new CreateEmployeeValidator();

        public static void Validate(this Employee employee)
        {
            var result = Validator.Validate(employee);
            if (!result.IsValid) throw new Exception(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
        }

        public static bool IsValid(this Employee employee)
        {
            return Validator.Validate(employee).IsValid;
        }

        public static IReadOnlyList<string> GetValidationErrors(this Employee employee)
        {
            var result = Validator.Validate(employee);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}
