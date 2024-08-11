using AttunedWebApi.Dtos;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackRepository(IXmlSource xmlSource, ITrackListParser trackListParser) : IRepository<TrackDto, TrackDto>
{
    public async Task<IEnumerable<TrackDto>> Get(CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);
        
        return trackListParser.ParseDocument(doc).Select(TrackDto.FromTrack);
    }

    public async Task<TrackDto?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlSource.GetXDocument(token);
        
        var track = trackListParser.GetById(doc, id);

        return track != null ? TrackDto.FromTrack(track) : null;
    }
}