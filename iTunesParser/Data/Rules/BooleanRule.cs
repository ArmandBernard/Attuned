using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record BooleanRule(BoolFields Field, Operator RuleType, Sign Sign) : IRule;