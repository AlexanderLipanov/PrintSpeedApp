using System;
using System.Net.Sockets;
using System.Text;
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
        public TextForm()
        {
            InitializeComponent();
        }

        private async Task SendDataAsync(string data)
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await socket.ConnectAsync("localhost", 55785);

                if (socket.Connected)
                {
                    var messageBytes = Encoding.UTF8.GetBytes(data);

                    await socket.SendAsync(messageBytes);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return; 
            }
            finally
            {
            }
        }

        private async void TextBox_OnChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;

            await SendDataAsync(textBox.Text);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;

            textBox.Clear();
        }
    }
}
