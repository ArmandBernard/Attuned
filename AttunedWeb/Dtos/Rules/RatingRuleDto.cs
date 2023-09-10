using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record RatingRuleDto()
    : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Rating)}\"")]
    public RuleType RuleType => RuleType.Rating;

    [TsProperty(Type = "\"Rating\"")] public string Field => "Rating";

    public required OperatorDto Operator { get; init; }
    public required SignDto Sign { get; init; }

    [TsProperty(Type = "Rating")] public required long ValueA { get; init; }
    

    [TsProperty(Type = "Rating | undefined")]
    public required long? ValueB { get; init; }
}