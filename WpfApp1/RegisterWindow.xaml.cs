using System.Windows;
using System.Threading.Tasks;
using System.Net.Sockets;
using System;
using System.IO;
namespace WpfApp1;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }
    private async void Ok_OnClick(object sender, RoutedEventArgs e)
    {
        var chatWindow = new ChatWindow(Host.Text, Port.Text, Name.Text);
        await chatWindow.Start();
    }
}