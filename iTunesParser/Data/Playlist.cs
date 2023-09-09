namespace iTunesSmartParser.Data;

public record Playlist(string Name, int Id, IEnumerable<int> Items, bool IsSmart, SmartPlaylistInformation? Filters);