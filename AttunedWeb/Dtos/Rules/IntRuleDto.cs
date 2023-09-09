using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record IntRuleDto(IntFieldsDto Field, OperatorDto RuleType, SignDto Sign, long ValueA, long? ValueB) : IRuleDto;