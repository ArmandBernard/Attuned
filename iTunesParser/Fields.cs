namespace iTunesSmartParser;

public enum StringFields
{
	Album = 0x03,
	AlbumArtist = 0x47,
	Artist = 0x04,
	Category = 0x37,
	Comments = 0x0e,
	Composer = 0x12,
	Description = 0x36,
	Genre = 0x08,
	Grouping = 0x27,
	Kind = 0x09,
	Name = 0x02,
	Show = 0x3e,
	SortAlbum = 0x4f,
	SortAlbumartist = 0x51,
	SortComposer = 0x52,
	SortName = 0x4e,
	SortShow = 0x53,
	VideoRating = 0x59
}

public enum IntFields
{
	BPM = 0x23,
	BitRate = 0x05,
	Compilation = 0x1f,
	DiskNumber = 0x18,
	Plays = 0x16,
	Rating = 0x19,
	Podcast = 0x39,
	SampleRate = 0x06,
	Season = 0x3f,
	Size = 0x0c,
	Skips = 0x44,
	Duration = 0x0d,
	TrackNumber = 0x0b,
	Year = 0x07
}

public enum BoolFields
{
	HasArtwork = 0x25,
	Purchased = 0x29,
	Checked = 0x1d
}

public enum DateFields
{
	DateAdded = 0x10,
	DateModified = 0x0a,
	LastPlayed = 0x17,
	LastSkipped = 0x45
}

public enum MediaKindFields
{
	MediaKind = 0x3c
}

public enum PlaylistFields
{
	PlaylistPersistentID = 0x28
}

public enum LoveFields
{
	Love = 0x9a
}

public enum CloudFields
{
	iCloudStatus = 0x86
}

public enum LocationFields
{
	Location = 0x85
}
