using System.Windows;

namespace WpfApp1;

public partial class MessegBox : Window
{
    public MessegBox(string? text)
    {
        InitializeComponent();
        MessegTextBox.Text = text;
        Show();
    }

    private void Ok(object sender, RoutedEventArgs e)
    {
       Hide();
    }
}