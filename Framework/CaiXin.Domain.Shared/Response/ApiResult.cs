using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.NiuMa.Domain.Shared.Response
{
    public class ApiResult<TResultData>
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

    public class ApiResult
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
}