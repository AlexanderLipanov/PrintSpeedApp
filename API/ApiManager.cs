using System.Net.Sockets;
using System.Text;

namespace API
{

    public class ApiManager : IApiManager
    {

        private readonly string _host = "localhost";
        private readonly int _port = 55785;

        public async Task SendDataAsync(string data)
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await socket.ConnectAsync(_host, _port);

                if (socket.Connected)
                {
                    var messageBytes = Encoding.UTF8.GetBytes(data);

                    await socket.SendAsync(messageBytes);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            finally
            {
            }
        }
    }

    public interface IApiManager
    {
        Task SendDataAsync(string data);
    }
}
