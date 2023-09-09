using AttunedWebApi.CodeGen;

namespace AttunedWebApi.Dtos.Limits;

[TypescriptEnum]
public enum SortFieldDto
{
    Random,
    Name,
    Album,
    Artist,
    Genre,
    Rating,
    LastPlayed,
    PlayCount,
    DateAdded
}
