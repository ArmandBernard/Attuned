using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record TimeSpanRule(DateFields Field, Operator Operator, Sign Sign, TimeSpan Value) : IRule;