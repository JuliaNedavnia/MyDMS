using System.Text.Json;
using DMSClasses.DTO;

namespace DMSClasses;

public static class DatabaseJsonConverter
{
    public static void SaveDatabaseTo(string path, Database database)
    {
        var databaseDto = new DatabaseDto
        {
            Name = database.Name,
            Tables = database.Tables.Select(table => 
                new TableDto
                {
                    Columns = table.Columns.ToList(),
                    Name = table.Name, 
                    Rows = table.Rows.ToList()
                }).ToList()
        };
        string jsonDatabase = SerializeDatabaseToJson(databaseDto);
        File.WriteAllText(Path.Combine(path,$"{database.Name}.json"), jsonDatabase);
    }

    public static Database GetDatabaseFrom(string path)
    {
        if(!path.EndsWith(".json") || !File.Exists(path))
        {
            throw new ArgumentException("Such database does not exist");
        }
        string jsonFromFile = File.ReadAllText(path);
        DatabaseDto deserializedDatabase = DeserializeJsonToDatabase(jsonFromFile);
        Database database = new Database(deserializedDatabase.Name);
        foreach (var tableDto in deserializedDatabase.Tables)
        {
            var table = new Table(tableDto.Name, tableDto.Columns);
            foreach (var row in tableDto.Rows)
            {
                object[] rowValues = row.Items.Select(item => item.Value).ToArray();
                table.AddRow(rowValues);
            }
            database.AddTable(table);
        }
        return database;
    }

    private static string SerializeDatabaseToJson(DatabaseDto database)
    {
        return JsonSerializer.Serialize(database);
    }

    private static DatabaseDto DeserializeJsonToDatabase(string json)
    {
        return JsonSerializer.Deserialize<DatabaseDto>(json);
    }
}