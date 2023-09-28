using DMSClasses;
using DMSClasses.Parsers;

namespace DMSTests;

public class MultiConditionFixture
{
    [Test]
    public void ParseMultiCondition_PositiveValidMulticonditionWithAnd_ReturnsMultiConditionResult()
    {
        //Arrange
        Column intColumn = new Column("IntColumn", ColumnType.Integer);
        Column realColumn = new Column("RealColumn", ColumnType.Real);
        Table table = new Table("Table", new List<Column> { intColumn, realColumn });
        var rowSatisfyResult = new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.5) });
        HashSet<Row> expectedResult = new HashSet<Row>() { rowSatisfyResult };
        List<Row> rows = new()
        {
            new Row(new List<RowItem> { new (intColumn, -1), new (realColumn, 0.5) }),
            rowSatisfyResult,
            new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.7) })
        };
        table.AddRangeOfRows(rows);
        string multiconditionString = "IntColumn > '0' AND RealColumn = '0.5'";
        var multiCondition = ConditionsParser.ParseMultiConditionForTable(multiconditionString, table);
        
        //Act
        var rowsWhichSatisfyCondition = multiCondition.GetRowsSatisfyMultiCondition();
        
        //Assert
        CollectionAssert.AreEquivalent(rowsWhichSatisfyCondition, expectedResult);
    }
    
    [Test]
    public void ParseMultiCondition_ValidMulticonditionWithAndAndNegativeSubcondition_ReturnsMultiConditionResult()
    {
        //Arrange
        Column intColumn = new Column("IntColumn", ColumnType.Integer);
        Column realColumn = new Column("RealColumn", ColumnType.Real);
        Table table = new Table("Table", new List<Column> { intColumn, realColumn });
        var rowSatisfyResult = new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.7) });
        HashSet<Row> expectedResult = new HashSet<Row>() { rowSatisfyResult };
        List<Row> rows = new()
        {
            new Row(new List<RowItem> { new (intColumn, -1), new (realColumn, 0.5) }),
            new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.5) }),
            rowSatisfyResult,
        };
        table.AddRangeOfRows(rows);
        string multiconditionString = "IntColumn > '0' AND NOT RealColumn = '0.5'";
        var multiCondition = ConditionsParser.ParseMultiConditionForTable(multiconditionString, table);
        
        //Act
        var rowsWhichSatisfyCondition = multiCondition.GetRowsSatisfyMultiCondition();
        
        //Assert
        CollectionAssert.AreEquivalent(rowsWhichSatisfyCondition, expectedResult);
    }

    [Test]
    public void ParseMultiCondition_PositiveValidMulticonditionWithOr_ReturnsMultiConditionResult()
    {
        //Arrange
        Column intColumn = new Column("IntColumn", ColumnType.Integer);
        Column realColumn = new Column("RealColumn", ColumnType.Real);
        Table table = new Table("Table", new List<Column> { intColumn, realColumn });
        var row1SatisfyResult = new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.5) });
        var row2SatisfyResult = new Row(new List<RowItem> { new (intColumn, -1), new (realColumn, 0.5) });
        var row3SatisfyResult = new Row(new List<RowItem> { new (intColumn, 2), new (realColumn, 0.7) });
        HashSet<Row> expectedResult = new HashSet<Row>() { row1SatisfyResult, row2SatisfyResult, row3SatisfyResult };
        List<Row> rows = new()
        {
            row1SatisfyResult,
            row2SatisfyResult,
            row3SatisfyResult,
            new Row(new List<RowItem> { new (intColumn, -2), new (realColumn, 0.7) })
        };
        table.AddRangeOfRows(rows);
        string multiconditionString = "IntColumn > '0' OR RealColumn = '0.5'";
        var multiCondition = ConditionsParser.ParseMultiConditionForTable(multiconditionString, table);
        
        //Act
        var rowsWhichSatisfyCondition = multiCondition.GetRowsSatisfyMultiCondition();
        
        //Assert
        CollectionAssert.AreEquivalent(rowsWhichSatisfyCondition, expectedResult);
    }

    [Test]
    public void ParseMultiCondition_ValidConditionWithLike_ReturnsResult()
    {
        //Arrange
        Column stringColumn = new Column("StringColumn", ColumnType.String);
        Table table = new Table("Table", new List<Column> { stringColumn });
        var row1SatisfyResult = new Row(new List<RowItem> { new (stringColumn, "kitten")});
        var row2SatisfyResult = new Row(new List<RowItem> { new (stringColumn, "kit"),});
        HashSet<Row> expectedResult = new HashSet<Row>() { row1SatisfyResult, row2SatisfyResult};
        List<Row> rows = new()
        {
            row1SatisfyResult,
            row2SatisfyResult,
            new Row(new List<RowItem> { new (stringColumn, "ten"), })
        };
        table.AddRangeOfRows(rows);
        string multiconditionString = "StringColumn LIKE 'kit%'";
        var multiCondition = ConditionsParser.ParseMultiConditionForTable(multiconditionString, table);

        //Act
        var rowsWhichSatisfyCondition = multiCondition.GetRowsSatisfyMultiCondition();

        //Assert
        CollectionAssert.AreEquivalent(rowsWhichSatisfyCondition, expectedResult);
    }
}