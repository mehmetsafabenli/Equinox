using Eqn.Core.System;
using Eqn.DistributedLocking.Abstraction.DistributedLocking;
using Medallion.Threading;

namespace Eqn.DistributedLocking.DistributedLocking;

public static class EqnDistributedLockHandleExtensions
{
    public static IDistributedSynchronizationHandle ToDistributedSynchronizationHandle(
        this IEqnDistributedLockHandle handle)
    {
        return handle.As<MedallionEqnDistributedLockHandle>().Handle;
    }
}
