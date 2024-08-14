namespace AttunedWebApi.Repositories;

public class CachingRepository<TBasic, TDetailed>(IRepository<TBasic, TDetailed> unCachedRepository, TimeSpan staleTime)
    : IRepository<TBasic, TDetailed>
{
    private Cache<IEnumerable<TBasic>>? CachedGet { get; set; }

    private Dictionary<int, Cache<TDetailed>> CachedGetByIdDictionary { get; set; } = new();

    public async Task<IEnumerable<TBasic>> Get(CancellationToken token)
    {
        if (CachedGet != null && CachedGet.Expiry > DateTime.UtcNow)
        {
            return CachedGet.Value;
        }

        var value = (await unCachedRepository.Get(token)).ToList();

        CachedGet = new Cache<IEnumerable<TBasic>>(value, DateTime.UtcNow.Add(staleTime));

        return value;
    }

    public async Task<TDetailed?> GetById(int id, CancellationToken token)
    {
        lock (CachedGetByIdDictionary)
        {
            if (CachedGetByIdDictionary.TryGetValue(id, out var cache) && cache.Expiry > DateTime.UtcNow)
            {
                return cache.Value;
            }

            if (cache != null && cache.Expiry < DateTime.UtcNow)
            {
                CachedGetByIdDictionary.Remove(id);
            }
        }

        var newValue = await unCachedRepository.GetById(id, token);

        lock (CachedGetByIdDictionary)
        {
            if (newValue != null && !CachedGetByIdDictionary.ContainsKey(id))
            {
                CachedGetByIdDictionary.Add(id, new Cache<TDetailed>(newValue, DateTime.UtcNow.Add(staleTime)));
            }
        }

        return newValue;
    }
}