using System.Net.Sockets;

namespace Demon.Models
{
    public class Client
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Host { get; set; }

        public readonly StreamWriter Writer;
        public readonly StreamReader Reader;

        private TcpClient _client;
        private Server _server;

        public Client(Server server, TcpClient client)
        {

            _server = server;
            _client = client;

            var stream = _client.GetStream();

            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }

        public async Task ProcessAsync()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var data = await Reader.ReadLineAsync();

                        Console.WriteLine($"GetData: {data}");

                        if (data != null)
                            await _server.SetDataAsync(data, Id);
                    }
                    catch
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

            }
        }
    }
}
