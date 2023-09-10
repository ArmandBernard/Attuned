using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record BooleanRuleDto(BoolFieldsDto Field, OperatorDto RuleType, SignDto Sign) : IRuleDto;