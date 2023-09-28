namespace DMSClasses.ConditionEvaluators;

public sealed class RealRowItemConditionEvaluator : RowItemConditionEvaluator
{
    public RealRowItemConditionEvaluator(RowItem rowItem) : base(rowItem) { }

    public override bool Equal(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToDouble(RowItemForCondition.Value).Equals(Convert.ToDouble(value));
    }

    public override bool GreaterThan(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToDouble(RowItemForCondition.Value) > Convert.ToDouble(value);
    }

    public override bool LessThan(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToDouble(RowItemForCondition.Value) < Convert.ToDouble(value);
    }

    public override bool GreaterThanOrEqual(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToDouble(RowItemForCondition.Value) >= Convert.ToDouble(value);
    }

    public override bool LessThanOrEqual(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return Convert.ToDouble(RowItemForCondition.Value) <= Convert.ToDouble(value);
    }

    public override bool Like(object value)
    {
        throw new InvalidOperationException("Operation like is not appropriate to row values from real columns ");
    }

    protected override void ThrowConditionValueNotOfRightType(object value)
    {
        if (!float.TryParse(value.ToString(), out _))
        {
            throw new ArgumentException(@"Condition value is not of type real");
        }
    }
}