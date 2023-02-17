using Eqn.Core.Aspects;
using Eqn.Core.DependencyInjection;
using Eqn.Core.DynamicProxy;

namespace Eqn.GlobalFeatures.GlobalFeatures;

public class GlobalFeatureInterceptor : EqnInterceptor, ITransientDependency
{
    public override async Task InterceptAsync(IEqnMethodInvocation invocation)
    {
        if (EqnCrossCuttingConcerns.IsApplied(invocation.TargetObject, EqnCrossCuttingConcerns.GlobalFeatureChecking))
        {
            await invocation.ProceedAsync();
            return;
        }

        if (!GlobalFeatureHelper.IsGlobalFeatureEnabled(invocation.TargetObject.GetType(), out var attribute))
        {
            throw new EqnGlobalFeatureNotEnabledException(code: EqnGlobalFeatureErrorCodes.GlobalFeatureIsNotEnabled)
                .WithData("ServiceName", invocation.TargetObject.GetType().FullName)
                .WithData("GlobalFeatureName", attribute.Name);
        }

        await invocation.ProceedAsync();
    }
}
