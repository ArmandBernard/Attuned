using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record DateRule(DateFields Field, Operator RuleType, Sign Sign, DateTime ValueA,
    DateTime? ValueB) : IRule;