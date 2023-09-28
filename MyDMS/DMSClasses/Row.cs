namespace DMSClasses;

public sealed class Row
{
    public Row(List<RowItem> items)
    {
        Items = items;
    }

    public List<RowItem> Items { get; init; }
}
