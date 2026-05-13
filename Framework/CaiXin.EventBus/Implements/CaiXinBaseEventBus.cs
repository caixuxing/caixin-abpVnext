using CaiXin.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace CaiXin.EventBus.Implements
{
    public class CaiXinBaseEventBus(ICurrentTenant CurrentTenant) : IUnitOfWorkEnabled, ITransientDependency
    {
        /// <summary>
        /// 发布消息前设置租户和用户信息
        /// </summary>
        /// <param name="eventData"></param>
        /// <typeparam name="TEvent"></typeparam>
        public void SetBasicInfo<TEvent>(TEvent eventData)
            where TEvent : class, IEventBase
        {
            eventData.TenantId = CurrentTenant.Id;
            eventData.TenantName = CurrentTenant.Name;
            eventData.UserId = Guid.NewGuid();
        }
    }
}