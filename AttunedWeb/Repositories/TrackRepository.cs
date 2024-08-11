using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackRepository(IXmlParser xmlParser) : IRepository<TrackDto>
{
    public async Task<IEnumerable<TrackDto>> Get()
    {
        return (await xmlParser.ParseTracks()).Select(TrackDto.FromTrack);
    }
}