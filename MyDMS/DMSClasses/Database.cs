namespace DMSClasses;

public sealed class Database
{
    private readonly List<Table> _tables = new List<Table>();
    
    public Database(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name should not be empty string");
        }
        Name = name;
    }

    public string Name { get; }
    public IEnumerable<Table> Tables => _tables;

    public void AddTable(Table table)
    {
        if (Tables.Any(x => x.Name == table.Name))
        {
            throw new ArgumentException("Table with such name already exists");
        }

        _tables.Add(table);
    }

    public void RemoveTable(Table table)
    {
        if (_tables.All(x => x.Name != table.Name))
        {
            throw new InvalidOperationException("Database do not have such table");
        }

        var tableToRemove = _tables.Single(x => x.Name == table.Name);

        _tables.Remove(tableToRemove);
    } 
}