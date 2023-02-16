using Demon.Models;
using System.Net;
using System.Net.Sockets;

Server server = new Server();

await server.ListenAsync();

public class Server
{
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 55785); // сервер для прослушивания

    List<Client> clients = new List<Client>();

    public async Task ListenAsync()
    {
        Console.WriteLine("Server Start");
        try
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                Client client = new Client(this, tcpClient);

                clients.Add(client);

                Task.Run(client.ProcessAsync);   

                Thread.Sleep(100);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {

        }
        Console.WriteLine("Server End");
    }

    public async Task SetDataAsync(string data, string id)
    {
        foreach (var client in clients.Where(e => e.Id != id))
        {
            await client.Writer.WriteAsync(data);
            await client.Writer.FlushAsync();
        }
    }
}