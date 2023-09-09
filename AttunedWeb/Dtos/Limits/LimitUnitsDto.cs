using AttunedWebApi.CodeGen;

namespace AttunedWebApi.Dtos.Limits;

[TypescriptEnum]
public enum LimitUnitsDto
{
    Minutes,
    MB,
    Items,
    Hours,
    GB
}