namespace AttunedWebApi.Repositories;

public interface IRepository<TBasic,TDetailed>
{
    Task<IEnumerable<TBasic>> Get(CancellationToken token);

    Task<TBasic?> GetById(int id, CancellationToken token);

    Task<TDetailed?> GetDetailsById(int id, CancellationToken token);
}