using iTunesSmartParser.Data.Limits;

namespace iTunesSmartParser.Data;

public record SmartPlaylistInformation(Limit Limit, Conjunction? RuleConjunction, bool LiveUpdating);