using AttunedWebApi.CodeGen;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Dtos.Playlists;

[TypescriptDto]
public record PlaylistDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsSmart { get; init; }

    public static PlaylistDto FromPlaylist(Playlist playlist) =>
        new()
        {
            Id = playlist.Id,
            Name = playlist.Name,
            IsSmart = playlist.IsSmart
        };
}