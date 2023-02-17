using JetBrains.Annotations;

namespace Eqn.Core.ExceptionHandling;

public interface IExceptionNotifier
{
    Task NotifyAsync([NotNull] ExceptionNotificationContext context);
}
