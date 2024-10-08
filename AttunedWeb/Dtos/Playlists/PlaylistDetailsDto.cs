using AttunedWebApi.CodeGen;
using AttunedWebApi.Dtos.Conjunction;
using AttunedWebApi.Dtos.Limits;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Dtos.Playlists;

[TypescriptDto]
public record PlaylistDetailsDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<int> Items { get; init; }
    public required bool IsSmart { get; init; }
    
    [TypescriptNullable(Type = nameof(LimitDto))]
    public required LimitDto? Limit { get; init; }
    
    [TypescriptNullable(Type = nameof(ConjunctionDto))]
    public required ConjunctionDto? RuleConjunction { get; init; }
    public bool LiveUpdating { get; init; }

    public static PlaylistDetailsDto FromPlaylistDetails(PlaylistDetails playlist) =>
        new()
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Items = playlist.Items,
            IsSmart = playlist.IsSmart,
            Limit = playlist.Filters != null ? LimitDto.FromLimit(playlist.Filters.Limit) : null,
            RuleConjunction = playlist.Filters?.RuleConjunction?.ToDto(),
            LiveUpdating = playlist.Filters?.LiveUpdating ?? false
        };
}