using System.Text;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Playlists;

public class PlaylistCriteriaHelper
{
    private readonly byte[] Criteria;

    #region constants

    // CRITERIA OFFSETS
    // Offsets for bytes which...
    const int FIELD = 139; // determine what is being matched (Artist, Album, &c) - Absolute offset

    const int LOGICTYPE = 15; // determine whether all or any criteria must match - Absolute offset

    const int
        LOGICSIGN = 1; // determine whether the matching rule is positive or negative (e.g., is vs. is not) - Relative offset from FIELD

    const int LOGICRULE = 4; // determine the kind of logic used (is, contains, begins, &c) - Relative offset from FIELD

    const int
        SUBLOGICTYPE =
            68; // determine whether all or any criteria must match in the subexpression - Relative offset from FIELD
    const int SUBINT = 61; // begin the int field that counts subexpressions - Relative offset from FIELD
    const int SUBEXPRESSIONLENGTH = 192; // The length of a subexpression starting from FIELD

    const int STRING = 54; // begin string data - Relative offset from FIELD
    const int INTA = 57; // begin the first int - Relative offset from FIELD
    const int INTB = 24; // begin the second int - Relative offset from INTA
    const int PLAYLISTA = 20; // begin the first playlist field - Relative offset from FIELD
    const int PLAYLISTB = 4; // begin the second playlist field - Relative offset from PLAYLISTA
    const int INTLENGTH = 67; // The length on a int criteria starting at the first int

    const int TIMEMULTIPLE = 73; // begin the int with the multiple of time - Relative offset from FIELD
    const int TIMEVALUE = 65; // begin the inverse int with the value of time - Relative offset from FIELD

    public static int StartingOffset => FIELD;

    #endregion

    public int Field(int offset) => Criteria[offset];

    #region logic properties

    public bool LogicIsOr => Criteria[LOGICTYPE] == 1;

    public bool SubLogicIsOr(int offset) => Criteria[offset + SUBLOGICTYPE] == 1;

    public LogicSign GetSign(int offset) => (LogicSign) Criteria[offset + LOGICSIGN];

    public LogicRule GetRule(int offset) => (LogicRule) Criteria[offset + LOGICRULE];

    public bool IsRangeField(int offset) =>
        GetRule(offset) == LogicRule.Other && Criteria[offset + LOGICSIGN + 2] == 1;

    public int NumberOfSubExpressions(int offset) => Utils.ByteToInt(Criteria.Skip(offset + SUBINT).Take(4));

    public static int PostSubExpressionOffset(int offset) => offset + SUBEXPRESSIONLENGTH;

    #endregion

    #region strings

    public StringFields String(int offset) => (StringFields) Criteria[offset + STRING];

    public string ReadStringContent(int offset)
    {
        // get remaining bytes of criteria from start of string field
        var remainingbytes = Criteria.Skip(offset + STRING).ToArray();

        // if there are uneven remaining bytes
        if (remainingbytes.Length % 2 != 0)
        {
            // add a padding null byte
            // (Needed for UTF16 parsing)
            remainingbytes = remainingbytes.Concat(new byte[] {0}).ToArray();
        }

        // iTunes uses UTF16 encoding for the string. Convert all of the remaining bytes to a string
        var content = Encoding.Unicode.GetString(remainingbytes);

        // The string should stop with either a null character or the end of the data.
        // try to find the first null character
        var stringEnd = content.IndexOf('\0');

        // if there are nulls crop after first null. It's the end of the string.
        return stringEnd != -1 ? content[..stringEnd] : content;
    }

    public static int PostStringOffset(int offset, string content) => offset + STRING + 2 * content.Length + 2;

    #endregion

    #region ints

    public int IntA(int offset) => HandleIntField(offset, INTA);

    public int IntB(int offset) => HandleIntField(offset, INTA + INTB);

    private int HandleIntField(int offset, int valueOffset)
    {
        var rawValue = Utils.ByteToInt(Criteria.Skip(offset + valueOffset).Take(4));
        
        // ratings field requires special attention as it gives weird numbers like 109 for 100%
        if ((IntFields) Field(offset) == IntFields.Rating)
        {
            return rawValue / 20;
        }
        
        return rawValue;
    } 
    
    #endregion

    #region dates

    public DateTime DateA(int offset) => Utils.BytesToDateTime(Criteria.Skip(offset + INTA).Take(4));

    public DateTime DateB(int offset) => Utils.BytesToDateTime(Criteria.Skip(offset + INTA + INTB).Take(4));

    public bool IsTimeSpanRule(int offset) =>
        GetRule(offset) == LogicRule.Other && Criteria[offset + LOGICSIGN + 2] == 2;

    public TimeUnits TimeUnits(int offset) =>
        (TimeUnits) Utils.ByteToInt(Criteria.Skip(offset + TIMEMULTIPLE).Take(4));

    // Determine the number of the given time unit
    // (I have no idea why this needs two's complement, needs to be capped or the + 1)
    public int TimeSpanValue(int offset)
    {
        var bytes = Criteria.Skip(offset + TIMEVALUE).Take(4);

        var flippedBytes = bytes.Select(b => (byte) ~b);

        return (int) ((Utils.ByteToInt(flippedBytes) + 1) % 4294967296);
    }

    #endregion

    #region playlist ids

    private int PlaylistA(int offset) => Utils.ByteToInt(Criteria.Skip(offset + PLAYLISTA).Take(4));

    private int PlaylistB(int offset) => Utils.ByteToInt(Criteria.Skip(offset + PLAYLISTA + PLAYLISTB).Take(4));

    // creates two 8 digit hex strings representing each number. E.g. (A: 1, B:255) would be 00000001000000FF
    public string PlaylistId(int offset) => $"{PlaylistA(offset):X8}{PlaylistB(offset):X8}";

    #endregion

    public static int PostFixedLengthFieldOffset(int offset) => offset + INTA + INTLENGTH;

    public bool KeepReading(int offset) => offset < Criteria.Length;

    public PlaylistCriteriaHelper(byte[] criteria)
    {
        Criteria = criteria;
    }
}