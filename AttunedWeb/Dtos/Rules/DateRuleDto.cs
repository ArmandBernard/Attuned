using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record DateRuleDto(DateFieldsDto Field, OperatorDto RuleType, SignDto Sign, DateTime ValueA,
    DateTime? ValueB) : IRuleDto;