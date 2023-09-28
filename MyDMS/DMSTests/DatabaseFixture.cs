using DMSClasses;

namespace DMSTests;

public class DatabaseFixture
{
    [Test]
    public void CreateDatabase_NameIsEmptyString_ThrowsArgumentException()
    {
        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new Database("");
        });
    }
    
    [Test]
    public void GetTables_DatabaseJustCreated_ReturnsEmptyTablesList()
    {
        //Arrange
        var database = new Database("TestDb");

        //Assert
        Assert.That(database.Tables, Is.Empty);
    }

    [Test]
    public void AddTable_TableWithNameAlreadyExists_ThrowsArgumentException()
    {
        //Arrange
        var database = new Database("TestDb");
        var columnsList = new List<Column>()
        {
            new Column("column", ColumnType.String)
        };
        var table1 = new Table("Table", columnsList);
        var table2 = new Table("Table", columnsList);

        //Act
        database.AddTable(table1);

        //Assert
        Assert.Throws<ArgumentException>(() => database.AddTable(table2));
    }
}