namespace Eqn.Auditing.Auditing;

public interface IAuditLogSaveHandle : IDisposable
{
    Task SaveAsync();
}
