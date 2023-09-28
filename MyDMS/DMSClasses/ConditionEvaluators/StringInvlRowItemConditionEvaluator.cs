namespace DMSClasses.ConditionEvaluators;

internal class StringInvlRowItemConditionEvaluator : RowItemConditionEvaluator
{
    private Column _column;

    public StringInvlRowItemConditionEvaluator(RowItem rowItem) : base(rowItem)
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
        throw new InvalidOperationException("Operation > is not appropriate to row values from stringInvl columns ");
    }

    public override bool LessThan(object value)
    {
        throw new InvalidOperationException("Operation < is not appropriate to row values from stringInvl columns ");
    }

    public override bool GreaterThanOrEqual(object value)
    {
        throw new InvalidOperationException("Operation >= is not appropriate to row values from stringInvl columns ");
    }

    public override bool LessThanOrEqual(object value)
    {
        throw new InvalidOperationException("Operation <= is not appropriate to row values from stringInvl columns ");
    }

    public override bool Like(object value)
    {
        throw new InvalidOperationException("Operation LIKE is not appropriate to row values from stringInvl columns ");
    }

    protected override void ThrowConditionValueNotOfRightType(object value)
    { }
}
