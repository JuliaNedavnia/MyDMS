namespace DMSClasses.ConditionEvaluators;

public abstract class RowItemConditionEvaluator
{
    protected RowItemConditionEvaluator(RowItem rowItem)
    {
        RowItemForCondition = rowItem;
    }
    
    protected RowItem RowItemForCondition { get; init; }

    public abstract bool Equal(object value);
    public abstract bool GreaterThan(object value);
    public abstract bool LessThan(object value);
    public abstract bool GreaterThanOrEqual(object value);
    public abstract bool LessThanOrEqual(object value);
    public abstract bool Like(object value);
    protected abstract void ThrowConditionValueNotOfRightType(object value);
}