using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record BooleanRule(BoolFields Field, Operator Operator, Sign Sign) : IRule;