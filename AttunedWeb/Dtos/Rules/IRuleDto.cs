using System.Text.Json.Serialization;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[JsonDerivedType(typeof(BooleanRuleDto))]
[JsonDerivedType(typeof(DateRuleDto))]
[JsonDerivedType(typeof(DictionaryRuleDto))]
[JsonDerivedType(typeof(IntRuleDto))]
[JsonDerivedType(typeof(PlaylistRuleDto))]
[JsonDerivedType(typeof(StringRuleDto))]
[JsonDerivedType(typeof(TimeSpanRuleDto))]
public interface IRuleDto
{
    public OperatorDto RuleType { get; }

    public SignDto Sign { get; }
}