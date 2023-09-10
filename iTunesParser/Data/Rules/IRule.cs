using iTunesSmartParser.Data.Logic;

namespace iTunesSmartParser.Data.Rules;

public interface IRule
{
    public Operator Operator { get; }

    public Sign Sign { get; }
}