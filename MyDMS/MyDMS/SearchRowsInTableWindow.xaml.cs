using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DMSClasses;
using DMSClasses.Parsers;
using Table = DMSClasses.Table;

namespace MyDMS
{
    /// <summary>
    /// Interaction logic for SearchRowsInTableWindow.xaml
    /// </summary>
    public partial class SearchRowsInTableWindow : Window
    {
        private Table _table;
        private Database _database;
        private readonly ObservableCollection<object[]> _tableRows = new();

        public SearchRowsInTableWindow(Table table, Database database)
        {
            InitializeComponent();
            _table = table;
            _database = database;
            tableRowsDataGrid.ItemsSource = _tableRows;
            columnNamesListBox.ItemsSource = _table.Columns;
        }

        private void BackToTablesBtn_Click(object sender, RoutedEventArgs e)
        {
            var tablesWindow = new TablesWindow(_database);
            tablesWindow.Show();
            Close();
        }

        private void SearchBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _tableRows.Clear();
                tableRowsDataGrid.Columns.Clear();
                var searchRequest = requestTextBox.Text;
                var multicondition = ConditionsParser.ParseMultiConditionForTable(searchRequest, _table);
                var rowsSatisfyCondition = multicondition.GetRowsSatisfyMultiCondition();
                FillTableRowsDataGridForSelectedTable(rowsSatisfyCondition);
            }
            catch (Exception ex)
            {
                var errorWindow = new ErrorWindow(ex.Message);
                errorWindow.Show();
            }

        }

        private void FillTableRowsDataGridForSelectedTable(IEnumerable<Row> rows)
        {
            ConfigureDataGridColumns();

            foreach (var row in rows)
            {
                _tableRows.Add(row.Items.Select(x => x.Value).ToArray());
            }
            tableRowsDataGrid.ItemsSource = _tableRows;

            void ConfigureDataGridColumns()
            {
                for (int i = 0; i < _table.Columns.Count(); i++)
                {
                    DataGridTextColumn column = new()
                    {
                        Width = new DataGridLength(150),
                        Binding = new Binding("[" + i + "]")
                    };
                    tableRowsDataGrid.Columns.Add(column);
                }
            }
        }
    }
}
