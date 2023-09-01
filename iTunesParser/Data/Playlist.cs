namespace iTunesSmartParser;

public record Playlist(string Name, int Id, bool IsSmart, IEnumerable<int> Items, string? Filter);
