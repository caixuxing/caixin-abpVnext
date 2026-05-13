using CaiXin.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.EventBus
{
    /// <summary>
    /// 分布式EventBus
    /// </summary>
    public interface ICaiXinDistributedEventBus
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="onUnitOfWorkComplete"></param>
        /// <param name="useOutbox"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class, IEventBase;
    }
}