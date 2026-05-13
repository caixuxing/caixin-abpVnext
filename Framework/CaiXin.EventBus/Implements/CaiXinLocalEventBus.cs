using CaiXin.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace CaiXin.EventBus.Implements
{
    /// <summary>
    /// 本地内存事件总线实现
    /// </summary>
    public class CaiXinLocalEventBus(ILocalEventBus localEventBus, ICurrentTenant CurrentTenant) : CaiXinBaseEventBus(CurrentTenant), ICaiXinLocalEventBus
    {
        public async Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = false) where TEvent : class, IEventBase
        {
            base.SetBasicInfo(eventData);
            await localEventBus.PublishAsync(eventData, onUnitOfWorkComplete);
        }
    }
}