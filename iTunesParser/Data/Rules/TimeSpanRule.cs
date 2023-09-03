using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record TimeSpanRule(DateFields Field, Operator RuleType, Sign Sign, TimeSpan Value) : IRule;