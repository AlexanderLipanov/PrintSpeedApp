using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //"95.161.223.161", 49690
        private string _responseData = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        public async Task GetDataAsync()
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await socket.ConnectAsync("localhost", 55785);

                if (socket.Connected)
                {
                    var responseBytes = new byte[512];
                    var bytes = await socket.ReceiveAsync(responseBytes);
                    string response = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                    Console.WriteLine(response);
                    _responseData = response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await GetDataAsync();
        }
    }
}
