namespace AttunedWebApi.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> Get();
}