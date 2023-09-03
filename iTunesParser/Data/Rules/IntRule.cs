using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record IntRule(IntFields Field, Operator RuleType, Sign Sign, long ValueA, long? ValueB) : IRule;