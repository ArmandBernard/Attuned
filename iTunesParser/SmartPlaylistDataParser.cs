using iTunesSmartParser.Fields;
using System.Text;

namespace iTunesSmartParser;

public static class SmartPlaylistDataParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="criteria"></param>
    /// <param name="useAltNeq"></param>
    /// <returns></returns>
    public static PlayListInfo ParsePlaylistInfo(
        byte[] info, byte[] criteria, bool useAltNeq = false, bool useBetween = true
    )
    {
        var playlistInfo = new PlayListInfo()
        {
            Info = info,
            Criteria = criteria,
            UseBetween = useBetween
        };

        if (useAltNeq)
        {
            playlistInfo.Neq = "<>";
        }

        // parse info and criteria bytes
        playlistInfo.ParseBytes();

        return playlistInfo;
    }

    public class PlayListInfo
    {
        #region Raw Data

        public byte[] Info { get; set; }
        public byte[] Criteria { get; set; }

        #endregion

        #region String Output

        /// <summary>
        /// Not equal sign
        /// </summary>
        public string Neq = "!=";

        /// <summary>
        /// Use between instead of ranges
        /// </summary>
        public bool UseBetween = true;

        public string CompleteQuery
        {
            get
            {
                string query = QuerySelect;

                if (!string.IsNullOrEmpty(QueryWhere))
                {
                    query += "\nWHERE\n\t" + QueryWhere;
                }

                if (!string.IsNullOrEmpty(QueryOrder))
                {
                    query += "\n" + QueryOrder;
                }

                return query;
            }
        }

        public string QuerySelect { get; set; }

        public string QueryWhere { get; set; }

        public string QueryOrder { get; set; }

        private string ConjQuery { get; set; }

        public string Output { get; set; }
        private string ConjOutput { get; set; }

        #endregion

        #region Match Rules

        private int Offset { get; set; } = FIELD;

        private int Field => Criteria[Offset];

        public bool Match => Info[MATCHBOOL] == 1;

        public bool LogicIsOr => Criteria[LOGICTYPE] == 1;

        private bool SubLogicIsOr => Criteria[Offset + SUBLOGICTYPE] == 1;

        private LogicSign LogicSign => (LogicSign) Criteria[Offset + LOGICSIGN];

        private LogicRule LogicRule => (LogicRule) Criteria[Offset + LOGICRULE];

        private StringFields String => (StringFields) Criteria[Offset + STRING];

        private bool KeepReading => Offset < Criteria.Length;

        /// <summary>
        /// Int Property A
        /// </summary>
        private int IntA
        {
            get
            {
                int value = ByteToInt(Criteria.Skip(Offset + INTA).Take(4));
                // if value is rating divide by 20 to get out of 5
                return Field == (int) IntFields.Rating ? value / 20 : value;
            }
        }

        /// <summary>
        /// Int Property B
        /// </summary>
        private int IntB
        {
            get
            {
                // get index
                int value = ByteToInt(Criteria.Skip(Offset + INTA + INTB).Take(4));
                // if value is rating divide by 20 to get out of 5
                return Field == (int) IntFields.Rating ? value / 20 : value;
            }
        }

        private int TimeMultiple => ByteToInt(Criteria.Skip(Offset + TIMEMULTIPLE).Take(4));

        private long TimeUnixA => ByteToUnix(Criteria.Skip(Offset + INTA).Take(4));

        private long TimeUnixB => ByteToUnix(Criteria.Skip(Offset + INTB).Take(4));

        private DateTime TimeA => UnixToDateTime(TimeUnixA);

        private DateTime TimeB => UnixToDateTime(TimeUnixB);

        #endregion

        #region Limit

        public bool Limit => Info[LIMITBOOL] == 1;

        public bool LimitChecked => Info[LIMITCHECKED] == 1;

        public int LimitNumber => ByteToInt(Info.Skip(LIMITINT).Take(4).ToArray());


        public LimitUnits LimitType => (LimitUnits) Convert.ToInt32(Info[LIMITMETHOD]);

        public SelectionMethods SelectionMethod => (SelectionMethods) Convert.ToInt32(Info[SELECTIONMETHOD]);

        public bool SelectionDesc => Info[SELECTIONMETHODSIGN] == 1;

        #endregion

        public bool LiveUpdate => Info[LIVEUPDATE] == 1;

        #region Parsing

        internal void ParseBytes()
        {
            // limited to a certain number of items?
            if (Limit && LimitType == LimitUnits.Items)
            {
                // put limit in query
                QuerySelect = $"SELECT TOP {LimitNumber} * FROM Library";
            }
            else
            {
                // select all
                QuerySelect = $"SELECT * FROM Library";
            }

            // perform logical matching?
            if (Match)
            {
                ProcessMatchRules();
            }

            // if limiting to checked items
            if (LimitChecked)
            {
                // if there was also match rules
                if (!string.IsNullOrEmpty(QueryWhere))
                {
                    // add a new line and "and" statement
                    QueryWhere += "\n AND";
                }

                // add checked criteria
                QueryWhere += "(Checked = 1)";
            }

            // limited to a certain number of items?
            if (Limit)
            {
                LimitItems();
            }

            // if limiting to checked items
            if (LimitChecked)
            {
                // if there is an output already
                if (!string.IsNullOrEmpty(Output))
                {
                    // add a new line first
                    Output += "\n";
                }

                Output += "Exclude unchecked items";
            }

            if (!LiveUpdate)
            {
                Output += "\nLive updating disabled";
            }
        }

        #region Match Rules

        private class StackItem
        {
            public readonly string Query;
            public readonly string Output;
            public int NumberOfSubQueries;
            public readonly string ConjQuery;
            public readonly string ConjOutput;
            public bool IsFirstItem;
            
            public StackItem(string query, string output, int numberOfSubQueries, string conjQuery, string conjOutput, bool isFirstItem)
            {
                Query = query;
                Output = output;
                NumberOfSubQueries = numberOfSubQueries;
                ConjQuery = conjQuery;
                ConjOutput = conjOutput;
                IsFirstItem = isFirstItem;
            }
        }

        private void ProcessMatchRules()
        {
            var subStack = new Stack<StackItem>();

            // is it an "or" or "and" general statement? (any vs all)
            if (LogicIsOr)
            {
                ConjQuery = "\n\tOR ";
                ConjOutput = " or\n";
            }
            else
            {
                ConjQuery = "\n\tAND ";
                ConjOutput = " and\n";
            }

            while (true)
            {
                // if there's anything left on the substack
                if (subStack.Count > 0)
                {
                    // if there is nothing left to read in the subexpression
                    if (subStack.Peek().NumberOfSubQueries == 0)
                    {
                        // pop the item, removing it from the list
                        StackItem old = subStack.Pop();

                        string conjQuery;
                        string conjOutput;
                        // if this is the first join
                        if (old.IsFirstItem)
                        {
                            // use no conjunction
                            conjQuery = "";
                            conjOutput = "";
                        }
                        else
                        {
                            // use the global one
                            conjQuery = old.ConjQuery;
                            conjOutput = old.ConjOutput;
                        }

                        // add subquery to the main query
                        QueryWhere = old.Query + conjQuery + $"( {QueryWhere} )";
                        Output = old.Output + conjOutput +
                                 $"(\n\t{string.Join("\n\t", Output.Split('\n'))}\n)";

                        ConjQuery = old.ConjQuery;
                        ConjOutput = old.ConjOutput;
                    }
                    else
                    {
                        // decrease the number of subexpressions
                        subStack.Peek().NumberOfSubQueries--;
                    }
                }

                if (!KeepReading)
                {
                    break;
                }

                // is this a string field
                if (Enum.IsDefined(typeof(StringFields), Field))
                {
                    ProcessStringField((StringFields) Field);
                }
                else if (Enum.IsDefined(typeof(IntFields), Field))
                {
                    ProcessIntField((IntFields) Field);
                }
                else if (Enum.IsDefined(typeof(DateFields), Field))
                {
                    ProcessDateField((DateFields) Field);
                }
                else if (Enum.IsDefined(typeof(BoolFields), Field))
                {
                    ProcessBoolField();
                }
                else if (Enum.IsDefined(typeof(MediaKindFields), Field))
                {
                    ProcessDictField(Kinds.Media, (MediaKindFields) Field);
                }
                else if (Enum.IsDefined(typeof(PlaylistFields), Field))
                {
                    throw new Exception($"Unhandled field code: {(PlaylistFields) Field}");
                }
                else if (Enum.IsDefined(typeof(LoveFields), Field))
                {
                    throw new Exception($"Unhandled field code: {(LoveFields) Field}");
                }
                else if (Enum.IsDefined(typeof(LocationFields), Field))
                {
                    ProcessDictField(Kinds.Location, (LocationFields) Field);
                }
                // if reached the statement 
                else if (Field == 0)
                {
                    // determine the number of subexpressions in the current statement
                    var numberOfSubExpression =
                        ByteToInt(Criteria.Skip(Offset + SUBINT).Take(4));

                    // save the current expression into the stack
                    subStack.Push(
                        new StackItem(QueryWhere, Output, numberOfSubExpression, ConjQuery, ConjOutput, Offset == FIELD)
                    );

                    // determine next statements and/or
                    if (SubLogicIsOr)
                    {
                        ConjQuery = " OR ";
                        ConjOutput = " or\n";
                    }
                    else
                    {
                        ConjQuery = " AND ";
                        ConjOutput = " and\n";
                    }

                    // clear current query
                    Output = QueryWhere = "";

                    // increase offset for next loop
                    Offset += SUBEXPRESSIONLENGTH;
                }
                else
                {
                    throw new Exception($"Unhandled field code: {Field}");
                }
            }
        }

        #region Process Fields

        /// <summary>
        /// Process a string field
        /// </summary>
        /// <param name="field"></param>
        private void ProcessStringField(StringFields field)
        {
            var fieldname = field.ToString();
            var workingOutput = fieldname;
            var workingQuery = $"([{fieldname}]";

            // does the query end have a % (wildcard)?
            var end = false;

            switch (LogicRule)
            {
                case LogicRule.Contains:
                    if (LogicSign == LogicSign.StringPositive)
                    {
                        workingOutput += $" contains ";
                        workingQuery += $" LIKE '%";
                    }
                    else
                    {
                        workingOutput += $" does not contain ";
                        workingQuery += $" NOT LIKE '%";
                    }

                    end = true;
                    break;
                case LogicRule.Is:
                    if (LogicSign == LogicSign.StringPositive)
                    {
                        workingOutput += $" is ";
                        workingQuery += $" = '";
                    }
                    else
                    {
                        workingOutput += $" is not ";
                        workingQuery += $" {Neq} '";
                    }

                    end = false;
                    break;
                case LogicRule.Starts:
                    workingOutput += $" starts with ";
                    workingQuery += $" LIKE '";
                    end = true;
                    break;
                case LogicRule.Ends:
                    workingOutput += $" ends with ";
                    workingQuery += $" LIKE '%";
                    end = true;
                    break;
                default:
                    throw new Exception($"Unsupported rule for {fieldname}: {LogicRule}");
            }

            // get remaining bytes of criteria from start of string field
            var remainingbytes = Criteria.Skip(Offset + STRING).ToArray();

            // if there are uneven remaining bytes
            if (remainingbytes.Length % 2 != 0)
            {
                // add a padding null byte
                // (helps with UTF16, as last character can be UTF8 if end of criteria)
                remainingbytes = remainingbytes.Concat(new byte[] {0}).ToArray();
            }

            // iTunes uses UTF16 encoding for the string. Convert all of the remaining bytes
            // to a string
            var content = Encoding.Unicode.GetString(remainingbytes);

            // the string should stop with either a null character or the end of the data.
            // try to find the first null character
            var stringEnd = content.IndexOf('\0');

            // if there are nulls
            if (stringEnd != -1)
            {
                // crop after first null. It's the end of the string.
                content = content.Substring(0, stringEnd);
            }

            workingOutput += $"\"{content}\" ";

            // if it's a file kind and the type is supported
            if (field == StringFields.Kind && Kinds.File.ContainsKey(content))
            {
                // clear working, we are about to replace it
                workingQuery = "";

                // specify a file name filter instead
                if (LogicSign == LogicSign.StringPositive)
                {
                    workingQuery += $"(lower(Uri)) LIKE '%{Kinds.File[content]}')";
                }
                else
                {
                    workingQuery += $"(lower(Uri)) NOT LIKE '%{Kinds.File[content]}')";
                }
            }
            // otherwise just add to the query like normal
            else
            {
                var qend = end ? "%" : "";
                workingQuery += content + qend + "')";
            }

            // add result to query and output
            AddToQuery(workingOutput, workingQuery);


            // if end found
            if (stringEnd != -1)
            {
                // adjust the offset for the next field
                Offset = Offset + STRING + 2 * stringEnd + 2;
            }
            else
            {
                // force end by going past end
                Offset = Criteria.Length;
            }
        }

        /// <summary>
        /// Process an integer field
        /// </summary>
        /// <param name="field"></param>
        private void ProcessIntField(IntFields field)
        {
            var fieldname = field.ToString();
            var workingOutput = fieldname;
            var workingQuery = $"([{fieldname}]";

            switch (LogicRule)
            {
                case LogicRule.Is:
                    if (LogicSign == LogicSign.IntPositive)
                    {
                        workingOutput += $" is {IntA}";
                        workingQuery += $" = {IntA}";
                    }
                    else
                    {
                        workingOutput += $" is not {IntA}";
                        workingQuery += $" {Neq} {IntA}";
                    }

                    break;
                case LogicRule.Greater:
                    workingOutput += $" is greater than {IntA}";
                    workingQuery += $" > {IntA}";
                    break;
                case LogicRule.Less:
                    workingOutput += $" is less than {IntA}";
                    workingQuery += $" < {IntA}";
                    break;
                case LogicRule.Other:
                    // if its a range statement
                    if (Criteria[Offset + LOGICSIGN + 2] == 1)
                    {
                        workingOutput += $" is between {IntA} and {IntB}";
                        if (UseBetween)
                        {
                            workingQuery += $" BETWEEN {IntA} AND {IntB}";
                        }
                        else
                        {
                            workingQuery += $" >= {IntA} AND [{fieldname}] <= {IntB}";
                        }
                    }
                    else
                    {
                        throw new Exception($"Unsupported rule for {fieldname}: {LogicRule}");
                    }

                    break;
            }

            workingQuery += ")";

            // add result to query and output
            AddToQuery(workingOutput, workingQuery);

            // offset past this rules's fields by going past intA's value
            Offset += INTA + INTLENGTH;
        }

        private void ProcessDateField(DateFields field)
        {
            var fieldName = field.ToString();
            var workingOutput = fieldName;
            var workingQuery = $"([{fieldName}]";

            switch (LogicRule)
            {
                case LogicRule.Greater:
                    workingOutput += $" is after {TimeA}";
                    workingQuery += $" > {TimeA}";
                    break;
                case LogicRule.Less:
                    workingOutput += $" is before {TimeA}";
                    workingQuery += $" < {TimeA}";
                    break;
                case LogicRule.Other:
                    // if its a range statement
                    if (Criteria[Offset + LOGICSIGN + 2] == 1)
                    {
                        if (LogicSign == LogicSign.IntPositive)
                        {
                            workingOutput += $" is between {TimeA} and {TimeB}";
                            if (UseBetween)
                            {
                                workingQuery += $" BETWEEN {TimeA} AND {TimeB}";
                            }
                            else
                            {
                                workingQuery += $" >= {TimeA} AND [{fieldName}] <= {TimeB}";
                            }
                        }
                        else
                        {
                            workingOutput += $" is not between {TimeA} and {TimeB}";
                            if (UseBetween)
                            {
                                workingQuery += $" NOT BETWEEN {TimeA} AND {TimeB}";
                            }
                            else
                            {
                                workingQuery += $" < {TimeA} AND [{fieldName}] > {TimeB}";
                            }
                        }
                    }
                    else if (Criteria[Offset + LOGICSIGN + 2] == 2)
                    {
                        if (LogicSign == LogicSign.IntPositive)
                        {
                            workingOutput += $" is in the last ";
                            workingQuery += $" (TIMESTAMP(NOW()) - TIMESTAMP({fieldName})) < ";
                        }
                        else
                        {
                            workingOutput += $" is not int the last ";
                            workingQuery += $" (TIMESTAMP(NOW()) - TIMESTAMP({fieldName})) > ";
                        }
                    }
                    else
                    {
                        throw new Exception($"Unsupported rule for {fieldName}: {LogicRule}");
                    }

                    // determine the number of the given time unit
                    // (I have no idea why this needs two's complement or + 1)
                    var t = (int) ((ByteToInt(
                        Criteria.Skip(Offset + TIMEVALUE).Take(4)
                            // find two's complement for each byte
                            .Select(b => (byte) ~b)
                    ) + 1) % 4294967296);

                    // invalid time interval
                    if (!Enum.IsDefined(typeof(DateDiffUnits), TimeMultiple))
                    {
                        throw new Exception($"Unsupported timespan for {fieldName}: {t * TimeMultiple} seconds");
                    }

                    // determine units
                    var units = (DateDiffUnits) TimeMultiple;

                    // number of seconds
                    workingQuery += t * TimeMultiple;

                    // state size and units
                    workingOutput += $"{t} {units}";

                    break;
            }

            workingQuery += ")";

            // add result to query and output
            AddToQuery(workingOutput, workingQuery);

            // offset past this rules fields by going past intA's value
            Offset += INTA + INTLENGTH;
        }

        private static void ProcessBoolField()
        {
        }

        private void ProcessDictField(Dictionary<int, string> dict, Enum field)
        {
            var fieldName = Enum.GetName(field.GetType(), field);
            var workingOutput = fieldName;
            var workingQuery = $"([{fieldName}]";

            // equality or or misc comparison
            if (LogicRule == LogicRule.Is || LogicRule == LogicRule.Other)
            {
                // invalid comparison
                if (LogicRule == LogicRule.Other && IntA != IntB)
                {
                    throw new Exception(
                        $"Invalid logic for {fieldName}: {dict[IntA]} {Neq} {dict[IntB]}"
                    );
                }

                // is or is not? Get value using dictionary for that type and add to string
                if (LogicSign == LogicSign.IntPositive)
                {
                    workingOutput += $" is {dict[IntA]}";
                    workingQuery += $" = '{dict[IntA]}'";
                }
                else
                {
                    workingOutput += $" is not {dict[IntA]}";
                    workingQuery += $" {Neq} '{dict[IntA]}'";
                }
            }
            else
            {
                throw new Exception($"Unsupported rule for {fieldName}: {LogicRule}");
            }

            workingQuery += ")";

            // add result to query and output
            AddToQuery(workingOutput, workingQuery);

            // offset past this rules's fields by going past intA's value
            Offset += INTA + INTLENGTH;
        }

        #endregion

        #endregion

        #region Limit

        private void LimitItems()
        {
            string outputOrder = "";
            string queryOrder = "";

            switch (SelectionMethod)
            {
                case SelectionMethods.Random:
                    outputOrder = "Random";
                    queryOrder = "RANDOM()";
                    break;
                case SelectionMethods.Name:
                    outputOrder = "Name";
                    queryOrder = "[Name] ASC";
                    break;
                case SelectionMethods.Album:
                    outputOrder = "Album";
                    queryOrder = "[Album] ASC";
                    break;
                case SelectionMethods.Artist:
                    outputOrder = "Artist";
                    queryOrder = "[Artist] ASC";
                    break;
                case SelectionMethods.Genre:
                    outputOrder = "Genre";
                    queryOrder = "[Genre] ASC";
                    break;
                case SelectionMethods.HighestRating:
                    outputOrder = "highest rated";
                    queryOrder = "[Rating] DESC";
                    break;
                case SelectionMethods.LowestRating:
                    outputOrder = "lowest rated";
                    queryOrder = "[Rating] ASC";
                    break;
                case SelectionMethods.RecentlyPlayed:
                    if (SelectionDesc)
                    {
                        outputOrder = "most recently played";
                        queryOrder = "[LastPlayed] DESC";
                    }
                    else
                    {
                        outputOrder = "least recently played";
                        queryOrder = "[LastPlayed] ASC";
                    }

                    break;
                case SelectionMethods.OftenPlayed:
                    if (SelectionDesc)
                    {
                        outputOrder = "most played";
                        queryOrder = "[Plays] DESC";
                    }
                    else
                    {
                        outputOrder = "least played";
                        queryOrder = "[Plays] ASC";
                    }

                    break;
                case SelectionMethods.RecentlyAdded:
                    if (SelectionDesc)
                    {
                        outputOrder = "most recently added";
                        queryOrder = "[DateAdded] DESC";
                    }
                    else
                    {
                        outputOrder = "least recently added";
                        queryOrder = "[DateAdded] ASC";
                    }

                    break;
            }

            // if there is an output already
            if (!string.IsNullOrEmpty(Output))
            {
                // add a new line first
                Output += "\n";
            }

            Output += $"Limited to {LimitNumber} {LimitType} selected by {outputOrder}";
            QueryOrder += $"ORDER BY {queryOrder}";
        }

        #endregion

        private void AddToQuery(string workingOutput, string workingQuery)
        {
            // if there is already a comparison in the output
            if (!string.IsNullOrEmpty(Output))
            {
                // add a conjunction first
                Output += ConjOutput;
            }

            // then add to the output
            Output += workingOutput;

            // if there is already a comparison in the query
            if (!string.IsNullOrEmpty(QueryWhere))
            {
                // add a conjunction first
                QueryWhere += ConjQuery;
            }

            // then add to the query
            QueryWhere += workingQuery;
        }

        #endregion
    }

    /// <summary>
    /// Convert bytes to integer.
    /// iTunes uses Big Endian encoding for its integers (reversed bytes), so bitconverter needs the array reversed
    /// if the converter itself is little endian (usually yes)
    /// </summary>
    /// <returns></returns>
    private static int ByteToInt(IEnumerable<byte> bytes) =>
        BitConverter.ToInt32(BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes.ToArray(), 0);

    private static long ByteToUnix(IEnumerable<byte> byteArray)
    {
        return ByteToInt(byteArray) + UNIXDELTA;
    }

    /// <summary>
    /// Convert Unix time value to a DateTime object.
    /// </summary>
    /// <param name="unixtime">The Unix time stamp you want to convert to DateTime.</param>
    /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
    public static DateTime UnixToDateTime(long unixtime)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        return dtDateTime;
    }

    private enum DateDiffUnits
    {
        Days = 86400,
        Weeks = 604800,
        Months = 2628000
    }

    const int UNIXDELTA = -2082844800; // iTunes/Unix time stamp 0 difference

    const int INTLENGTH = 67; // The length on a int criteria starting at the first int
    const int SUBEXPRESSIONLENGTH = 192; // The length of a subexpression starting from FIELD

// INFO OFFSETS
// Offsets for bytes which...
    const int LIVEUPDATE = 0; // determine whether live updating is enabled - Absolute offset
    const int MATCHBOOL = 1; // determine whether logical matching is to be performed - Absolute offset
    const int LIMITBOOL = 2; // determine whether results are limited - Absolute offset
    const int LIMITMETHOD = 3; // determine by what criteria the results are limited - Absolute offset
    const int SELECTIONMETHOD = 7; // determine by what criteria limited playlists are populated - Absolute offset
    const int LIMITINT = 8; // determine the limited - Absolute offset
    const int LIMITCHECKED = 12; // determine whether to exclude unchecked items - Absolute offset

    const int
        SELECTIONMETHODSIGN = 13; // determine whether certain selection methods are "most" or "least" - Absolute offset

// CRITERIA OFFSETS
// Offsets for bytes which...
    const int LOGICTYPE = 15; // determine whether all or any criteria must match - Absolute offset
    const int FIELD = 139; // determine what is being matched (Artist, Album, &c) - Absolute offset

    const int
        LOGICSIGN = 1; // determine whether the matching rule is positive or negative (e.g., is vs. is not) - Relative offset from FIELD

    const int LOGICRULE = 4; // determine the kind of logic used (is, contains, begins, &c) - Relative offset from FIELD
    const int STRING = 54; // begin string data - Relative offset from FIELD
    const int INTA = 57; // begin the first int - Relative offset from FIELD
    const int INTB = 24; // begin the second int - Relative offset from INTA
    const int TIMEMULTIPLE = 73; // begin the int with the multiple of time - Relative offset from FIELD
    const int TIMEVALUE = 65; // begin the inverse int with the value of time - Relative offset from FIELD
    const int SUBLOGICTYPE = 68; // determine whether all or any criteria must match - Relative offset from FIELD
    const int SUBINT = 61; // begin the first int - Relative offset from FIELD
}