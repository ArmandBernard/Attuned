namespace AttunedWebApi.Repositories;

public interface IRepository<TBasic,TDetailed>
{
    Task<IEnumerable<TBasic>> Get(CancellationToken token);

    Task<TDetailed?> GetById(int id, CancellationToken token);
}