using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record BooleanRuleDto(BoolFieldsDto Field, OperatorDto Operator, SignDto Sign) : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Boolean)}\"")]
    public RuleType RuleType => RuleType.Boolean;
}