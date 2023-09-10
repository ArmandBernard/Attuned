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
[JsonDerivedType(typeof(RatingRuleDto))]
public interface IRuleDto
{
    public RuleType RuleType { get; }
    
    public OperatorDto Operator { get; }

    public SignDto Sign { get; }
}