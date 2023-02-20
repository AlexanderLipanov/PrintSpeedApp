using API;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrintSpeedApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IApiManager _apiManager;

        public MainWindow(IApiManager apiManager)
        {
            _apiManager = apiManager;
            InitializeComponent();
        }

        private async void TextBox_OnChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox == null || textBox.Text == null) return;

            var data = textBox.Text;

            if (string.IsNullOrEmpty(data) || data == "Введите сообщение") return;

            await _apiManager.SendDataAsync(data.LastOrDefault().ToString());
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;

            textBox.Clear();
        }
    }
}
