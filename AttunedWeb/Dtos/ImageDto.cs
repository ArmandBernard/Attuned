using AttunedWebApi.CodeGen;

namespace AttunedWebApi.Dtos;

[TypescriptDto]
public record ImageDto(string Image, IEnumerable<int> TrackIds);