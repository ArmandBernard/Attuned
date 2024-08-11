using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class PlaylistRepository(IXmlParser xmlParser) : IRepository<PlaylistDto>
{
    public async Task<IEnumerable<PlaylistDto>> Get()
    {
        return (await xmlParser.ParsePlaylists())
            .Where(x => x.Name != "Downloaded" && x.Name != "Library")
            .Select(PlaylistDto.FromPlaylist);
    }
}