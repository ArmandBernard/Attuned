using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record PlaylistRule(PlaylistFields Field, Operator RuleType, Sign Sign, string Value) : IRule;