using CaiXin.Domain.Shared.Response;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace CaiXin.NiuMa.Domain.Shared.Validations;

public static class EntityValidationHelper
{
    /// <summary>
    /// 验证实体，失败时抛出 AbpValidationException
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="entity">待验证的实体实例</param>
    /// <param name="validator">对应的验证器</param>
    /// <exception cref="AbpValidationException">验证失败时抛出</exception>
    public static void Validate<T>(this T entity, IValidator<T> validator)
    {
        var result = validator.Validate(entity);
        if (!result.IsValid)
        {
            // 1. 转换为 System.ComponentModel.DataAnnotations.ValidationResult 列表
            var validationResults = result.Errors
                .Select(e => new ValidationResult(e.ErrorMessage, new[] { e.PropertyName }))
                .ToList();

            // 2. 构造 AbpValidationException
            var exception = new AbpValidationException(
                message: $"Validation failed for {typeof(T).Name}",
                validationErrors: validationResults
            );

            // 3.（可选）附加更多调试信息到 Data
            exception.Data["EntityType"] = typeof(T).FullName;
            exception.Data["ValidationErrors"] = result.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage, e.AttemptedValue })
                .ToList();

            throw exception;
        }
    }
}