namespace DMSClasses.ConditionEvaluators;

public sealed class CharRowItemConditionEvaluator : RowItemConditionEvaluator
{
    public CharRowItemConditionEvaluator(RowItem rowItem) : base(rowItem) { }

    public override bool Equal(object value)
    {
        ThrowConditionValueNotOfRightType(value);
        return RowItemForCondition.Value.ToString() == value.ToString();
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
        throw new InvalidOperationException("Operation like is not appropriate to row values from char columns ");
    }

    protected override void ThrowConditionValueNotOfRightType(object value)
    {
        if (!char.TryParse(value.ToString(), out _))
        {
            throw new ArgumentException(@"Condition value is not of type char");
        }
    }
}