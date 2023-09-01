﻿namespace iTunesSmartParser;

public static partial class Parser
{
    public static readonly Dictionary<string, string> FileKinds = new Dictionary<string, string>()
    {
        {"Protected AAC audio file", ".m4p"},
        {"MPEG audio file", ".mp3"},
        {"AIFF audio file", ".aiff"},
        {"WAV audio file", ".wav"},
        {"QuickTime movie file", ".mov"},
        {"MPEG-4 video file", ".mp4"},
        {"AAC audio file", ".m4a"},
    };

    public static readonly Dictionary<int, string> MediaKinds = new Dictionary<int, string>()
    {
        {0x01, "Music"},
        {0x02, "Movie"},
        {0x04, "Podcast"},
        {0x08, "Audiobook"},
        {0x20, "Music Video"},
        {0x40, "TV Show"},
        {0x400, "Home Video"},
        {0x10000, "iTunes Extras"},
        {0x100000, "Voice Memo"},
        {0x200000, "iTunes U"},
        {0xC00000, "Book"},
        {0xC00008, "Book or Audiobook"},
        {0x1021B1, "Music"},
        {0x208004, "Undesired Music"},
        {0x20A004, "Undesired Other"}
    };

    public static readonly Dictionary<int, string> LoveKinds = new Dictionary<int, string>()
    {
        {0x00, "None"},
        {0x02, "Loved"},
        {0x03, "Disliked"}
    };

    public static readonly Dictionary<int, string> LocationKinds = new Dictionary<int, string>()
    {
        {0x01, "Computer"},
        {0x10, "iCloud"}
    };
}