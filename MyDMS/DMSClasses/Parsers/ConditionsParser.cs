using System.Text.RegularExpressions;
using DMSClasses.Enums;
using ArgumentException = System.ArgumentException;

namespace DMSClasses.Parsers;

public static class ConditionsParser
{
    public static MultiCondition ParseMultiConditionForTable(string multiCondition, Table table)
    {
        var dividedMultiCondition = Regex.Split(multiCondition, @"\s+(and|or)\s+", RegexOptions.IgnoreCase);
        List<Linker> conditionLinkers = GetLinkersList();
        List<Condition> subConditions = new List<Condition>();
        for (int i = 0; i < dividedMultiCondition.Length; i += 2)
        {
            subConditions.Add(ParseConditionForTable(dividedMultiCondition[i], table));
        }

        return new MultiCondition(subConditions, conditionLinkers, table);

        List<Linker> GetLinkersList()
        {
            List<Linker> linkers = new List<Linker>();
            
            for (int i = 1; i < dividedMultiCondition.Length; i+=2)
            {
                switch (dividedMultiCondition[i].ToLower())
                {
                    case "and":
                        linkers.Add(Linker.And);
                        break;
                    case "or":
                        linkers.Add(Linker.Or);
                        break;
                    default:
                        throw new ArgumentException("Such linker does not exist");
                }
            }

            return linkers;
        }
    }

    private static Condition ParseConditionForTable(string conditionString, Table table)
    { 
        string columnName;
        Operation operation;
        object value;

        string conditionPattern = @"^(not\s+)?([a-zA-Z_][a-zA-Z0-9_]*)\s*(=|<|>|<=|>=|like)\s*'(\([^)]+\)|[\w.,%]+)'$"; 
        
        Match match = Regex.Match(conditionString, conditionPattern, RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var isNegative = !string.IsNullOrEmpty(match.Groups[1].Value);
            columnName = match.Groups[2].Value;
            string operationStr = match.Groups[3].Value;
            value = match.Groups[4].Value;

            operation = ParseOperation(operationStr);

            return new Condition(columnName, operation, value, isNegative, table);
        }

        throw new ArgumentException("Condition have wrong pattern");
    }

    private static Operation ParseOperation(string operationStr)
    {
        switch (operationStr.ToLower())
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
                throw new ArgumentException($"Unsupported operation: {operationStr}");
        }
    }
}