using System.Text.RegularExpressions;

namespace DMSClasses;

public sealed class Column
{
    public Column(string name, ColumnType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public ColumnType Type { get; }

    public bool IsValueValidForColumn(object value)
    {
        var valueAsString = value.ToString();

        return Type switch
        {
            ColumnType.Integer => int.TryParse(valueAsString, out _),
            ColumnType.Char => char.TryParse(valueAsString, out _),
            ColumnType.Real => float.TryParse(valueAsString, out _),
            ColumnType.String => true,
            ColumnType.Html => !string.IsNullOrEmpty(valueAsString)
                               && valueAsString.EndsWith(".html") 
                               && File.Exists(valueAsString),
            ColumnType.StringInvl => CheckStringInvlValue(value.ToString()),
            _ => throw new ArgumentException("Such type does not exist")
        };
    }

    private bool CheckStringInvlValue(string value)
    {
        string stringInvlPattern = @"\(([^,]+), ([^)]+)\)";

        Match match = Regex.Match(value, stringInvlPattern);

        if (match.Success)
        {
            string firstString = match.Groups[1].Value.Trim();
            string secondString = match.Groups[2].Value.Trim();

            return string.Compare(firstString, secondString) <= 0;
        }

        throw new ArgumentException("StringInvl value should look like (\"string1\",\"string2\"." +
                                    "Where string1 < string2.");
    }
}
