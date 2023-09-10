using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.Rules;

[TypescriptDto]
public record PlaylistRuleDto(PlaylistFieldsDto FieldDto, OperatorDto Operator, SignDto Sign, string Value) : IRuleDto
{
    [TsProperty(Type = $"\"{nameof(RuleType.Playlist)}\"")]
    public RuleType RuleType => RuleType.Playlist;    
}