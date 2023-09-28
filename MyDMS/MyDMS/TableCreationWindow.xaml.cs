using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DMSClasses;

namespace MyDMS;

    /// <summary>
    /// Interaction logic for TableCreationWindow.xaml
    /// </summary>
public partial class TableCreationWindow
{
    private readonly Database _database;
    private readonly ObservableCollection<ColumnInfoDto> _columnsInfo = new ();

    public TableCreationWindow(Database database)
    {
        InitializeComponent();
        _database = database;
        tableNameTextBox.Text = $"NewTable{_database.Tables.Count()}";
    }

    private void AddRowBtn_Click(object sender, RoutedEventArgs e)
    {
        columnDataGrid.ItemsSource ??= _columnsInfo;   
        var defaultColumnInfo = new ColumnInfoDto
        {
            Name = $"Column{_columnsInfo.Count}",
            Type = ColumnType.Integer
        };
        _columnsInfo.Add(defaultColumnInfo);
    }

    private void DeleteRowBtn_OnClick(object sender, RoutedEventArgs e)
    {
        _columnsInfo.RemoveAt(_columnsInfo.Count - 1);
    }

    private void SaveTableBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            AddEnteredColumnsToTable();
            TablesWindow tablesWindow = new TablesWindow(_database);
            tablesWindow.Show();
            Close();
        }
        
        catch (ArgumentException exception)
        {
            ErrorWindowCaller.ShowErrorWindow(exception.Message);
        }

        void AddEnteredColumnsToTable()
        {
            var tableName = tableNameTextBox.Text;
            List<Column> tableColumns = new List<Column>();

            foreach (var columnInfo in _columnsInfo)
            {
                var column = new Column(columnInfo.Name,  columnInfo.Type);
                tableColumns.Add(column);
            }
            
            _database.AddTable(new Table(tableName, tableColumns));
        }
    }
}
