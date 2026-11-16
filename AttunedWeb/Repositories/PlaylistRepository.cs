using AttunedWebApi.Dtos.Playlists;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class PlaylistRepository(IITunesXmlReader xmlReader, IPlaylistParser playlistParser) : IRepository<PlaylistDto, PlaylistDetailsDto>
{
    public async Task<IEnumerable<PlaylistDto>> Get(CancellationToken token)
    {
        var doc = await xmlReader.GetXDocument(token);
        
        return playlistParser.ParseDocument(doc)
            .Where(x => x.Name != "Downloaded" && x.Name != "Library")
            .Select(PlaylistDto.FromPlaylist);
    }

    public async Task<PlaylistDetailsDto?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlReader.GetXDocument(token);

        var playlist = playlistParser.GetById(doc, id);
        
        return playlist != null ? PlaylistDetailsDto.FromPlaylistDetails(playlist) : null;
    }
}