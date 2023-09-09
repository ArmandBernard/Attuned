using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record DictionaryRuleDto(DictionaryFieldsDto Field, OperatorDto RuleType, SignDto Sign, string Value) : IRuleDto;