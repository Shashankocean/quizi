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
    /// Interaction logic for aboutus.xaml
    /// </summary>
    public partial class aboutus : Window
    {
        public aboutus()
        {
            InitializeComponent();
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Admin1.jpg"));
            ellipse_left.Fill = img;
            ellipse_left.Stretch = Stretch.UniformToFill;
            ImageBrush img2 = new ImageBrush();
            img2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Admin2.jpg"));
            ellipse_right.Fill = img2;
            ellipse_right.Stretch = Stretch.UniformToFill;
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

        }
        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Hide();
            }
        }

        private void btn_close_Click_1(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
