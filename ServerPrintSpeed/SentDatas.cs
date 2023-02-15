using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerPrintSpeed
{
    public class SentDatas : ISentDatas
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
        public async Task SendMyMessage(string text)
        {
            using Socket tcpListener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
                );

            try
            {
                tcpListener.Bind(ipPoint);
                tcpListener.Listen();   

                while (true)
                {
                    using var tcpClient = await tcpListener.AcceptAsync();
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    await tcpClient.SendAsync(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    public interface ISentDatas
    {
        Task SendMyMessage(string text);
    }
}