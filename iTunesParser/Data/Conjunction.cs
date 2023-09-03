using iTunesSmartParser.Data.Rules;

namespace iTunesSmartParser.Data;

public record Conjunction(ConjunctionType Type, IList<Conjunction> SubConjunctions, IList<IRule> Rules);