using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Playlists;

public class PlayListInfoBuilder
{
    public class PlaylistInformation
    {
        public Limit Limit { get; set; }

        public Conjunction? RuleConjunction { get; set; }

        public bool LiveUpdating { get; set; }
    }

    public class Limit
    {
        public bool IsLimited { get; set; }
        public LimitUnits? Units { get; set; }
        public int? Amount { get; set; }
        public bool OnlyChecked { get; set; }
        public SelectionMethods SortBy { get; set; }
        public bool SortByDescending { get; set; }
    }

    public PlaylistInformation ParsePlaylist(byte[] info, byte[] criteria)
    {
        var infoHelper = new PlayListInfoHelper(info);

        var limit = new Limit()
        {
            IsLimited = infoHelper.IsLimited,
            Units = infoHelper.LimitUnits,
            Amount = infoHelper.LimitNumber,
            OnlyChecked = infoHelper.OnlyChecked,
            SortBy = infoHelper.SortBy,
            SortByDescending = infoHelper.SortDescending
        };

        // perform logical matching
        var conjunctions = infoHelper.Match ? ProcessMatchRules(criteria) : null;

        return new PlaylistInformation
        {
            Limit = limit,
            RuleConjunction = conjunctions,
            LiveUpdating = infoHelper.LiveUpdate
        };
    }

    private class StackItem
    {
        public int NumberOfSubConjunctions;
        public readonly Conjunction Conjunction;

        public StackItem(int numberOfSubConjunctions, Conjunction conjunction)
        {
            NumberOfSubConjunctions = numberOfSubConjunctions;
            Conjunction = conjunction;
        }
    }

    public class Conjunction
    {
        public ConjunctionType Type { get; set; }

        public IList<Conjunction> SubConjunctions { get; set; }

        public IList<IRule> Rules { get; set; }
    }

    public interface IRule
    {
        public LogicRule RuleType { get; set; }

        public LogicSign Sign { get; set; }
    }

    public enum ConjunctionType
    {
        And,
        Or
    }

    private class StringRule : IRule
    {
        public StringFields Field { get; set; }
        public LogicRule RuleType { get; set; }

        public LogicSign Sign { get; set; }

        public string Value { get; set; }
    }

    private class IntRule : IRule
    {
        public IntFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
        public long ValueA { get; set; }
        public long? ValueB { get; set; }
    }

    private class DateRule : IRule
    {
        public DateFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
        public DateTime ValueA { get; set; }
        public DateTime? ValueB { get; set; }
    }

    private class TimeSpanRule : IRule
    {
        public DateFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
        public TimeSpan Value { get; set; }
    }

    private class BooleanRule : IRule
    {
        public BoolFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
    }

    private class DictionaryRule : IRule
    {
        public DictionaryFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
        public string Value { get; set; }
    }

    private class PlaylistRule : IRule
    {
        public PlaylistFields Field { get; set; }
        public LogicRule RuleType { get; set; }
        public LogicSign Sign { get; set; }
        public string Value { get; set; }
    }

    private Conjunction ProcessMatchRules(byte[] criteria)
    {
        var criteriaHelper = new PlaylistCriteriaHelper(criteria);

        var offset = PlaylistCriteriaHelper.StartingOffset;

        // is it an "or" (any) or "and" (all) general statement?
        var mainConjunction = new Conjunction()
        {
            Type = criteriaHelper.LogicIsOr ? ConjunctionType.Or : ConjunctionType.And,
            SubConjunctions = new List<Conjunction>(),
            Rules = new List<IRule>()
        };

        var subStack = new Stack<StackItem>();

        var currentConjunction = mainConjunction;
        
        while (true)
        {
            // if there's anything left on the substack
            if (subStack.Count > 0)
            {
                // if there is nothing left to read in the subexpression
                if (subStack.Peek().NumberOfSubConjunctions == 0)
                {
                    subStack.Pop();

                    // move onto the next one
                    currentConjunction = subStack.Peek().Conjunction;
                }
                else
                {
                    // decrease the number of subexpressions
                    subStack.Peek().NumberOfSubConjunctions--;
                }
            }

            // if you're are the end of the criteria, end
            if (offset >= criteria.Length)
            {
                break;
            }

            var field = criteriaHelper.Field(offset);

            var logicRule = criteriaHelper.GetRule(offset);

            var logicSign = criteriaHelper.GetSign(offset);

            IRule? rule = null;

            // is this a string field
            if (Enum.IsDefined(typeof(StringFields), field))
            {
                var content = criteriaHelper.ReadStringContent(offset);

                rule = new StringRule
                {
                    Field = (StringFields) field,
                    RuleType = logicRule,
                    Sign = logicSign,
                    Value = content
                };

                offset = PlaylistCriteriaHelper.PostStringOffset(offset, content);
            }
            else if (Enum.IsDefined(typeof(IntFields), field))
            {
                rule = new IntRule
                {
                    Field = (IntFields) field,
                    RuleType = logicRule,
                    Sign = logicSign,
                    ValueA = criteriaHelper.IntA(offset),
                    // Only range rule has two values.
                    ValueB = criteriaHelper.IsRangeField(offset) ? criteriaHelper.IntB(offset) : null
                };

                offset = PlaylistCriteriaHelper.PostFixedLengthFieldOffset(offset);
            }
            else if (Enum.IsDefined(typeof(DateFields), field))
            {
                if (criteriaHelper.IsTimeSpanRule(offset))
                {
                    rule = new TimeSpanRule
                    {
                        Field = (DateFields) field,
                        RuleType = LogicRule.Other,
                        Sign = logicSign,
                        Value = TimeSpan.FromSeconds(
                            (int) criteriaHelper.TimeUnits(offset) * criteriaHelper.TimeSpanValue(offset))
                    };
                }
                else
                {
                    rule = new DateRule
                    {
                        Field = (DateFields) field,
                        RuleType = logicRule,
                        Sign = logicSign,
                        ValueA = criteriaHelper.DateA(offset),
                        // Only range rule has two values.
                        ValueB = criteriaHelper.IsRangeField(offset) ? criteriaHelper.DateB(offset) : null
                    };

                    offset = PlaylistCriteriaHelper.PostFixedLengthFieldOffset(offset);
                }
            }
            else if (Enum.IsDefined(typeof(BoolFields), field))
            {
                rule = new BooleanRule
                {
                    Field = (BoolFields) field,
                    RuleType = LogicRule.Is,
                    // when sign is IntPositive it means true
                    Sign = logicSign
                };

                offset = PlaylistCriteriaHelper.PostFixedLengthFieldOffset(offset);
            }
            else if (Enum.IsDefined(typeof(DictionaryFields), field))
            {
                if (logicRule != LogicRule.Is)
                {
                    throw new Exception(
                        $"{(DictionaryFields) field} had incompatible rule type ${logicRule}");
                }

                var dictionary = (DictionaryFields) field switch
                {
                    DictionaryFields.Love => Kinds.Love,
                    DictionaryFields.MediaKind => Kinds.Media,
                    DictionaryFields.Location => Kinds.Location,
                    DictionaryFields.iCloudStatus => Kinds.iCloudStatus,
                    _ => throw new ArgumentOutOfRangeException(
                        $"DictionaryField not recognized: ${(DictionaryFields) field}")
                };

                rule = new DictionaryRule
                {
                    Field = (DictionaryFields) field,
                    RuleType = logicRule,
                    Sign = logicSign,
                    Value = dictionary[criteriaHelper.IntA(offset)]
                };

                offset = PlaylistCriteriaHelper.PostFixedLengthFieldOffset(offset);
            }
            else if (Enum.IsDefined(typeof(PlaylistFields), field))
            {
                rule = new PlaylistRule
                {
                    Field = (PlaylistFields) field,
                    RuleType = logicRule,
                    Sign = logicSign,
                    Value = criteriaHelper.PlaylistId(offset)
                };

                offset = PlaylistCriteriaHelper.PostFixedLengthFieldOffset(offset);
            }

            if (rule != null)
            {
                currentConjunction.Rules.Add(rule);
            }
            
            if (!criteriaHelper.KeepReading(offset))
            {
                break;
            }

            // if field is null, reached the statement 
            if (field == 0)
            {
                // determine the number of subexpressions in the current statement
                var numberOfSubExpressions = criteriaHelper.NumberOfSubExpressions(offset);

                var subExpressionConjunctionType =
                    criteriaHelper.SubLogicIsOr(offset) ? ConjunctionType.Or : ConjunctionType.And;

                var childConjunction = new Conjunction()
                {
                    Type = subExpressionConjunctionType,
                    SubConjunctions = new List<Conjunction>(),
                    Rules = new List<IRule>()
                };
                
                currentConjunction.SubConjunctions.Add(childConjunction);
                
                subStack.Push(new StackItem(numberOfSubExpressions, childConjunction));

                // interrupt current conjunction
                currentConjunction = childConjunction;

                // increase offset for next loop
                offset = PlaylistCriteriaHelper.PostSubExpressionOffset(offset);
            }
            else
            {
                throw new Exception($"Unhandled field code: {field}");
            }
        }

        return mainConjunction;
    }
}