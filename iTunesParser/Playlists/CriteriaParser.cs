using iTunesSmartParser.Data;
using iTunesSmartParser.Fields;

namespace iTunesSmartParser.Playlists;

public static class CriteriaParser
{
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

    public static Conjunction Parse(byte[] criteria)
    {
        var criteriaHelper = new CriteriaHelper(criteria);

        var offset = CriteriaHelper.StartingOffset;

        // is it an "or" (any) or "and" (all) general statement?
        var mainConjunction = new Conjunction(Type: criteriaHelper.LogicIsOr ? ConjunctionType.Or : ConjunctionType.And,
            SubConjunctions: new List<Conjunction>(), Rules: new List<IRule>());

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

                    // if there are more subconjunctions to handle
                    if (subStack.Count > 0)
                    {
                        // move onto the next one
                        currentConjunction = subStack.Peek().Conjunction;
                    }
                    else
                    {
                        // move back up to the main conjunction
                        currentConjunction = mainConjunction;
                    }
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

                rule = new StringRule(Field: (StringFields) field, RuleType: logicRule, Sign: logicSign,
                    Value: content);

                offset = CriteriaHelper.PostStringOffset(offset, content);
            }
            else if (Enum.IsDefined(typeof(IntFields), field))
            {
                rule = new IntRule(Field: (IntFields) field, RuleType: logicRule, Sign: logicSign,
                    ValueA: criteriaHelper.IntA(offset), // Only range rule has two values.
                    ValueB: criteriaHelper.IsRangeField(offset) ? criteriaHelper.IntB(offset) : null);

                offset = CriteriaHelper.PostFixedLengthFieldOffset(offset);
            }
            else if (Enum.IsDefined(typeof(DateFields), field))
            {
                if (criteriaHelper.IsTimeSpanRule(offset))
                {
                    rule = new TimeSpanRule(Field: (DateFields) field, RuleType: LogicRule.Other, Sign: logicSign,
                        Value: TimeSpan.FromSeconds(
                            (int) criteriaHelper.TimeUnits(offset) * criteriaHelper.TimeSpanValue(offset)));
                }
                else
                {
                    rule = new DateRule(Field: (DateFields) field, RuleType: logicRule, Sign: logicSign,
                        ValueA: criteriaHelper.DateA(offset), // Only range rule has two values.
                        ValueB: criteriaHelper.IsRangeField(offset) ? criteriaHelper.DateB(offset) : null);

                    offset = CriteriaHelper.PostFixedLengthFieldOffset(offset);
                }
            }
            else if (Enum.IsDefined(typeof(BoolFields), field))
            {
                rule = new BooleanRule(Field: (BoolFields) field,
                    RuleType: LogicRule.Is, // when sign is IntPositive it means true
                    Sign: logicSign);

                offset = CriteriaHelper.PostFixedLengthFieldOffset(offset);
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
                    DictionaryFields.Love => Dictionaries.Love,
                    DictionaryFields.MediaKind => Dictionaries.Media,
                    DictionaryFields.Location => Dictionaries.Location,
                    DictionaryFields.iCloudStatus => Dictionaries.iCloudStatus,
                    _ => throw new ArgumentOutOfRangeException(
                        $"DictionaryField not recognized: ${(DictionaryFields) field}")
                };

                rule = new DictionaryRule(Field: (DictionaryFields) field, RuleType: logicRule, Sign: logicSign,
                    Value: dictionary[criteriaHelper.IntA(offset)]);

                offset = CriteriaHelper.PostFixedLengthFieldOffset(offset);
            }
            else if (Enum.IsDefined(typeof(PlaylistFields), field))
            {
                rule = new PlaylistRule(Field: (PlaylistFields) field, RuleType: logicRule, Sign: logicSign,
                    Value: criteriaHelper.PlaylistId(offset));

                offset = CriteriaHelper.PostFixedLengthFieldOffset(offset);
            }

            if (rule != null)
            {
                currentConjunction.Rules.Add(rule);

                continue;
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

                var childConjunction = new Conjunction(Type: subExpressionConjunctionType,
                    SubConjunctions: new List<Conjunction>(), Rules: new List<IRule>());

                currentConjunction.SubConjunctions.Add(childConjunction);

                subStack.Push(new StackItem(numberOfSubExpressions, childConjunction));

                // interrupt current conjunction
                currentConjunction = childConjunction;

                // increase offset for next loop
                offset = CriteriaHelper.PostSubExpressionOffset(offset);
            }
            else
            {
                throw new Exception($"Unhandled field code: {field}");
            }
        }

        return mainConjunction;
    }
}