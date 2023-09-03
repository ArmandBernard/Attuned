using iTunesSmartParser.Data.Logic;

namespace iTunesSmartParser.Data.Rules;

public interface IRule
{
    public Operator RuleType { get; }

    public Sign Sign { get; }
}