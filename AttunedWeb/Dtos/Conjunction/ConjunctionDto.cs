using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Rules;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Conjunction;

[TypescriptDto]
public record ConjunctionDto
{
    public required ConjunctionTypeDto Type { get; init; }
    public required IEnumerable<ConjunctionDto> SubConjunctions { get; init; }

    [TsProperty(Type = $"({RuleConverters.TYPE_SCRIPT_UNION_TYPE})[]")]
    public required IEnumerable<IRuleDto> Rules { get; init; }
};