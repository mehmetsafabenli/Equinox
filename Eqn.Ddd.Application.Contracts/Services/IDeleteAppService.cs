namespace Eqn.Ddd.Application.Contracts.Services;

public interface IDeleteAppService<in TKey> : IApplicationService
{
    Task DeleteAsync(TKey id);
}
