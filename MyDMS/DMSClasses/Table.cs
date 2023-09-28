using System.Data.Common;

namespace DMSClasses;

public sealed class Table
{
    private readonly List<Row> _rows;

    public Table(string tableName, List<Column> tableColumns)
    {
        Name = tableName;
        ThrowIfColumnsWithEqualNames(tableColumns);
        if (!tableColumns.Any())
        {
            throw new ArgumentException("Can not create table without columns");
        }
        Columns = tableColumns;
        _rows = new List<Row>();
    }

    public string Name { get; }
    public IEnumerable<Column> Columns { get; init; }
    public IEnumerable<Row> Rows => _rows;

    public void AddRow(object[] rowValues)
    {
        if (rowValues.Length > Columns.Count())
        {
            throw new ArgumentException("Cells count is greater than column count");
        }

        var rowValuesWithColumn = new List<RowItem>();
        for (int i = 0; i < Columns.Count(); i++)
        {
            rowValuesWithColumn.Add(new RowItem(Columns.ElementAt(i), rowValues[i]));
        }
        
        _rows.Add(new Row(rowValuesWithColumn));
    }

    public void AddRangeOfRows(IEnumerable<Row> rows) => _rows.AddRange(rows);

    public void RemoveAllRows() => _rows.Clear();

    private void ThrowIfColumnsWithEqualNames(List<Column> columns)
    {
        bool hasDuplicateColumnNames = columns
                                       .GroupBy(col => col.Name)
                                       .Any(group => group.Count() > 1);
        if (hasDuplicateColumnNames)
        {
            throw new ArgumentException("There are columns with the same name");
        }
    }
}
