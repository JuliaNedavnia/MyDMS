using DMSClasses;

namespace DMSTests;

internal class TableFixture
{
    [Test]
    public void CreateTable_ShouldBeCreatedTableWithColumnsButWithoutRows()
    {
        //Arrange
        var column1 = new Column("stringColumn", ColumnType.String);
        var column2 = new Column("intColumn", ColumnType.Integer);

        //Act
        var tableCreated = new Table("TestTable", new List<Column> { column1, column2 });

        //Assert
        Assert.That(tableCreated.Columns.Count, Is.EqualTo(2));
        Assert.That(tableCreated.Rows, Is.Empty);
        Assert.That(tableCreated.Columns.ElementAt(0), Is.EqualTo(column1));
        Assert.That(tableCreated.Columns.ElementAt(1), Is.EqualTo(column2));
    }

    [Test]
    public void CreateTable_WithoutColumns_ThrowsArgumentException()
    {
        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new Table("table", new List<Column>());
        });
    }

    [Test]
    public void CreateTable_WithTwoColumnsWithEqualName_ThrowsArgumentException()
    {
        //Arrange
        var column1 = new Column("column", ColumnType.String);
        var column2 = new Column("column", ColumnType.Integer);

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = new Table("table", new List<Column> { column1, column2 });
        });
    }

    [Test]
    public void AddRow_WithProperValues_ShouldAddRowWithValues()
    {
        //Arrange
        var stringColumn = new Column("stringColumn", ColumnType.String);
        var intColumn = new Column("intColumn", ColumnType.Integer);
        var charColumn = new Column("charColumn", ColumnType.Char);
        var realColumn = new Column("realColumn", ColumnType.Real);
        var table = new Table("TestTable", new List<Column> { stringColumn, intColumn, charColumn, realColumn });

        var value1 = "string";
        var value2 = 3;
        var value3 = 'c';
        var value4 = 2.1;
        var values = new object[] { value1, value2, value3, value4 };

        //Act
        table.AddRow(values);

        //Assert
        Assert.That(table.Rows.Count(), Is.EqualTo(1));
        var createdRowValues = table.Rows.ElementAt(0).Items;
        Assert.That(createdRowValues.Count, Is.EqualTo(4));
        Assert.That(createdRowValues[0].Value, Is.EqualTo(value1));
        Assert.That(createdRowValues[1].Value, Is.EqualTo(value2));
        Assert.That(createdRowValues[2].Value, Is.EqualTo(value3));
        Assert.That(createdRowValues[3].Value, Is.EqualTo(value4));
    }

    [Test]
    public void AddRow_WithValuesInWrongOrder_ShouldAddRowWithValues()
    {
        //Arrange
        var stringColumn = new Column("stringColumn", ColumnType.String);
        var intColumn = new Column("intColumn", ColumnType.Integer);
        var charColumn = new Column("charColumn", ColumnType.Char);
        var realColumn = new Column("realColumn", ColumnType.Real);
        var table = new Table("TestTable", new List<Column> { stringColumn, intColumn, charColumn, realColumn });

        var value1 = "string";
        var value2 = 3;
        var value3 = 'c';
        var value4 = 2.1;
        var values = new object[] { value2, value4, value1, value3 };

        //Act

        //Assert
        Assert.Throws<ArgumentException>(() => table.AddRow(values));
    }

    [Test]
    public void AddRow_ProvidedValueWithWrongType_ShouldAddRowWithValues()
    {
        //Arrange
        var stringColumn = new Column("stringColumn", ColumnType.String);
        var intColumn = new Column("intColumn", ColumnType.Integer);
        var charColumn = new Column("charColumn", ColumnType.Char);
        var realColumn = new Column("realColumn", ColumnType.Real);
        var table = new Table("TestTable", new List<Column> { stringColumn, intColumn, charColumn, realColumn });

        var value1 = "string";
        var value2 = 3;
        var wrongValue = 675;//not char
        var value4 = 2.1;
        var values = new object[] { value1, value2, wrongValue, value4 };

        //Act

        //Assert
        Assert.Throws<ArgumentException>(() => table.AddRow(values));
    }

    [Test]
    public void AddRow_CountOfValuesIsGreaterThanColumns_ShouldAddRowWithValues()
    {
        //Arrange
        var stringColumn = new Column("stringColumn", ColumnType.String);
        var intColumn = new Column("intColumn", ColumnType.Integer);
        var charColumn = new Column("charColumn", ColumnType.Char);
        var realColumn = new Column("realColumn", ColumnType.Real);
        var table = new Table("TestTable", new List<Column> { stringColumn, intColumn, charColumn, realColumn });

        var value1 = "string";
        var value2 = 3;
        var value3 = 'c';
        var value4 = 2.1;
        var values = new object[] { value1, value2, value3, value4, "additional value" };

        //Act

        //Assert
        Assert.Throws<ArgumentException>(() => table.AddRow(values));
    }
}
