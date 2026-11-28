using AttunedWebApi.Dtos.Tracks;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackRepository(IITunesXmlReader xmlReader, ITrackListParser trackListParser)
    : IRepository<TrackDto, TrackDetailsDto>
{
    public async Task<IEnumerable<TrackDto>> Get(CancellationToken token)
    {
        var doc = await xmlReader.GetXDocument(token);

        return trackListParser.ParseDocument(doc).Select(TrackDto.FromTrack);
    }

    public async Task<TrackDto?> GetById(int id, CancellationToken token)
    {
        var doc = await xmlReader.GetXDocument(token);

        var track = trackListParser.GetById(doc, id);

        return track != null ? TrackDto.FromTrack(track) : null;
    }

    public async Task<TrackDetailsDto?> GetDetailsById(int id, CancellationToken token)
    {
        var doc = await xmlReader.GetXDocument(token);

        var track = trackListParser.GetDetailsById(doc, id);

        return track != null ? TrackDetailsDto.FromTrackDetails(track) : null;
    }
}