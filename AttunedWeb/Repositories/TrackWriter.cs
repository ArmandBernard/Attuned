using AttunedWebApi.Dtos.Tracks;
using iTunesSmartParser.Data;
using iTunesSmartParser.Xml;

namespace AttunedWebApi.Repositories;

public class TrackWriter(IITunesXmlReader xmlReader, IITunesXmlWriter xmlWriter, ITrackListWriter trackListWriter, ICacheInvalidator cacheInvalidator)
    : ITrackWriter
{
    // Patching is a read-modify-write of the whole library file, so only one can happen at a time
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<TrackDto?> Patch(int id, TrackPatch patch, CancellationToken token)
    {
        await _lock.WaitAsync(token);

        try
        {
            // keep the file's formatting, so that only the lines that change are different when it is saved
            var doc = await xmlReader.GetXDocument(token);

            var track = trackListWriter.ApplyPatch(doc, id, patch);

            if (track == null)
            {
                return null;
            }

            await xmlWriter.SaveXDocument(doc, token);

            cacheInvalidator.Invalidate();

            return TrackDto.FromTrack(track);
        }
        finally
        {
            _lock.Release();
        }
    }
}
