using System.Text;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Playlists;

public class CriteriaHelper(byte[] criteria)
{
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

    const int STRINGLENGTH = 49; // begin the int that is the length in bytes of the string - Relative offset from FIELD
    const int STRING = 53; // begin string data - Relative offset from FIELD
    const int INTA = 57; // begin the first int - Relative offset from FIELD
    const int INTB = 24; // begin the second int - Relative offset from INTA
    const int PLAYLISTA = 20; // begin the first playlist field - Relative offset from FIELD
    const int PLAYLISTB = 4; // begin the second playlist field - Relative offset from PLAYLISTA
    const int INTLENGTH = 67; // The length on a int criteria starting at the first int

    const int TIMEMULTIPLE = 73; // begin the int with the multiple of time - Relative offset from FIELD
    const int TIMEVALUE = 65; // begin the inverse int with the value of time - Relative offset from FIELD

    public static int StartingOffset => FIELD;

    #endregion

    public int Field(int offset) => criteria[offset];

    #region logic properties

    public bool LogicIsOr => criteria[LOGICTYPE] == 1;

    public bool SubLogicIsOr(int offset) => criteria[offset + SUBLOGICTYPE] == 1;

    public Sign GetSign(int offset) => (Sign) criteria[offset + LOGICSIGN];

    public Operator GetRule(int offset) => (Operator) criteria[offset + LOGICRULE];

    public bool IsRangeField(int offset) =>
        GetRule(offset) == Operator.Other && criteria[offset + LOGICSIGN + 2] == 1;

    public int NumberOfSubExpressions(int offset) => Utils.ByteToInt(criteria.Skip(offset + SUBINT).Take(4));

    public static int PostSubExpressionOffset(int offset) => offset + SUBEXPRESSIONLENGTH;

    #endregion

    #region strings

    /// <summary>
    /// Strings are stored as a length in bytes, followed by that many bytes of big endian UTF-16.
    /// </summary>
    public string ReadStringContent(int offset)
    {
        var start = offset + STRING;

        var length = Utils.ByteToInt(criteria.Skip(offset + STRINGLENGTH).Take(4));

        // don't read past the end if the data is cut short
        length = Math.Clamp(length, 0, criteria.Length - start);

        // a character is 2 bytes, so a half character at the end can't be read
        return Encoding.BigEndianUnicode.GetString(criteria, start, length - length % 2);
    }

    // A rule's data ends 3 bytes before the next rule's FIELD, because the first 3 bytes of a field's 4 byte id are 0.
    public static int PostStringOffset(int offset, string content) => offset + STRING + 2 * content.Length + 3;

    #endregion

    #region ints

    public int IntA(int offset) => HandleIntField(offset, INTA);

    public int IntB(int offset) => HandleIntField(offset, INTA + INTB);

    private int HandleIntField(int offset, int valueOffset)
    {
        var rawValue = Utils.ByteToInt(criteria.Skip(offset + valueOffset).Take(4));
        
        // ratings field requires special attention as it gives weird numbers like 109 for 100%
        if ((IntFields) Field(offset) == IntFields.Rating)
        {
            return rawValue / 20;
        }
        
        return rawValue;
    } 
    
    #endregion

    #region dates

    public DateTime DateA(int offset) => Utils.BytesToDateTime(criteria.Skip(offset + INTA).Take(4));

    public DateTime DateB(int offset) => Utils.BytesToDateTime(criteria.Skip(offset + INTA + INTB).Take(4));

    public bool IsTimeSpanRule(int offset) =>
        GetRule(offset) == Operator.Other && criteria[offset + LOGICSIGN + 2] == 2;

    public TimeUnits TimeUnits(int offset) =>
        (TimeUnits) Utils.ByteToInt(criteria.Skip(offset + TIMEMULTIPLE).Take(4));

    // Determine the number of the given time unit
    // (I have no idea why this needs two's complement, needs to be capped or the + 1)
    public int TimeSpanValue(int offset)
    {
        var bytes = criteria.Skip(offset + TIMEVALUE).Take(4);

        var flippedBytes = bytes.Select(b => (byte) ~b);

        return (int) ((Utils.ByteToInt(flippedBytes) + 1) % 4294967296);
    }

    #endregion

    #region playlist ids

    private int PlaylistA(int offset) => Utils.ByteToInt(criteria.Skip(offset + PLAYLISTA).Take(4));

    private int PlaylistB(int offset) => Utils.ByteToInt(criteria.Skip(offset + PLAYLISTA + PLAYLISTB).Take(4));

    // creates two 8 digit hex strings representing each number. E.g. (A: 1, B:255) would be 00000001000000FF
    public string PlaylistId(int offset) => $"{PlaylistA(offset):X8}{PlaylistB(offset):X8}";

    #endregion

    public static int PostFixedLengthFieldOffset(int offset) => offset + INTA + INTLENGTH;

    public bool KeepReading(int offset) => offset < criteria.Length;
}