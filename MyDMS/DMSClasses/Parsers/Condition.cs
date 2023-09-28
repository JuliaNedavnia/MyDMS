using DMSClasses.ConditionEvaluators;
using DMSClasses.Enums;

namespace DMSClasses.Parsers;

public class Condition
{
    private readonly Table _table;
    private readonly bool _isNegative;
    private readonly Column _column;
    private readonly object _value;
    private readonly Operation _operation;
    
    public Condition(string columnName, Operation operation, object value, bool isNegative, Table table)
    {
        _table = table;
        _column = GetColumnWithName(columnName);
        ThrowIfValueIsNotOfColumnType(value);
        _value = value;
        _operation = operation;
        _isNegative = isNegative;
    }

    public HashSet<Row> GetRowsWhichSatisfyCondition(IEnumerable<Row> rowsToCheck)
    {
        HashSet<Row> rowsSatisfyConditions = new HashSet<Row>();

        foreach (var row in rowsToCheck)
        {
            var rowItemWithColumn = row.Items.Single(item => item.Column == _column);
            var conditionEvaluatorFactory = new ConditionEvaluatorFactory();
            var conditionEvaluator = conditionEvaluatorFactory.GetConditionEvaluator(rowItemWithColumn);
            if (CheckConditionSuccess(conditionEvaluator))
            {
                rowsSatisfyConditions.Add(row);
            }
        }

        return rowsSatisfyConditions;
    }

    private Column GetColumnWithName(string columnName)
    {
        var foundColumn = _table.Columns.SingleOrDefault(column => column.Name == columnName);
        
        if (foundColumn is null)
        {
            throw new ArgumentException("Column with such name does not exist in table");
        }

        return foundColumn;
    }

    private void ThrowIfValueIsNotOfColumnType(object value)
    {
        if (_column.Type == ColumnType.Html)
        {
            return;
        }
        if (!_column.IsValueValidForColumn(value))
        {
            throw new ArgumentException($"Value {value} has not appropriate type for column {_column.Name}");
        }
    }

    private bool CheckConditionSuccess(RowItemConditionEvaluator rowItemConditionEvaluator)
    {
        bool result = false;
        switch (_operation)
        {
            case Operation.Equal:
                result = rowItemConditionEvaluator.Equal(_value);
                break;
            case Operation.GreaterThan:
                result = rowItemConditionEvaluator.GreaterThan(_value);
                break;
            case Operation.LessThan:
                result = rowItemConditionEvaluator.LessThan(_value);
                break;
            case Operation.GreaterThanOrEqual:
                result = rowItemConditionEvaluator.GreaterThanOrEqual(_value);
                break;
            case Operation.LessThanOrEqual:
                result = rowItemConditionEvaluator.LessThanOrEqual(_value);
                break;
            case Operation.Like:
                result = rowItemConditionEvaluator.Like(_value);
                break;
            default:
                throw new ArgumentException("Do not have such operation");
        }

        return _isNegative ? !result : result;
    }
}