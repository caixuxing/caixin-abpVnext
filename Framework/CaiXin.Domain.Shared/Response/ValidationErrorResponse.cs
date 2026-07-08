using System;
using System.Collections.Generic;

namespace CaiXin.Domain.Shared.Response
{
    public class ValidationErrorResponse
    {
        public List<ValidationErrorDetail> Errors { get; set; }
        public int ErrorCount { get; set; }
        public string Summary { get; set; }

        public ValidationErrorResponse()
        {
            Errors = new List<ValidationErrorDetail>();
            Summary = "Validation failed for one or more fields";
        }
    }

    public class ValidationErrorDetail
    {
        public string Property { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public object AttemptedValue { get; set; }
        public string Severity { get; set; }

        public ValidationErrorDetail()
        {
            Property = "General";
            Message = "Validation error";
            ErrorCode = "ValidationError";
        }
    }
}