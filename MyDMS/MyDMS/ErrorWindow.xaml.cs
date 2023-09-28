using System.Windows;

namespace MyDMS;

public partial class ErrorWindow : Window
{
    public ErrorWindow(string errorMessage)
    {
        InitializeComponent();
        errorMessageText.Text = errorMessage;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}