namespace MyDMS;

public static class ErrorWindowCaller
{
    public static void ShowErrorWindow(string errorMessage)
    {
        var errorWindow = new ErrorWindow(errorMessage);
        errorWindow.Show();
        errorWindow.Focus();
    }
}