using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record TimeSpanRuleDto(DateFieldsDto Field, OperatorDto RuleType, SignDto Sign, TimeSpan Value) : IRuleDto;