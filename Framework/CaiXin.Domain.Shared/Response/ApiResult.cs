namespace CaiXin.NiuMa.Domain.Shared.Response
{
    public partial class ApiResult<TResultData>
    {
        /// <summary>
        /// 结果码(为0则表示请求成功)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 结果数据
        /// </summary>
        public TResultData Data { get; set; } = default!;

    }

    public partial class ApiResult
    {
        /// <summary>
        /// 结果码(为0则表示请求成功)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    public partial class ApiResult
    {
        public static ApiResult Success(string message = "成功")
        {
            return new ApiResult
            {
                Code = 200,
                Message = message,
            };
        }
        public static ApiResult Failure(string message = "失败")
        {
            return new ApiResult
            {
                Code = 400,
                Message = message
            };
        }
    }


    public partial class ApiResult<TResultData>
    {
        // 成功响应
        public static ApiResult<TResultData> Success(TResultData data, string message = "成功")
        {
            return new ApiResult<TResultData>
            {
                Code = 200,
                Message = message,
                Data = data
            };
        }

        public static ApiResult<TResultData> Failure(TResultData data, string message = "失败")
        {
            return new ApiResult<TResultData>
            {
                Code = 400,
                Message = message,
                Data = data
            };
        }
    }
}