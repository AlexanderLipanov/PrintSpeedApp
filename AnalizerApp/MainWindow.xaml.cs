using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TcpClient tcpClient = new TcpClient("95.161.223.161", 49690);
        public static NetworkStream networkStream = tcpClient.GetStream();
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ConnectAsTcpClient();
            }
            catch
            {
                Console.WriteLine("Server Error");
            }
        }

        public static String response = string.Empty;

        public async Task ConnectAsTcpClient()
        {
            Byte[] data = new Byte[256];
            Int32 bytes = await networkStream.ReadAsync(data, 0, data.Length);
            response = System.Text.Encoding.UTF8.GetString(data);
            Console.WriteLine($"Response: {response}");
        }

    }
}
