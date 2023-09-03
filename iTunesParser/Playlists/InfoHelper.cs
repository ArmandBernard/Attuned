using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Limits;

namespace iTunesSmartParser.Playlists;

public class InfoHelper
{
    // INFO OFFSETS
    // Offsets for bytes which...
    const int LIVEUPDATE = 0; // determine whether live updating is enabled - Absolute offset
    const int MATCHBOOL = 1; // determine whether logical matching is to be performed - Absolute offset

    const int SELECTIONMETHOD = 7; // determine by what criteria limited playlists are populated - Absolute offset

    const int LIMITBOOL = 2; // determine whether results are limited - Absolute offset
    const int LIMITMETHOD = 3; // determine by what criteria the results are limited - Absolute offset
    const int LIMITINT = 8; // determine the limited - Absolute offset
    const int LIMITCHECKED = 12; // determine whether to exclude unchecked items - Absolute offset

    const int
        SELECTIONMETHODSIGN = 13; // determine whether certain selection methods are "most" or "least" - Absolute offset

    private byte[] Info { get; }

    public InfoHelper(byte[] info)
    {
        Info = info;
    }

    public bool IsLimited => Info[LIMITBOOL] == 1;

    public bool OnlyChecked => Info[LIMITCHECKED] == 1;

    public SelectionMethods SortBy => (SelectionMethods) Info[SELECTIONMETHOD];

    public bool SortDescending => Info[SELECTIONMETHODSIGN] == 0;

    public LimitUnits LimitUnits => (LimitUnits) Convert.ToInt32(Info[LIMITMETHOD]);

    public int LimitNumber => Utils.ByteToInt(Info.Skip(LIMITINT).Take(4).ToArray());

    public bool Match => Info[MATCHBOOL] == 1;
    
    public bool LiveUpdate => Info[LIVEUPDATE] == 1;
}