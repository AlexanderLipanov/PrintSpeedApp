using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_OnChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;


            Console.WriteLine(textBox.Text);
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox? textBox = sender as TextBox;

            if (textBox is null) return;

            textBox.Clear();
        }
    }
}
