using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class PlaylistRepository(IXmlSource xmlSource, IPlaylistParser playlistParser) : IRepository<PlaylistDto, PlaylistDto>
{
    public async Task<IEnumerable<PlaylistDto>> Get(CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);
        
        return playlistParser.ParseDocument(doc)
            .Where(x => x.Name != "Downloaded" && x.Name != "Library")
            .Select(PlaylistDto.FromPlaylist);
    }

    public async Task<PlaylistDto?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);

        var playlist = playlistParser.GetById(doc, id);
        
        return playlist != null ? PlaylistDto.FromPlaylist(playlist) : null;
    }
}