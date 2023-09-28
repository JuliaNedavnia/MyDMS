using DMSClasses.Enums;
using DMSClasses.Parsers;
using System.Text.RegularExpressions;

namespace DMSClasses.ConditionEvaluators;

public sealed class StringRowItemConditionEvaluator : RowItemConditionEvaluator
{
    public StringRowItemConditionEvaluator(RowItem rowItem) : base(rowItem) { }

    public override bool Equal(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return (string)RowItemForCondition.Value == (string)value;
    }

    public override bool GreaterThan(object value)
    {
        throw new InvalidOperationException("Operation > is not appropriate to row values from char columns ");
    }

    public override bool LessThan(object value)
    {
        throw new InvalidOperationException("Operation < is not appropriate to row values from char columns ");
    }

    public override bool GreaterThanOrEqual(object value)
    {
        throw new InvalidOperationException("Operation >= is not appropriate to row values from char columns ");
    }

    public override bool LessThanOrEqual(object value)
    {
        throw new InvalidOperationException("Operation <= is not appropriate to row values from char columns ");
    }

    public override bool Like(object value)
    {
        var regexForLike = @"^%?(\w+)%?$";
        if (string.IsNullOrEmpty(value.ToString()))
        {
            throw new ArgumentException("Wrong value for like expression");
        }

        string conditionInString = value.ToString()!;

        Match match = Regex.Match(conditionInString, regexForLike);

        if (match.Success)
        {
            bool result = false;
            var stringToCompare = match.Groups[1].Value;
            if (conditionInString.StartsWith('%'))
            {
                result = RowItemForCondition.Value.ToString()!.EndsWith(stringToCompare);
            }
            else if(conditionInString.EndsWith('%'))
            {
                result = RowItemForCondition.Value.ToString()!.StartsWith(stringToCompare);
            }
            else if ((conditionInString.EndsWith('%') && conditionInString.StartsWith('%')) 
                     || (!conditionInString.EndsWith('%') && !conditionInString.StartsWith('%')))
            {
                result = RowItemForCondition.Value.ToString()!.Equals(stringToCompare);
            }
            return result;
        }
        throw new ArgumentException("Wrong like expression");
    }

    protected override void ThrowConditionValueNotOfRightType(object value) { }
}