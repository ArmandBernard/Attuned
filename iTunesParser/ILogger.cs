namespace iTunesSmartParser;

public interface ILogger
{
    public void Log(string text);

    public void Log(Exception ex);
}