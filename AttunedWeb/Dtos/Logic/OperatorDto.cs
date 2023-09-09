using AttunedWebApi.CodeGen;

namespace AttunedWebApi.Dtos.Logic;

[TypescriptEnum]
public enum OperatorDto
{
    Is,
    Contains,
    Starts,
    Ends,
    Greater,
    Less,
    Between
}
