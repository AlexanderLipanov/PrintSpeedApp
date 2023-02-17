using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            catch(Exception ex) 
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

                    var data = await reader.ReadLineAsync();

                    outputText.Text = _responseData;

                    Thread.Sleep(100);
                }
            }
            catch
            {

            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}
