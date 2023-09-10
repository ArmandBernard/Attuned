using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record StringRuleDto(StringFieldsDto Field, OperatorDto Operator, SignDto Sign, string ValueA) : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.String)}\"")]
    public RuleType RuleType => RuleType.String;
}