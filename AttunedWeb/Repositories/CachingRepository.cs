namespace AttunedWebApi.Repositories;

public class CachingRepository<T>(IRepository<T> unCachedRepository, TimeSpan staleTime) : IRepository<T>
{
    private Cache<IEnumerable<T>>? CachedGet { get; set; }

    public async Task<IEnumerable<T>> Get()
    {
        if (CachedGet != null && CachedGet.Expiry > DateTime.UtcNow)
        {
            return CachedGet.Value;
        }

        var value = (await unCachedRepository.Get()).ToList();

        CachedGet = new Cache<IEnumerable<T>>(value, DateTime.UtcNow.Add(staleTime));

        return value;
    }
}