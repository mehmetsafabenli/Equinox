using Castle.DynamicProxy;
using Eqn.Core.DynamicProxy;

namespace Eqn.Castle.Core.DynamicProxy;

public class EqnAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
    where TInterceptor : IEqnInterceptor
{
    public EqnAsyncDeterminationInterceptor(TInterceptor EqnInterceptor)
        : base(new CastleAsyncEqnInterceptorAdapter<TInterceptor>(EqnInterceptor))
    {

    }
}
