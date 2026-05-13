using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.Application.Contracts
{
    /// <summary>
    /// 事件基础模型
    /// </summary>
    public class EventBaseDto : IEventBase
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 相关ID
        /// </summary>
        public string CorrelationId { get; set; }
    }
}