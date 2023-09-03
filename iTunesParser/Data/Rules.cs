using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Data;

public record StringRule(StringFields Field, LogicRule RuleType, LogicSign Sign, string Value) : IRule;

public record IntRule(IntFields Field, LogicRule RuleType, LogicSign Sign, long ValueA, long? ValueB) : IRule;


public record DateRule(DateFields Field, LogicRule RuleType, LogicSign Sign, DateTime ValueA,
    DateTime? ValueB) : IRule;

public record TimeSpanRule(DateFields Field, LogicRule RuleType, LogicSign Sign, TimeSpan Value) : IRule;

public record BooleanRule(BoolFields Field, LogicRule RuleType, LogicSign Sign) : IRule;

public record DictionaryRule(DictionaryFields Field, LogicRule RuleType, LogicSign Sign, string Value) : IRule;

public record PlaylistRule(PlaylistFields Field, LogicRule RuleType, LogicSign Sign, string Value) : IRule;