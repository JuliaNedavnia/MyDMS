namespace DMSClasses.ConditionEvaluators;

public sealed class ConditionEvaluatorFactory
{
    public RowItemConditionEvaluator GetConditionEvaluator(RowItem rowItem)
    {
        switch (rowItem.Column.Type)
        {
            case ColumnType.Integer:
                return new IntRowItemConditionEvaluator(rowItem);
            case ColumnType.Real:
                return new RealRowItemConditionEvaluator(rowItem);
            case ColumnType.Char:
                return new CharRowItemConditionEvaluator(rowItem);
            case ColumnType.String:
                return new StringRowItemConditionEvaluator(rowItem);
            case ColumnType.StringInvl:
                return new StringInvlRowItemConditionEvaluator(rowItem);
            case ColumnType.Html:
                return new HtmlRowItemConditionEvaluator(rowItem);
            default:
                throw new ArgumentException("Do not have such column type");
        }
    }
}