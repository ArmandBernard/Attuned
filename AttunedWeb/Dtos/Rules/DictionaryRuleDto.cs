using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record DictionaryRuleDto(DictionaryFieldsDto Field, OperatorDto Operator, SignDto Sign, string Value)
    : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Dictionary)}\"")]
    public RuleType RuleType => RuleType.Dictionary;
}