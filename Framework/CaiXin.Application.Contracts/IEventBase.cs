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
    public interface IEventBase
    {
        /// <summary>
        /// 租户id
        /// </summary>
        Guid? TenantId { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        string TenantName { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        Guid? UserId { get; set; }

        /// <summary>
        /// 事件名
        /// </summary>
        string EventName { get; set; }
    }
}