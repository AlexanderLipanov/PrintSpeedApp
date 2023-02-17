using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, 55785);

        private string? _responseData = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ListenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();


                while (true)
                {

                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    var stream = tcpClient.GetStream();

                    var reader = new StreamReader(stream);

                    _responseData = await reader.ReadLineAsync();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private string _dataLengeth = string.Empty;

        private void Calculate(object sender, RoutedEventArgs e)
        {
            if (_responseData is null) return;

            _dataLengeth = _responseData.Length.ToString();
            lengthData.Text = _dataLengeth;
            Console.WriteLine(_responseData?.Length);
        }
    }
}
