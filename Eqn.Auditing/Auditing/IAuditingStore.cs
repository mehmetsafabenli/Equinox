namespace Eqn.Auditing.Auditing;

public interface IAuditingStore
{
    Task SaveAsync(AuditLogInfo auditInfo);
}
