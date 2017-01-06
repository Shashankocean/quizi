using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Quizi
{
    /// <summary>
    /// Interaction logic for MessageBoxQuiz.xaml
    /// </summary>
    public partial class MessageBoxQuiz : Window
    {
        public static bool ok;

        public MessageBoxQuiz()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            ok = true;
            this.Hide();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            ok = false;
            this.Hide();
        }
        public void display(string message, string caption)
        {
            textBlock_message.Text = message;
            label_title.Content = caption;
        }
        public static bool conform()
        {
            return ok;
        }
    }
}
