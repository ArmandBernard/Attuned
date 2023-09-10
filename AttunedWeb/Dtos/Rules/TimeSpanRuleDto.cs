using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record TimeSpanRuleDto(DateFieldsDto Field, OperatorDto Operator, SignDto Sign, TimeSpan Value) : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.TimeSpan)}\"")]
    public RuleType RuleType => RuleType.TimeSpan;
}