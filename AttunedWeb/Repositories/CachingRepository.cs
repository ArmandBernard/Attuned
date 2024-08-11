namespace AttunedWebApi.Repositories;

public class CachingRepository<TBasic, TDetailed>(IRepository<TBasic, TDetailed> unCachedRepository, TimeSpan staleTime)
    : IRepository<TBasic, TDetailed>
{
    private Cache<IEnumerable<TBasic>>? CachedGet { get; set; }

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

    // don't cache individual fetches, as there could be many
    public Task<TDetailed?> GetById(int id, CancellationToken token) => unCachedRepository.GetById(id, token);
}