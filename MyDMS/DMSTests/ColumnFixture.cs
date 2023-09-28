using DMSClasses;

namespace DMSTests;
internal class ColumnFixture
{
    [Test]
    public void ValidateValueForColumn_ColumnTypeInt_ProvidedInt_ReturnsTrue()
    {
        //Arrange
        var intColumn = new Column("intColumn", ColumnType.Integer);

        //Act
        var validationWithIntResult = intColumn.IsValueValidForColumn(3);

        //Assert
        Assert.That(validationWithIntResult, Is.True);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeInt_ProvidedNotInt_ReturnsFalse()
    {
        //Arrange
        var intColumn = new Column("intColumn", ColumnType.Integer);

        //Act
        var validationWithNotIntResult = intColumn.IsValueValidForColumn('c');

        //Assert
        Assert.That(validationWithNotIntResult, Is.False);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeChar_ProvidedChar_ReturnsTrue()
    {
        //Arrange
        var charColumn = new Column("charColumn", ColumnType.Char);

        //Act
        var validationWithCharResult = charColumn.IsValueValidForColumn('k');

        //Assert
        Assert.That(validationWithCharResult, Is.True);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeChar_ProvidedNotChar_ReturnsFalse()
    {
        //Arrange
        var charColumn = new Column("charColumn", ColumnType.Char);

        //Act
        var validationWithNotCharResult = charColumn.IsValueValidForColumn("string");

        //Assert
        Assert.That(validationWithNotCharResult, Is.False);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeReal_ProvidedReal_ReturnsTrue()
    {
        //Arrange
        var realColumn = new Column("realColumn", ColumnType.Real);

        //Act
        var validationWithRealResult = realColumn.IsValueValidForColumn(2.5);

        //Assert
        Assert.That(validationWithRealResult, Is.True);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeReal_ProvidedNotReal_ReturnsFalse()
    {
        //Arrange
        var realColumn = new Column("realColumn", ColumnType.Real);

        //Act
        var validationWithNotRealResult = realColumn.IsValueValidForColumn("notReal");

        //Assert
        Assert.That(validationWithNotRealResult, Is.False);
    }

    [Test]
    public void ValidateValueForColumn_ColumnTypeString_ReturnsTrue()
    {
        //Arrange
        var stringColumn = new Column("stringColumn", ColumnType.String);

        //Act
        var validationWithStringResult = stringColumn.IsValueValidForColumn("4.5");
        var validationWithNotStringResult = stringColumn.IsValueValidForColumn(5);

        //Assert
        Assert.That(validationWithStringResult, Is.True);
        Assert.That(validationWithNotStringResult, Is.True);
    }
}
