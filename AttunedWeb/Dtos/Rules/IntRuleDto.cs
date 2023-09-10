using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record IntRuleDto(IntFieldsDto Field, OperatorDto Operator, SignDto Sign, long ValueA, long? ValueB) : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Int)}\"")]
    public RuleType RuleType => RuleType.Int;
};