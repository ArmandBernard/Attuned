using AttunedWebApi.Dtos.Logic;

namespace AttunedWebApi.Dtos.Rules;

public interface IRuleDto
{
    public OperatorDto RuleType { get; }

    public SignDto Sign { get; }
}