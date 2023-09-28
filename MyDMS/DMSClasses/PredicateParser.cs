using System.Text.RegularExpressions;

namespace DMSClasses;

internal enum Operation
{
    Equal,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    Like
}

public sealed class PredicateParser
{
    private readonly List<Condition> _subConditions = new ();
    private readonly Table _table;

    public PredicateParser(string condition, Table table)
    {
        var subConditionsInString = Regex.Split(condition, @"\s+(and,or)\s+", RegexOptions.IgnoreCase);
        foreach (var subCondition in subConditionsInString)
        {
            _subConditions.Add(ParseSubCondition(condition));
        }
        _table = table;
    }

    private Condition ParseSubCondition(string condition)
    {
        string pattern = @"^(not\s+)?([a-zA-Z_][a-zA-Z0-9_]*)\s*(=|<|>|<=|>=|like)\s*'([^']*)'$";
        
        Match match = Regex.Match(condition, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ArgumentException($"Invalid condition: {condition}");
        }
        
        bool isNegative = !string.IsNullOrEmpty(match.Groups[1].Value);
        string columnName = match.Groups[2].Value;
        string operationStr = match.Groups[3].Value;
        object value = match.Groups[4].Value;

        Operation operation = ParseOperation(operationStr);

        return new Condition(isNegative, columnName, operation, value, _table);
    }
    
    private Operation ParseOperation(string operationString)
    {
        switch (operationString.ToLower())
        {
            case "=":
                return Operation.Equal;
            case "<":
                return Operation.LessThan;
            case ">":
                return Operation.GreaterThan;
            case "<=":
                return Operation.LessThanOrEqual;
            case ">=":
                return Operation.GreaterThanOrEqual;
            case "like":
                return Operation.Like;
            default:
                throw new ArgumentException($"Unsupported operation: {operationString}");
        }
    }
}


internal sealed class Condition
{
    private readonly Table _table;
    private readonly Column _column;
    private readonly object _conditionValue;
    private readonly Operation _operation;
    private readonly bool _isNegative;

    public Condition(bool isNegative, string columnName, Operation operation, object conditionValue, Table table)
    {
        _column = GetColumnFromTable(columnName);
        CheckValueIsOfRightType(conditionValue);
        _conditionValue = conditionValue;
        _operation = operation;
        _table = table;
        _isNegative = _isNegative;
    }

    public List<Row> GetRowsWithConditions()
    {
        var appropriateRows = new List<Row>();
        foreach (var row in _table.Rows)
        {
            if (IsAppropriateRowForCondition(row))
            {
                appropriateRows.Add(row);
            }
        }
        return appropriateRows;
    }

    private bool IsAppropriateRowForCondition(Row row)
    {
        var rowItemForColumn = row.Items.Single(item => item.Column == _column);
        switch (_operation)
        {
            case Operation.Equal:
                return rowItemForColumn.Equal(_conditionValue);
            case Operation.GreaterThan:
            case Operation.LessThan:
            case Operation.LessThanOrEqual:
            case Operation.GreaterThanOrEqual:
                return rowItemForColumn.GreaterThanOrEqual(_conditionValue);
            case Operation.Like:
                return rowItemForColumn.Like(_conditionValue);
            default:
                throw new ArgumentException("Do not have such operation");
        }
    }

    private Column GetColumnFromTable(string columnName)
    {
        var foundColumn = _table.Columns.SingleOrDefault(column => column.Name == columnName);
        return foundColumn ?? throw new ArgumentException($"Table does not contain column with name {columnName}");
    }

    private void CheckValueIsOfRightType(object value)
    {
        if (!_column.IsValueValidForColumn(value))
        {
            throw new ArgumentException($"{value} is not of type {Enum.GetName(typeof(ColumnType), _column.Type)}");
        }
    }
}
