using System.Windows;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1;

public partial class ChatWindow : Window
{
    private bool _isSend = false;
    private string _host = "127.0.0.1";
    private int _port = 8888;
    private string _name = "Name";
    public static Dispatcher D;
    public ChatWindow( string host, string port, string name)
    {
        try
        {
            _port = int.Parse(port);
            _host = host;
            _name = name;
        }
        catch(Exception exception)
        {
            var error = new MessegBox(exception.Message +
                                      "You will be connect to local server");
        }

        D = Dispatcher;
        InitializeComponent();
        Input.Text = string.Empty;
        Show();
    }

    private void Send_OnClick(object sender, RoutedEventArgs e)
    {
        if (Input.Text != string.Empty)
        {
            ShowMessage(Input.Text);
            _isSend = true;
        }
    }

    private void ShowMessage(string message)
    {
        message = Messege.CorrectShow(message);
        var time = Convert.ToString(DateTime.Now);
        var i = 0;
        string[] correctTime = time.Split(' ' );
        string[] seconds = correctTime[1].Split(':');
        var messageItem = new Messege("You:" + "\n" + message + "\n" + seconds[0]+":"+seconds[1]);
        Chat.Items.Add(messageItem);
    }

    private void GetMessage(string message)
    {
        message = Messege.CorrectShow(message);
        string[] convertedMessage = message.Split(':');
        var name = convertedMessage[0];
        convertedMessage[0] = string.Empty;
        message = String.Empty;
        message = name + ":" + "\n";
        foreach (var c in convertedMessage)
        {
            message += c;
        }
        var time = Convert.ToString(DateTime.Now);
        var i = 0;
        string[] correctTime = time.Split(' ' );
        string[] seconds = correctTime[1].Split(':');
        message += "\n" + seconds[0] + ":" + seconds[1];
        var messageItem = new Messege(message)
        {
            Background = Brushes.GhostWhite,
            Foreground = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.3),
            Margin = new Thickness(10),
            MaxWidth = 250
        };
        Chat.Items.Add(messageItem);
    }

    public async Task Start()
    {
        using TcpClient client = new TcpClient();
        StreamReader? Reader = null;
        StreamWriter? Writer = null;
 
        try
        {
            await client.ConnectAsync(_host, _port); //connecting client
            Reader = new StreamReader(client.GetStream());
            Writer = new StreamWriter(client.GetStream());
            if (Writer is null || Reader is null) return;
            // запускаем новый поток для получения данных
            Task.Run(()=>ReceiveMessageAsync(Reader));
            // start input messeges
            await SendMessageAsync(Writer, _name);
        }
        catch (Exception ex)
        {
            var error = new MessegBox(ex.Message);
        }
        Writer?.Close();
        Reader?.Close();
    }

    private async Task SendMessageAsync(StreamWriter writer, string userName)
    {
        // first sending name
    
        await writer.WriteLineAsync(userName);
        await writer.FlushAsync();
        var hello = new Messege($"For sending message enter it and press 'Send' button. \n" +
                                $"Max message length is 165 symbols")
        {
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Chat.Items.Add(hello);
        
        while (true)
        {
            await Task.Delay(1);
            while (_isSend && Input.Text!=string.Empty)
            {
                string? message = Input.Text;
                Input.Text = string.Empty;
                _isSend = false;
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }
    }
    
    // получение сообщений
    private async Task ReceiveMessageAsync(StreamReader reader)
    {
        while (true)
        {
            await Task.Delay(1);
            try
            {
                // считываем ответ в виде строки
                string? message = await reader.ReadLineAsync();
                // если пустой ответ, ничего не выводим на консоль
                if (string.IsNullOrEmpty(message)) continue;
                D.Invoke((Action)(() =>
                {
                    GetMessage(message);
                })); //вывод сообщения)
            }
            catch
            {
                break;
            }
        }
    }
    
}