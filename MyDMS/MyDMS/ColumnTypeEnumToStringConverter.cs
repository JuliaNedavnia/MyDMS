using DMSClasses;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System;
using System.Linq;
using System.Windows;

namespace MyDMS;
public class ColumnTypeEnumToStringConverter : IValueConverter
{
    public static readonly ColumnTypeEnumToStringConverter Instance = new ();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<ColumnType> enumValues)
        {
            return enumValues.Select(v => v.ToString());
        }

        return new object();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Enum.TryParse(value as string, out ColumnType columnType))
        {
            return columnType;
        }

        return DependencyProperty.UnsetValue;
    }
}
