using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI
{
    /// <summary>
    /// Interaction logic for TextForm.xaml
    /// </summary>
    public partial class TextForm
    {

        public static TcpListener tcpListener = new TcpListener(IPAddress.Any, 49690);
        private string textData = string.Empty;

        public TextForm()
        {
            InitializeComponent();

            try
            {
                Connect();
            }
            catch
            {

            }
        }

        private async Task Connect()
        {
            try
            {
                tcpListener.Start();

                using var tcpClient = await tcpListener.AcceptTcpClientAsync();

                var stream = tcpClient.GetStream();

                using var sr = new StreamWriter(stream);

                while (true)
                {
                    await sr.WriteLineAsync(textData);
                    await sr.FlushAsync();

                    // Задержка, что бы разгрузить CPU
                    Thread.Sleep(500);
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private async void TextBox_OnChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;


        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;

            textBox.Clear();
        }
    }
}
