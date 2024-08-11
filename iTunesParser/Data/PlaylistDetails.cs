namespace iTunesSmartParser.Data;

public record PlaylistDetails(string Name, int Id, bool IsSmart, IEnumerable<int> Items, SmartPlaylistInformation? Filters);