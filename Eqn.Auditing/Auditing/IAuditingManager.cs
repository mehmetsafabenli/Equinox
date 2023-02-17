using JetBrains.Annotations;

namespace Eqn.Auditing.Auditing;

public interface IAuditingManager
{
    [CanBeNull]
    IAuditLogScope Current { get; }

    IAuditLogSaveHandle BeginScope();
}
