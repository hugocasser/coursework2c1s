using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WpfApp1;


class ClientObject
{
    protected internal string Id { get;} = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get;}
    protected internal StreamReader Reader { get;}
 
    TcpClient _client;
    ServerObject _server; // объект сервера
 
    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        _client = tcpClient;
        _server = serverObject;
        // получаем NetworkStream для взаимодействия с сервером
        var stream = _client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
    }
 
    public async Task ProcessAsync()
    {
        try
        {
            // получаем имя пользователя
            string? userName = await Reader.ReadLineAsync();
            string? message = $"{userName} join chat";
            // посылаем сообщение о входе в чат всем подключенным пользователям
            await _server.BroadcastMessageAsync(message, Id);
            Console.WriteLine(message);
            // в бесконечном цикле получаем сообщения от клиента
            while (true)
            {
                try
                {
                    message = await Reader.ReadLineAsync();
                    if (message == null) continue;
                    message = $"{userName}: {message}";
                    Console.WriteLine(message);
                    await _server.BroadcastMessageAsync(message, Id);
                }
                catch
                {
                    message = $"{userName} left chat";
                    Console.WriteLine(message);
                    await _server.BroadcastMessageAsync(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // в случае выхода из цикла закрываем ресурсы
            _server.RemoveConnection(Id);
        }
    }
    // закрытие подключения
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        _client.Close();
    }
}