using CaiXin.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.EventBus
{
    internal interface ICaiXinLocalEventBus
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="eventData">发布的数据</param>
        /// <param name="onUnitOfWorkComplete">是否等待当前单位工作完成</param>
        /// <typeparam name="TEvent">泛型 继承ITenantBase</typeparam>
        /// <returns></returns>
        Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = false) where TEvent : class, IEventBase;
    }
}