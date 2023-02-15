using System;
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
