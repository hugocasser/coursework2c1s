using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1;

public partial class Messege : ListBoxItem
{
    private static Dispatcher D;
    public Messege(string message)
    {
        Background = Brushes.Black;
        Foreground = Brushes.GhostWhite;
        Margin = new Thickness(10);
        MaxWidth = 250;
        HorizontalAlignment = HorizontalAlignment.Right;
        Content = message;
        D = ChatWindow.D;
    }

    public static string CorrectShow(string message)
    {
        if (message.Length <= 30) return message;
        var i = 0;
        foreach (var item in message)
        {
            i++;
            if (i == 30) message = message.Insert(i, "\n");
        }

        return message;
    }
}