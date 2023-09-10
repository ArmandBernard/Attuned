using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record DateRuleDto(DateFieldsDto Field, OperatorDto Operator, SignDto Sign, DateTime ValueA,
        DateTime? ValueB)
    : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Date)}\"")]
    public RuleType RuleType => RuleType.Date;
}