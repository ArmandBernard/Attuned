using System.Buffers.Binary;
using System.Text;
using iTunesSmartParser.Data;
using iTunesSmartParser.Data.Logic;
using iTunesSmartParser.Data.Rules;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Playlists;

/// <summary>
/// Turns a <see cref="Conjunction"/> back into the bytes iTunes stores as a smart playlist's "Smart Criteria". It is the
/// inverse of <see cref="CriteriaParser"/>.
///
/// The format was worked out by comparing every smart playlist in a real library. All integers are big endian.
///
/// A list of rules ("SLst") is a 136 byte header followed by its rules, one after another:
/// <code>
///   0   "SLst"
///   4   00 01 00 01     (never varies)
///   8   int             the number of rules (a group counts as one rule)
///   12  int             0 if all of the rules must match, 1 if any of them can
///   16  zeros           (120 bytes)
/// </code>
///
/// A rule is a 56 byte prefix, followed by its data. The prefix is:
/// <code>
///   0   int             the field being matched, or 0 for a group of rules
///   4   byte            the <see cref="Sign"/>: whether the rule is negated and whether it is a string or number rule
///   5   byte            always 0
///   6   byte            1 for a range ("between"), 2 for "in the last", 4 for "any of" a list of values, otherwise 0
///   7   byte            the <see cref="Operator"/>
///   8   zeros           (44 bytes)
///   52  int             the length of the data, in bytes
/// </code>
///
/// A string rule's data is the string in UTF-16, big endian. There is no null at the end.
///
/// Every other kind of rule has 68 bytes of data, made of six 8 byte numbers and 20 bytes of zeros. Each one
/// is either a number the rule is about or one of these values: the number being matched ("from value"), a relative
/// date ("from date"), the unit that relative date is in ("from units"), and then a second set of the same three for
/// the end of a range ("to value", "to date", "to units"). Numbers and dates the rule is about are set as a "from" and
/// a "to" value, but for any rule that is not a range the "to" is a copy of the "from". Both units are 1 unless the
/// rule is "in the last".
///
/// A group's data is a nested list of rules, so a group is a prefix with a field of 0, followed by a nested "SLst". The
/// nested list's rules follow its header directly.
/// </summary>
public static class CriteriaEncoder
{
    private const int HEADER_LENGTH = 136;
    private const int PREFIX_LENGTH = 56;
    private const int FIXED_DATA_LENGTH = 68;

    // offsets in a header
    private const int HEADER_RULE_COUNT = 8;
    private const int HEADER_CONJUNCTION = 12;

    // offsets in a rule's prefix
    private const int PREFIX_FIELD = 0;
    private const int PREFIX_SIGN = 4;
    private const int PREFIX_KIND = 6;
    private const int PREFIX_OPERATOR = 7;
    private const int PREFIX_DATA_LENGTH = 52;

    // offsets in a rule's 68 bytes of data
    private const int FROM_VALUE = 0;
    private const int FROM_DATE = 8;
    private const int FROM_UNITS = 16;
    private const int TO_VALUE = 24;
    private const int TO_UNITS = 40; // (the "to date" is at 32, and is always 0)

    // The values of PREFIX_KIND
    private const byte KIND_RANGE = 1;
    private const byte KIND_TIME_SPAN = 2;
    private const byte KIND_ANY_OF = 4;

    /// <summary>
    /// iTunes marks the "from value" and the "to value" of an "in the last" rule as not being used by filling them with
    /// this
    /// </summary>
    private const long UNUSED_VALUE = 0x2dae2dae2dae2daeL;

    /// <summary>
    /// iTunes stores a rating as 20 for each star, and any number up to 9 more is the same number of stars. So "up to
    /// 5 stars" is stored as up to 109, and "greater than 1 star" is stored as greater than 29.
    /// </summary>
    private const int RATING_STAR = 20;

    private const int RATING_STAR_RANGE_END = 9;

    private static readonly TimeUnits[] _timeUnitsLargestFirst = [TimeUnits.Months, TimeUnits.Weeks, TimeUnits.Days];

    /// <summary>
    /// Encode the rules as iTunes does.
    ///
    /// <see cref="Conjunction"/> keeps its rules and its groups apart, so it doesn't know which came first, and iTunes
    /// lets a person put them in any order. To keep the order that a playlist has, give the criteria the playlist has
    /// now. Any list in the conjunction with the same number of rules and groups as the one in the same place in the
    /// existing criteria, is written in the order of that one. Otherwise rules come before groups.
    /// </summary>
    /// <param name="conjunction">The rules to encode</param>
    /// <param name="existing">The criteria the playlist has now, if it has any</param>
    /// <exception cref="ArgumentException">A rule can't be stored in a smart playlist</exception>
    /// <exception cref="NotSupportedException">The kind of rule isn't supported yet</exception>
    public static byte[] Encode(Conjunction conjunction, byte[]? existing = null) =>
        EncodeList(conjunction, ReadOrder(existing));

    private static byte[] EncodeList(Conjunction conjunction, ListOrder? existingOrder)
    {
        var groups = conjunction.SubConjunctions;
        var rules = conjunction.Rules;

        // The existing order is only useful if it is of a list that is about the same
        var keptOrder = existingOrder != null && existingOrder.Matches(groups.Count, rules.Count) ? existingOrder : null;

        // true for each group, and false for each rule, in the order they are written
        var order = keptOrder?.IsGroup ??
                    Enumerable.Repeat(false, rules.Count).Concat(Enumerable.Repeat(true, groups.Count)).ToList();

        var children = new List<byte[]>();
        var groupIndex = 0;
        var ruleIndex = 0;

        foreach (var isGroup in order)
        {
            if (isGroup)
            {
                children.Add(EncodeGroup(groups[groupIndex], keptOrder?.Groups[groupIndex]));
                groupIndex++;
            }
            else
            {
                children.Add(EncodeRule(rules[ruleIndex]));
                ruleIndex++;
            }
        }

        var list = new byte[HEADER_LENGTH + children.Sum(x => x.Length)];

        "SLst"u8.CopyTo(list);
        list[5] = 1;
        list[7] = 1;
        WriteInt(list, HEADER_RULE_COUNT, children.Count);
        WriteInt(list, HEADER_CONJUNCTION, conjunction.Type == ConjunctionType.Or ? 1 : 0);

        var offset = HEADER_LENGTH;
        foreach (var child in children)
        {
            child.CopyTo(list, offset);
            offset += child.Length;
        }

        return list;
    }

    private static byte[] EncodeGroup(Conjunction group, ListOrder? existingOrder)
    {
        var list = EncodeList(group, existingOrder);

        // A group is the rule 0, with the usual sign and operator for "is"
        var rule = NewRule(0, list.Length, Sign.IntPositive, Operator.Is);
        // (in the 44 bytes iTunes leaves empty in every other rule, a group has one byte set)
        rule[PREFIX_OPERATOR + 1] = 1;

        list.CopyTo(rule, PREFIX_LENGTH);

        return rule;
    }

    /// <summary>
    /// The order that the rules and groups in a list are in, and the order in each of the groups in it
    /// </summary>
    private record ListOrder(IReadOnlyList<bool> IsGroup, IReadOnlyList<ListOrder> Groups)
    {
        public bool Matches(int groupCount, int ruleCount) =>
            IsGroup.Count(x => x) == groupCount && IsGroup.Count(x => !x) == ruleCount;
    }

    /// <summary>
    /// Find the order in criteria. If the criteria is not as expected, there is no order to keep.
    /// </summary>
    private static ListOrder? ReadOrder(byte[]? criteria)
    {
        if (criteria == null)
        {
            return null;
        }

        try
        {
            var order = ReadListOrder(criteria, 0, out var end);

            return end == criteria.Length ? order : null;
        }
        catch (Exception ex) when (ex is FormatException or ArgumentException)
        {
            return null;
        }
    }

    private static ListOrder ReadListOrder(byte[] criteria, int start, out int end)
    {
        if (start + HEADER_LENGTH > criteria.Length || !criteria.AsSpan(start, 4).SequenceEqual("SLst"u8))
        {
            throw new FormatException("Expected a list of rules.");
        }

        var count = ReadInt(criteria, start + HEADER_RULE_COUNT);
        var offset = start + HEADER_LENGTH;

        var isGroup = new List<bool>();
        var groups = new List<ListOrder>();

        for (var i = 0; i < count; i++)
        {
            var field = ReadInt(criteria, offset + PREFIX_FIELD);
            var dataLength = ReadInt(criteria, offset + PREFIX_DATA_LENGTH);

            if (dataLength < 0 || offset + PREFIX_LENGTH + dataLength > criteria.Length)
            {
                throw new FormatException("A rule is longer than the criteria.");
            }

            isGroup.Add(field == 0);

            if (field == 0)
            {
                groups.Add(ReadListOrder(criteria, offset + PREFIX_LENGTH, out _));
            }

            offset += PREFIX_LENGTH + dataLength;
        }

        end = offset;

        return new ListOrder(isGroup, groups);
    }

    private static byte[] EncodeRule(IRule rule) => rule switch
    {
        StringRule stringRule => EncodeString(stringRule),
        IntRule intRule => EncodeInt(intRule),
        DateRule dateRule => EncodeDate(dateRule),
        TimeSpanRule timeSpanRule => EncodeTimeSpan(timeSpanRule),
        BooleanRule booleanRule => EncodeBoolean(booleanRule),
        DictionaryRule dictionaryRule => EncodeDictionary(dictionaryRule),
        // The way a playlist is referred to isn't known well enough to write: the parser reads the playlist's id from
        // a different place to the one that other documentation says it is in, and no library was available to see
        // which was right.
        PlaylistRule => throw new NotSupportedException("Rules about other playlists can't be written yet."),
        _ => throw new ArgumentOutOfRangeException(nameof(rule), rule, "Unknown kind of rule.")
    };

    #region rules

    private static byte[] EncodeString(StringRule rule)
    {
        RequireOperator(rule, Operator.Is, Operator.Contains, Operator.Starts, Operator.Ends);
        RequireSign(rule, Sign.StringPositive, Sign.StringNegative);

        var text = Encoding.BigEndianUnicode.GetBytes(rule.Value);

        var bytes = NewRule((int) rule.Field, text.Length, rule.Sign, rule.Operator);

        text.CopyTo(bytes, PREFIX_LENGTH);

        return bytes;
    }

    private static byte[] EncodeInt(IntRule rule)
    {
        RequireOperator(rule, Operator.Is, Operator.Greater, Operator.Less, Operator.Other);
        RequireSign(rule, Sign.IntPositive, Sign.IntNegative);

        var isRange = rule.Operator == Operator.Other;

        var from = rule.ValueA;
        var to = isRange ? RequireSecondValue(rule, rule.ValueB) : from;

        if (rule.Field == IntFields.Rating)
        {
            // "up to 5 stars" and "greater than 1 star" both mean from after the last of the stars, so they
            // are the end of the stars' range. "less than 5 stars" is below the start of them.
            var endOfStars = rule.Operator is Operator.Greater ? RATING_STAR_RANGE_END : 0;

            from = ToRating(from) + endOfStars;
            to = ToRating(to) + (isRange ? RATING_STAR_RANGE_END : endOfStars);
        }

        var bytes = NewFixedRule((int) rule.Field, rule.Sign, rule.Operator);

        if (isRange)
        {
            bytes[PREFIX_KIND] = KIND_RANGE;
        }

        WriteLong(bytes, PREFIX_LENGTH + FROM_VALUE, from);
        WriteLong(bytes, PREFIX_LENGTH + TO_VALUE, to);

        return bytes;

        static long ToRating(long stars) => stars * RATING_STAR;
    }

    private static byte[] EncodeDate(DateRule rule)
    {
        RequireOperator(rule, Operator.Is, Operator.Greater, Operator.Less, Operator.Other);
        RequireSign(rule, Sign.IntPositive, Sign.IntNegative);

        var isRange = rule.Operator == Operator.Other;

        var from = Utils.DateTimeToMacSeconds(rule.ValueA);
        var to = isRange ? Utils.DateTimeToMacSeconds(RequireSecondValue(rule, rule.ValueB)) : from;

        var bytes = NewFixedRule((int) rule.Field, rule.Sign, rule.Operator);

        if (isRange)
        {
            bytes[PREFIX_KIND] = KIND_RANGE;
        }

        WriteLong(bytes, PREFIX_LENGTH + FROM_VALUE, from);
        WriteLong(bytes, PREFIX_LENGTH + TO_VALUE, to);

        return bytes;
    }

    /// <summary>
    /// "in the last [number] [days, weeks or months]"
    /// </summary>
    private static byte[] EncodeTimeSpan(TimeSpanRule rule)
    {
        RequireOperator(rule, Operator.Other);
        RequireSign(rule, Sign.IntPositive, Sign.IntNegative);

        var seconds = (long) rule.Value.TotalSeconds;

        var unit = ChooseTimeUnit(seconds) ?? throw new ArgumentException(
            $"{rule.Value} can't be stored as a whole number of days, weeks or months.", nameof(rule));

        var bytes = NewFixedRule((int) rule.Field, rule.Sign, rule.Operator);

        bytes[PREFIX_KIND] = KIND_TIME_SPAN;

        WriteLong(bytes, PREFIX_LENGTH + FROM_VALUE, UNUSED_VALUE);
        // "in the last" a number of units is a date that many units before now
        WriteLong(bytes, PREFIX_LENGTH + FROM_DATE, -(seconds / (int) unit));
        WriteLong(bytes, PREFIX_LENGTH + FROM_UNITS, (int) unit);
        WriteLong(bytes, PREFIX_LENGTH + TO_VALUE, UNUSED_VALUE);

        return bytes;
    }

    /// <summary>
    /// A time span is stored as a number of one unit, so choose the largest unit that gets it exactly. This means that
    /// 14 days is stored as "2 weeks".
    /// </summary>
    private static TimeUnits? ChooseTimeUnit(long seconds)
    {
        foreach (var unit in _timeUnitsLargestFirst)
        {
            if (seconds > 0 && seconds % (int) unit == 0)
            {
                return unit;
            }
        }

        return null;
    }

    private static byte[] EncodeBoolean(BooleanRule rule)
    {
        RequireOperator(rule, Operator.Is);
        RequireSign(rule, Sign.IntPositive, Sign.IntNegative);

        var bytes = NewFixedRule((int) rule.Field, rule.Sign, rule.Operator);

        // The same as an "is 1" rule on a number
        WriteLong(bytes, PREFIX_LENGTH + FROM_VALUE, 1);
        WriteLong(bytes, PREFIX_LENGTH + TO_VALUE, 1);

        return bytes;
    }

    private static byte[] EncodeDictionary(DictionaryRule rule)
    {
        RequireOperator(rule, Operator.Is, Operator.Other);
        RequireSign(rule, Sign.IntPositive, Sign.IntNegative);

        var bytes = NewFixedRule((int) rule.Field, rule.Sign, rule.Operator);

        // "Other" means the value is one of a list: iTunes uses it for the playlists that are built in
        if (rule.Operator == Operator.Other)
        {
            bytes[PREFIX_KIND] = KIND_ANY_OF;
        }

        var value = LookUpValue(rule);

        WriteLong(bytes, PREFIX_LENGTH + FROM_VALUE, value);
        WriteLong(bytes, PREFIX_LENGTH + TO_VALUE, value);

        return bytes;
    }

    /// <summary>
    /// Find the number for the name of a value in a list. More than one number can have the same name. "Music" is 1
    /// when it is chosen by a person, but "Music" in the playlist that has all of the music is another number.
    /// </summary>
    private static int LookUpValue(DictionaryRule rule)
    {
        var dictionary = rule.Field switch
        {
            DictionaryFields.Love => Dictionaries.Love,
            DictionaryFields.MediaKind => Dictionaries.Media,
            DictionaryFields.Location => Dictionaries.Location,
            DictionaryFields.iCloudStatus => Dictionaries.iCloudStatus,
            _ => throw new ArgumentOutOfRangeException(nameof(rule), rule.Field, "Unknown field.")
        };

        var matches = dictionary.Where(x => x.Value == rule.Value).Select(x => x.Key).ToList();

        if (matches.Count == 0)
        {
            throw new ArgumentException($"'{rule.Value}' is not a value of {rule.Field}.", nameof(rule));
        }

        return rule.Operator == Operator.Is ? matches.First() : matches.Last();
    }

    #endregion

    #region helpers

    private static T RequireSecondValue<T>(IRule rule, T? value) where T : struct =>
        value ?? throw new ArgumentException($"A range needs two values, but this {rule.GetType().Name} has one.",
            nameof(rule));

    private static void RequireOperator(IRule rule, params Operator[] allowed)
    {
        if (!allowed.Contains(rule.Operator))
        {
            throw new ArgumentException(
                $"{rule.GetType().Name} can't use the operator {rule.Operator}. It can use {string.Join(", ", allowed)}.",
                nameof(rule));
        }
    }

    private static void RequireSign(IRule rule, params Sign[] allowed)
    {
        if (!allowed.Contains(rule.Sign))
        {
            throw new ArgumentException(
                $"{rule.GetType().Name} can't use the sign {rule.Sign}. It can use {string.Join(", ", allowed)}.",
                nameof(rule));
        }
    }

    private static byte[] NewFixedRule(int field, Sign sign, Operator @operator)
    {
        var bytes = NewRule(field, FIXED_DATA_LENGTH, sign, @operator);

        // The units are the number 1, except where a number of days, weeks or months is given
        WriteLong(bytes, PREFIX_LENGTH + FROM_UNITS, 1);
        WriteLong(bytes, PREFIX_LENGTH + TO_UNITS, 1);

        return bytes;
    }

    private static byte[] NewRule(int field, int dataLength, Sign sign, Operator @operator)
    {
        var bytes = new byte[PREFIX_LENGTH + dataLength];

        WriteInt(bytes, PREFIX_FIELD, field);
        bytes[PREFIX_SIGN] = (byte) sign;
        bytes[PREFIX_OPERATOR] = (byte) @operator;
        WriteInt(bytes, PREFIX_DATA_LENGTH, dataLength);

        return bytes;
    }

    private static int ReadInt(byte[] bytes, int offset) =>
        BinaryPrimitives.ReadInt32BigEndian(bytes.AsSpan(offset));

    private static void WriteInt(byte[] bytes, int offset, int value) =>
        BinaryPrimitives.WriteInt32BigEndian(bytes.AsSpan(offset), value);

    private static void WriteLong(byte[] bytes, int offset, long value) =>
        BinaryPrimitives.WriteInt64BigEndian(bytes.AsSpan(offset), value);

    #endregion
}
