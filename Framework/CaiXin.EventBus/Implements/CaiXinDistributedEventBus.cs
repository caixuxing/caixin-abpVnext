using CaiXin.Application.Contracts;
using DotNetCore.CAP;
using Volo.Abp.MultiTenancy;

namespace CaiXin.EventBus.Implements
{
    /// <summary>
    /// 分布式事务总线实现
    /// </summary>
    public class CaiXinDistributedEventBus(ICapPublisher distributedEventBus, ICurrentTenant CurrentTenant) : CaiXinBaseEventBus(CurrentTenant), ICaiXinDistributedEventBus
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class, IEventBase
        {
            if (eventData is IEventBase eventObject)
            {
                if (string.IsNullOrWhiteSpace(eventObject.EventName)) throw new ArgumentNullException(nameof(eventObject.EventName), "消息的事件名称不能为空！");

                base.SetBasicInfo(eventData);
                await distributedEventBus.PublishAsync(eventObject.EventName, eventData);
            }
        }
    }
}