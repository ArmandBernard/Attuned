using AttunedWebApi.Dtos.Rules;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Dtos.Conjunction;

public static class ConjunctionConverter
{
    public static ConjunctionDto ToDto(this iTunesSmartParser.Data.Conjunction conjunction)
    {
        return new ConjunctionDto
        {
            Type = conjunction.Type.ToDto(),
            SubConjunctions = conjunction.SubConjunctions.Select(x => x.ToDto()),
            Rules = conjunction.Rules.Select(x => x.ToDto())
        };
    }

    private static ConjunctionTypeDto ToDto(this ConjunctionType conjunctionType) => conjunctionType switch
    {
        ConjunctionType.And => ConjunctionTypeDto.And,
        ConjunctionType.Or => ConjunctionTypeDto.Or,
        _ => throw new ArgumentOutOfRangeException(nameof(conjunctionType), conjunctionType, null)
    };
}