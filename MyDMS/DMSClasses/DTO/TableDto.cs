namespace DMSClasses.DTO;

internal class TableDto
{
    public string Name { get; set; }
    public List<Column> Columns { get; set; }
    public List<Row> Rows { get; set; }
}
