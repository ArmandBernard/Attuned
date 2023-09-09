using iTunesSmartParser.Data.Logic;

namespace AttunedWebApi.Dtos.Logic;

public static class LogicConverter
{
    public static SignDto ToDto(this Sign sign)
    {
        return sign switch
        {
            Sign.IntPositive => SignDto.Positive,
            Sign.StringPositive => SignDto.Positive,
            Sign.IntNegative => SignDto.Negative,
            Sign.StringNegative => SignDto.Negative,
            _ => throw new ArgumentOutOfRangeException(nameof(sign), sign, null)
        };
    }

    public static OperatorDto ToDto(this Operator operatorEnum)
    {
        return operatorEnum switch
        {
            Operator.Other => OperatorDto.Between,
            Operator.Is => OperatorDto.Is,
            Operator.Contains => OperatorDto.Contains,
            Operator.Starts => OperatorDto.Starts,
            Operator.Ends => OperatorDto.Ends,
            Operator.Greater => OperatorDto.Greater,
            Operator.Less => OperatorDto.Less,
            _ => throw new ArgumentOutOfRangeException(nameof(operatorEnum), operatorEnum, null)
        };
    }
}