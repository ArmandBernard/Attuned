using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data.Rules;

public record IntRule(IntFields Field, Operator Operator, Sign Sign, long ValueA, long? ValueB) : IRule;