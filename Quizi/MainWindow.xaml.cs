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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Windows.Threading;
namespace Quizi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         private DispatcherTimer timer;
         int i = 0,played;  

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += timertick;
            timer.Start();

            play();
           
        }

        void timertick(object sender, EventArgs e)
        {
            if(i<3)
            {
                i++;
            }
            else
            {
                timer.Stop();
                Menu menu = new Menu();
                menu.Show();
                this.Close();
            }
        }
        private void play()
        {
            if(played==0)
            {
                
               /* MediaPlayer playme = new MediaPlayer();
                playme.Open(new Uri("C:/Users/shashank/Documents/Visual Studio 2013/Projects/Quizi/Quizi/Resources/Alaram.wav", UriKind.Absolute));
                playme.Play();*/
                
                SoundPlayer play = new SoundPlayer(playsound.K_B_C_Whistle_Tone);
                play.Play();
                played = 1;

            }
            else
            {
                Menu menu = new Menu();
                menu.Show();
                MediaElement melement = new MediaElement();
                melement.Source = new Uri("pack://siteoforigin:,,,/Resources/LockTone.wav");
                melement.Play();
            }
        }
    }
}
