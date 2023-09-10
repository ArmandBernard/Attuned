using iTunesSmartParser.Fields;

namespace AttunedWebApi.Dtos.Fields;

public static class FieldConverter
{
    public static BoolFieldsDto ToDto(this BoolFields fields) => fields switch
    {
        BoolFields.HasArtwork => BoolFieldsDto.HasArtwork,
        BoolFields.Purchased => BoolFieldsDto.Purchased,
        BoolFields.Checked => BoolFieldsDto.Checked,
        _ => throw new ArgumentOutOfRangeException(nameof(fields), fields, null)
    };

    public static DateFieldsDto ToDto(this DateFields fields) => fields switch
    {
        DateFields.DateAdded => DateFieldsDto.DateAdded,
        DateFields.DateModified => DateFieldsDto.DateModified,
        DateFields.LastPlayed => DateFieldsDto.LastPlayed,
        DateFields.LastSkipped => DateFieldsDto.LastSkipped,
        _ => throw new ArgumentOutOfRangeException(nameof(fields), fields, null)
    };

    public static DictionaryFieldsDto ToDto(this DictionaryFields fields) => fields switch
    {
        DictionaryFields.MediaKind => DictionaryFieldsDto.MediaKind,
        DictionaryFields.Location => DictionaryFieldsDto.Location,
        DictionaryFields.iCloudStatus => DictionaryFieldsDto.iCloudStatus,
        DictionaryFields.Love => DictionaryFieldsDto.Love,
        _ => throw new ArgumentOutOfRangeException(nameof(fields), fields, null)
    };

    public static IntFieldsDto ToDto(this IntFields fields) => fields switch
    {
        IntFields.Bpm => IntFieldsDto.Bpm,
        IntFields.BitRate => IntFieldsDto.BitRate,
        IntFields.Compilation => IntFieldsDto.Compilation,
        IntFields.DiskNumber => IntFieldsDto.DiskNumber,
        IntFields.Plays => IntFieldsDto.Plays,
        IntFields.Rating => IntFieldsDto.Rating,
        IntFields.Podcast => IntFieldsDto.Podcast,
        IntFields.SampleRate => IntFieldsDto.SampleRate,
        IntFields.Season => IntFieldsDto.Season,
        IntFields.Size => IntFieldsDto.Size,
        IntFields.Skips => IntFieldsDto.Skips,
        IntFields.Duration => IntFieldsDto.Duration,
        IntFields.TrackNumber => IntFieldsDto.TrackNumber,
        IntFields.Year => IntFieldsDto.Year,
        _ => throw new ArgumentOutOfRangeException(nameof(fields), fields, null)
    };

    public static StringFieldsDto ToDto(this StringFields fields) => fields switch
    {
        StringFields.Album => StringFieldsDto.Album,
        StringFields.AlbumArtist => StringFieldsDto.AlbumArtist,
        StringFields.Artist => StringFieldsDto.Artist,
        StringFields.Category => StringFieldsDto.Category,
        StringFields.Comments => StringFieldsDto.Comments,
        StringFields.Composer => StringFieldsDto.Composer,
        StringFields.Description => StringFieldsDto.Description,
        StringFields.Genre => StringFieldsDto.Genre,
        StringFields.Grouping => StringFieldsDto.Grouping,
        StringFields.Kind => StringFieldsDto.Kind,
        StringFields.Name => StringFieldsDto.Name,
        StringFields.Show => StringFieldsDto.Show,
        StringFields.SortAlbum => StringFieldsDto.SortAlbum,
        StringFields.SortAlbumArtist => StringFieldsDto.SortAlbumArtist,
        StringFields.SortComposer => StringFieldsDto.SortComposer,
        StringFields.SortName => StringFieldsDto.SortName,
        StringFields.SortShow => StringFieldsDto.SortShow,
        StringFields.VideoRating => StringFieldsDto.VideoRating,
        _ => throw new ArgumentOutOfRangeException(nameof(fields), fields, null)
    };
}