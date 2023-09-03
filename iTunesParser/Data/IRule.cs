namespace iTunesSmartParser.Data;

public interface IRule
{
    public LogicRule RuleType { get; }

    public LogicSign Sign { get; }
}