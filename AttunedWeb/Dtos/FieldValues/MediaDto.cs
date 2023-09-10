using AttunedWebApi.CodeGen;
using Reinforced.Typings.Attributes;

namespace AttunedWebApi.Dtos.FieldValues;

[TypescriptEnum]
public enum MediaDto
{
    Music,
    Movie,
    Podcast,
    Audiobook,
    [TsValue(Initializer = "Music Video")] MusicVideo,
    [TsValue(Initializer = "TV Show")] TvShow,
    [TsValue(Initializer = "Home Video")] HomeVideo,
    [TsValue(Initializer = "iTunes Extras")] iTunesExtras,
    [TsValue(Initializer = "Voice Memo")] VoiceMemo,
    [TsValue(Initializer = "iTunes U")] iTunesU,
    Book,
    [TsValue(Initializer = "Book or Audiobook")] BookOrAudiobook,
    [TsValue(Initializer = "Undesired Music")] UndesiredMusic,
    [TsValue(Initializer = "Undesired Other")] UndesiredOther,
}