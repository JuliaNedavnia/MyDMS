namespace DMSClasses;
public sealed class RowItem
{
    public RowItem(Column column, object value)
    {
        if (!column.IsValueValidForColumn(value))
        {
            throw new ArgumentException($"Value should be of type {Enum.GetName(typeof(ColumnType), column.Type)}");
        }
        Value = value;
        Column = column;
    }

    public object Value { get; }
    public Column Column { get; }
}
