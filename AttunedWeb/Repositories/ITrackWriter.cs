using AttunedWebApi.Dtos.Tracks;
using iTunesSmartParser.Data;

namespace AttunedWebApi.Repositories;

public interface ITrackWriter
{
    /// <summary>
    /// Apply a patch to a track in the library
    /// </summary>
    /// <returns>The updated track, or null if there is no track with that id</returns>
    Task<TrackDto?> Patch(int id, TrackPatch patch, CancellationToken token);
}
