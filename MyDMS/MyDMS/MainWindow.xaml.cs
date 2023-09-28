using System;
using System.Windows;
using DMSClasses;

namespace MyDMS;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow 
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CreateDatabaseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var database = GetNewDatabase();
            RedirectToTablesWindowPage(database);
            Close();
        }
        catch (ArgumentException exception)
        {
            ErrorWindowCaller.ShowErrorWindow(exception.Message);
        }
    }

    private Database GetNewDatabase() => new Database(databaseNameTextBox.Text);

    private void RedirectToTablesWindowPage(Database database)
    {
        TablesWindow tablesWindow = new TablesWindow(database);
        tablesWindow.Show();
    }

    private void OpenDatabaseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var database = DatabaseJsonConverter.GetDatabaseFrom(databasePathTextBox.Text);
            RedirectToTablesWindowPage(database);
            Close();
        }
        catch (ArgumentException exception)
        {
            ErrorWindowCaller.ShowErrorWindow(exception.Message);
        }
    }
}
