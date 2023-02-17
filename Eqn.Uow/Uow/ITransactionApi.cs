namespace Eqn.Uow.Uow;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync();
}
