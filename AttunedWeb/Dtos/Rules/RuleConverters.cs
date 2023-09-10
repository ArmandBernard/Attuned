using AttunedWebApi.Dtos.Fields;
using AttunedWebApi.Dtos.Logic;
using iTunesSmartParser.Data.Rules;

namespace AttunedWebApi.Dtos.Rules;

public static class RuleConverters
{
    public const string TYPE_SCRIPT_UNION_TYPE =
        "BooleanRuleDto | DateRuleDto | DictionaryRuleDto | IntRuleDto | PlaylistRuleDto | StringRuleDto | TimeSpanRuleDto";

    public static IRuleDto ToDto(this IRule iRule)
    {
        return iRule switch
        {
            BooleanRule rule => new BooleanRuleDto(rule.Field.ToDto(), rule.Operator.ToDto(),
                rule.Sign.ToDto()),
            DateRule rule => new DateRuleDto(rule.Field.ToDto(), rule.Operator.ToDto(),
                rule.Sign.ToDto(), rule.ValueA, rule.ValueB),
            DictionaryRule rule => new DictionaryRuleDto(rule.Field.ToDto(),
                rule.Operator.ToDto(), rule.Sign.ToDto(), rule.Value),
            IntRule rule => new IntRuleDto(rule.Field.ToDto(), rule.Operator.ToDto(),
                rule.Sign.ToDto(), rule.ValueA, rule.ValueB),
            PlaylistRule rule => new PlaylistRuleDto(PlaylistFieldsDto.PlaylistPersistentID, rule.Operator.ToDto(),
                rule.Sign.ToDto(), rule.Value),
            StringRule rule => new StringRuleDto(rule.Field.ToDto(),
                rule.Operator.ToDto(), rule.Sign.ToDto(), rule.Value),
            TimeSpanRule rule => new TimeSpanRuleDto(rule.Field.ToDto(),
                rule.Operator.ToDto(), rule.Sign.ToDto(), rule.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(iRule))
        };
    }
}