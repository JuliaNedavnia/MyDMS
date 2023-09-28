namespace DMSClasses;

internal static class RowItemFactory
{
    public static RowItem CreateRowItem(Column column, object value)
    {
        switch (column.Type)
        {
            case ColumnType.String:
                return new StringRowValue(column, value);
            case ColumnType.Char:
                return new CharRowValue(column, value);
            case ColumnType.Integer:
                return new IntRowValue(column, value);
            case ColumnType.Real:
                return new RealRowValue(column, value);
            default:
                throw new ArgumentException("Such type for column does not exist");
        }
    }
}
