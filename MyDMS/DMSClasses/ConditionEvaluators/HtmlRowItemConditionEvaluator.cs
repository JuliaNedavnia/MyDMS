using System.Text.RegularExpressions;

namespace DMSClasses.ConditionEvaluators
{
    internal class HtmlRowItemConditionEvaluator : RowItemConditionEvaluator
    {
        private Column _column;

        public HtmlRowItemConditionEvaluator(RowItem rowItem) : base(rowItem)
        {
            _column = rowItem.Column;
        }

        public override bool Equal(object value)
        {
            ThrowConditionValueNotOfRightType(value);
            return (string)RowItemForCondition.Value == (string)value;
        }

        public override bool GreaterThan(object value)
        {
            throw new InvalidOperationException("Operation > is not appropriate to row values from html columns ");
        }

        public override bool LessThan(object value)
        {
            throw new InvalidOperationException("Operation < is not appropriate to row values from html columns ");
        }

        public override bool GreaterThanOrEqual(object value)
        {
            throw new InvalidOperationException("Operation >= is not appropriate to row values from html columns ");
        }

        public override bool LessThanOrEqual(object value)
        {
            throw new InvalidOperationException("Operation <= is not appropriate to row values from html columns ");
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
                var rowValue = RowItemForCondition.Value.ToString()!;
                int lastDotIndex = rowValue.LastIndexOf(".");
                rowValue = rowValue.Substring(0, lastDotIndex);

                if (conditionInString.StartsWith('%'))
                {
                    result = rowValue.EndsWith(stringToCompare);
                }
                else if (conditionInString.EndsWith('%'))
                {
                    result = rowValue.StartsWith(stringToCompare);
                }
                else if ((conditionInString.EndsWith('%') && conditionInString.StartsWith('%'))
                         || (!conditionInString.EndsWith('%') && !conditionInString.StartsWith('%')))
                {
                    result = rowValue.Equals(stringToCompare);
                }
                return result;
            }
            throw new ArgumentException("Wrong like expression");
        }

        protected override void ThrowConditionValueNotOfRightType(object value)
        {
        }
    }
}
