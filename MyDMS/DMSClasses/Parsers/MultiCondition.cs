using DMSClasses.Enums;

namespace DMSClasses.Parsers;

public sealed class MultiCondition
{
    private List<Linker> _linkers;
    private List<Condition> _conditions;
    private Table _table;

    public MultiCondition(List<Condition> conditions, List<Linker> linkers, Table table)
    {
        _conditions = conditions;
        _linkers = linkers;
        _table = table;
    }

    public HashSet<Row> GetRowsSatisfyMultiCondition()
    {
        HashSet<Row> rowsSatisfyMultiCondition = _conditions[0].GetRowsWhichSatisfyCondition(_table.Rows);
        
        for (int i = 0; i < _linkers.Count; i++)
        {
            switch (_linkers[i])
            {
                case Linker.And:
                    rowsSatisfyMultiCondition = _conditions[i + 1].GetRowsWhichSatisfyCondition(rowsSatisfyMultiCondition);
                    break;
                case Linker.Or:
                    rowsSatisfyMultiCondition.UnionWith(_conditions[i + 1].GetRowsWhichSatisfyCondition(_table.Rows));
                    break;
                default:
                    throw new ArgumentException("Do not have such condition linker");
            }
        }

        return rowsSatisfyMultiCondition;
    }
}