namespace AttunedWebApi.Repositories;

public record Cache<T>(T Value, DateTime Expiry);