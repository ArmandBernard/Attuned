namespace iTunesSmartParser
{
    public enum LimitUnits
    {
        Minutes = 0x01,
        MB = 0x02,
        Items = 0x03,
        Hours = 0x04,
        GB = 0x05
    }

    public enum SelectionMethods
    {
        Random = 0x02,
        Name = 0x05,
        Album = 0x06,
        Artist = 0x07,
        Genre = 0x09,
        HighestRating = 0x1c,
        LowestRating = 0x01,
        RecentlyPlayed = 0x1a,
        OftenPlayed = 0x19,
        RecentlyAdded = 0x15
    }
}
