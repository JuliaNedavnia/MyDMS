using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DMSClasses;

namespace MyDMS;

/// <summary>
/// Interaction logic for TablesWindow.xaml
/// </summary>
public partial class TablesWindow
{
    private readonly Database _database;
    private readonly ObservableCollection<object[]> _tableRows = new ();
    private readonly ObservableCollection<Table> _tables = new();

    public TablesWindow(Database database)
    {
        _database = database;
        InitializeComponent();
        Title = database.Name;

        DisableAllButtonsToChangeTableValues();
        foreach (var table in _database.Tables)
        {
            _tables.Add(table);
        }
        tablesListBox.ItemsSource = _tables;
        tableRowsDataGrid.ItemsSource = _tableRows;
    }

    private void TablesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ClearTableDataToShow();
        if(tablesListBox.SelectedItems.Count != 0)
        {
            FillColumnsNameForSelectedTable();
            FillTableRowsDataGridForSelectedTable();
            EnableAllButtonsToChangeTableValues();
        }
    }

    private void CreateTableBtn_Click(object sender, RoutedEventArgs e)
    {
        TableCreationWindow tableCreationWindow = new TableCreationWindow(_database);
        tableCreationWindow.Show();
        Close();
    }

    private void AddRowBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var selectedTable = GetSelectedTable();
            ValidateLastFilledRow(selectedTable);

            var defaultValues = new object[selectedTable.Columns.Count()];
            for (int i = 0; i < defaultValues.Length; i++)
            {
                defaultValues[i] = "";
            }
            _tableRows.Add(defaultValues);
        }
        catch (ArgumentException exception)
        {
            ErrorWindowCaller.ShowErrorWindow("Last row had such problems:" + exception.Message);
        }

        void ValidateLastFilledRow(Table selectedTable)
        {
            if (_tableRows.Count != 0)
            {
                for (int i = 0; i < _tableRows.Last().Length; i++)
                {
                    var isValueValid = selectedTable.Columns.ElementAt(i).IsValueValidForColumn(_tableRows.Last().ElementAt(i));
                    if (!isValueValid)
                    {
                        throw new ArgumentException(
                            $"Value with index {i} should have type {Enum.GetName(typeof(ColumnType), selectedTable.Columns.ElementAt(i).Type)}");
                    }
                }
            }
        }
    }

    private void DeleteTableBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var selectedTable = GetSelectedTable();
            _database.RemoveTable(selectedTable);

            _tables.Remove(selectedTable);
            ClearTableDataToShow();
            DisableAllButtonsToChangeTableValues();
        }
        catch (InvalidOperationException exception)
        {
            ErrorWindowCaller.ShowErrorWindow(exception.Message);
        }
    }

    private void ClearTableDataToShow()
    {
        _tableRows.Clear();
        columnNamesListBox.ItemsSource = null;
        tableRowsDataGrid.ItemsSource = null;
        tableRowsDataGrid.Columns.Clear();
    }

    private void FillTableRowsDataGridForSelectedTable()
    {
        var selectedTable = GetSelectedTable();
        ConfigureDataGridColumns();
        
        foreach (var row in selectedTable.Rows)
        {
            _tableRows.Add(row.Items.Select(x => x.Value).ToArray());
        }
        tableRowsDataGrid.ItemsSource = _tableRows;

        void ConfigureDataGridColumns()
        {
            for (int i = 0; i < selectedTable.Columns.Count(); i++)
            {
                DataGridTextColumn column = new ()
                {
                    Width = new DataGridLength(150),
                    Binding = new Binding("[" + i + "]")
                };
                tableRowsDataGrid.Columns.Add(column);
            }
        }
    }

    private void FillColumnsNameForSelectedTable()
    {
        var selectedTable = GetSelectedTable();
        columnNamesListBox.ItemsSource = selectedTable.Columns;
    }
    
    private Table GetSelectedTable() => (Table)tablesListBox.SelectedItem;

    private void RemoveRowBtn_Click(object sender, RoutedEventArgs e)
    {
        _tableRows.Remove((object[])tableRowsDataGrid.SelectedItem);
    }

    private void SaveTableValuesBtn_OnClick_Click(object sender, RoutedEventArgs e)
    {
        var selectedTable = GetSelectedTable();
        selectedTable.RemoveAllRows();
        var previousRows = selectedTable.Rows.ToList();

        for (int i = 0; i < _tableRows.Count; i++)
        {
            try
            {
                selectedTable.AddRow(_tableRows[i]);
            }
            catch (ArgumentException exception)
            {
                selectedTable.AddRangeOfRows(previousRows);
                ErrorWindowCaller.ShowErrorWindow($"Error in a row {i}: {exception.Message}");
            }
        }
    }

    private void EnableAllButtonsToChangeTableValues()
    {
        addRowBtn.IsEnabled = true;
        deleteTableBtn.IsEnabled = true;
        saveTableValuesBtn.IsEnabled = true;
        removeRowBtn.IsEnabled = true;
        searchRowsBtn.IsEnabled = true;
    }
    private void DisableAllButtonsToChangeTableValues()
    {
        addRowBtn.IsEnabled = false;
        deleteTableBtn.IsEnabled = false;
        saveTableValuesBtn.IsEnabled = false;
        removeRowBtn.IsEnabled = false;
        searchRowsBtn.IsEnabled = false;
    }

    private void SaveDatabaseBtn_OnClickBtn_OnClick_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            DatabaseJsonConverter.SaveDatabaseTo(databaseSavePathTextBox.Text, _database);
        }
        catch (ArgumentException exception)
        {
            ErrorWindowCaller.ShowErrorWindow(exception.Message);
        }
    }

    private void SearchRowsBtn_OnClick_Click(object sender, RoutedEventArgs e)
    {
        var selectedTable = GetSelectedTable();
        var searchWindow = new SearchRowsInTableWindow(selectedTable, _database);
        searchWindow.Show();
        Close();
    }
}
