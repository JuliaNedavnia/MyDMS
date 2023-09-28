using DMSClasses;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyDMS;

public static class EnumExtensions
{
    public static IEnumerable<ColumnType> ColumnTypeValues => 
        Enum.GetValues(typeof(ColumnType)).Cast<ColumnType>();
}
