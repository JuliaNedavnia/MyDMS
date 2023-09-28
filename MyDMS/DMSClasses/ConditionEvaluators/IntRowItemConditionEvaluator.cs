namespace DMSClasses.ConditionEvaluators;

public sealed class IntRowItemConditionEvaluator : RowItemConditionEvaluator
{
    public IntRowItemConditionEvaluator(RowItem rowItem) : base(rowItem) { }

    public override bool Equal(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return (int)RowItemForCondition.Value == (int)value;
    }

    public override bool GreaterThan(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToInt32(RowItemForCondition.Value) > Convert.ToInt32(value);
    }

    public override bool LessThan(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToInt32(RowItemForCondition.Value) < Convert.ToInt32(value);
    }

    public override bool GreaterThanOrEqual(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToInt32(RowItemForCondition.Value) >= Convert.ToInt32(value);
    }

    public override bool LessThanOrEqual(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToInt32(RowItemForCondition.Value) <= Convert.ToInt32(value);
    }

    public override bool Like(object value)
    {
        throw new InvalidOperationException("Operation like is not appropriate to row values from int columns ");
    }

    protected override void ThrowConditionValueNotOfRightType(object value)
    {
        if (!int.TryParse(value.ToString(), out _))
        {
            throw new ArgumentException(@"Condition value is not of type int");
        }
    }
}