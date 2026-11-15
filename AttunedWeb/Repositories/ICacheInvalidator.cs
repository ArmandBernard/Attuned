namespace AttunedWebApi.Repositories;

public interface ICacheInvalidator
{
    /// <summary>
    /// Drop anything cached so the next read goes to the source
    /// </summary>
    void Invalidate();
}
