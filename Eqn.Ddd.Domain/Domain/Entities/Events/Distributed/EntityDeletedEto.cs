﻿using Eqn.EventBus;
using Eqn.EventBus.Abstraction.EventBus;
using Eqn.MultiTenancy.MultiTenancy;

namespace Eqn.Ddd.Domain.Domain.Entities.Events.Distributed;

[Serializable]
[GenericEventName(Postfix = ".Deleted")]
public class EntityDeletedEto<TEntityEto> : IEventDataMayHaveTenantId
{
    public TEntityEto Entity { get; set; }

    public EntityDeletedEto(TEntityEto entity)
    {
        Entity = entity;
    }

    public virtual bool IsMultiTenant(out Guid? tenantId)
    {
        if (Entity is IMultiTenant multiTenantEntity)
        {
            tenantId = multiTenantEntity.TenantId;
            return true;
        }

        tenantId = null;
        return false;
    }
}
