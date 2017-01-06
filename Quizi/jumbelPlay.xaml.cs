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
using System.Data;
using System.Data.SQLite;
using System.Windows.Threading;

namespace Quizi
{
    /// <summary>
    /// Interaction logic for jumbelPlay.xaml
    /// </summary>
    public partial class jumbelPlay : Window
    {
        exception exc = new exception();
        private DispatcherTimer timer;
        public TimeSpan TimeoutToHide { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime LastMouseMove { get; private set; }

        SQLiteConnection con = new SQLiteConnection("Data Source = '"+databasefiles.jumble+"'; Version=3;New=False;Compress=True;");

        static string[] words = new string[50];
        static string[] hints = new string[50];
        static int next = -1,length;
        static string answer;
        public jumbelPlay()
        {
            InitializeComponent();
           
            int i = 0;

            try
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("select * from jumbleword ", con);
                SQLiteDataReader dr = cmd.ExecuteReader();
                
                while(dr.Read())
                {
                    hints[i] = dr[1].ToString();
                    words[i++]=dr[2].ToString();
                    
                }
                length = i;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"unable to select: "+exc.stack_trace(ex));
            }
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            TimeoutToHide = TimeSpan.FromSeconds(5);
            this.MouseMove += new MouseEventHandler(mouse_move_event);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape)
            {
                PlayGame pg = new PlayGame();
                pg.label_score.Content = PlayGame.total_score.ToString();
                this.Hide();
            }
            if(e.Key == Key.D1)
            {
                PlayGame pg = new PlayGame();
                pg.label_score.Content = PlayGame.total_score.ToString();
                this.Hide();
            }
            if(e.Key == Key.Right)
            {
                if (next >= -1 && next < length-1)
                {
                    string s=jumble(words[++next]);
                    setwords(s);
                }
                else
                    next = -1;
            }
            if(e.Key==Key.Left)
            {
                if (next >= 1 && next < length-1)
                {
                    string s = jumble(words[--next]);
                    setwords(s);
                }
                else
                    next = 1;
            }
            if(e.Key==Key.Enter)
            {
                answer_display();
            }
            if(e.Key== Key.P)
            {
                PlayGame.total_score += PlayGame.score;
                label_score.Content = PlayGame.total_score.ToString();
            }
            if (e.Key == Key.M)
            {
                PlayGame.total_score -= PlayGame.score;
                label_score.Content = PlayGame.total_score.ToString();
            }
        }
        private string jumble(string word)
        {
            answer = word.ToUpper();
            char[] chars = new char[word.Length];
            Random rand = new Random(10000);
            int index = 0;
            textBlock_hint.Text = hints[next];
            while (word.Length > 0)
            { // Get a random number between 0 and the length of the word. 
                int next = rand.Next(0, word.Length - 1); // Take the character from the random position 
                                                          //and add to our char array. 
                chars[index] = word[next];                // Remove the character from the word. 
                word = word.Substring(0, next) + word.Substring(next + 1);
                ++index;
            }
            return new String(chars);
       

    }
        private void setwords(string jumbleWord)
        {
            Grid_jumble.ColumnDefinitions.Clear();
            Grid_jumble.Children.Clear();
            jumbleWord = jumbleWord.ToUpper();
            int row, col,i=0;
            int length = jumbleWord.Length;
            Label[] label = new Label[length];
            SolidColorBrush br = new SolidColorBrush();
            br.Color = Color.FromRgb(251, 251, 251);
            SolidColorBrush br2 = new SolidColorBrush();
            br2.Color = Color.FromRgb(20, 45, 67);

            for (row = 0; row < 1; row++)
                {
                    for (col = 0; col < length; col++)
                    {
                    ColumnDefinition gridCol1 = new ColumnDefinition();
                    Grid_jumble.ColumnDefinitions.Add(gridCol1);
                    label[i] = new Label();
                    label[i].HorizontalAlignment = HorizontalAlignment.Center;
                    label[i].VerticalAlignment = VerticalAlignment.Center;
                    label[i].FontSize = 62;
                    label[i].Background = br2;
                    label[i].Foreground = br;
                    label[i].Width = 100;
                    label[i].Content = jumbleWord[i];
                    label[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                    label[i].SetValue(Grid.RowProperty, 0);
                    label[i].SetValue(Grid.ColumnProperty, col);
                    Grid_jumble.Children.Add(label[i]);
                    i++;
                    }             
                }
        }

        private void btn_answer_click(object sender, RoutedEventArgs e)
        {

            answer_display();
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void button_score_m_Click(object sender, RoutedEventArgs e)
        {
            PlayGame.total_score -= PlayGame.score;
            label_score.Content = PlayGame.total_score.ToString();
        }

        private void button_score_p_Click(object sender, RoutedEventArgs e)
        {
            PlayGame.total_score += PlayGame.score;
            label_score.Content = PlayGame.total_score.ToString();
        }

        private void mouse_move_event(object sender, MouseEventArgs e)
        {
            LastMouseMove = DateTime.Now;
            if(IsHidden)
            {
                this.Cursor = Cursors.Arrow;
                button_score_m.Visibility = Visibility.Visible;
                button_score_p.Visibility = Visibility.Visible;
                btn_back.Visibility = Visibility.Visible;
                btn_answer.Visibility = Visibility.Visible;
                IsHidden = false;
            } 
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timertick);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            
            timer.Start();
        }

        private void timertick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - LastMouseMove;
            if(elapsed>=TimeoutToHide && !IsHidden)
            {
                this.Cursor = Cursors.None;
                button_score_m.Visibility = Visibility.Hidden;
                button_score_p.Visibility = Visibility.Hidden;
                btn_back.Visibility = Visibility.Hidden;
                btn_answer.Visibility = Visibility.Hidden;
                IsHidden = true;
            }
        }

        private void answer_display()
        {
            if (answer != null)
            {
                Grid_jumble.ColumnDefinitions.Clear();
                Grid_jumble.Children.Clear();
                int row, col, i = 0;
                int length = answer.Length;
                Label[] label = new Label[length];
                SolidColorBrush br = new SolidColorBrush();
                br.Color = Color.FromRgb(251, 251, 251);
                SolidColorBrush br2 = new SolidColorBrush();
                br2.Color = Color.FromRgb(22, 125, 164);

                for (row = 0; row < 1; row++)
                {
                    for (col = 0; col < length; col++)
                    {
                        ColumnDefinition gridCol1 = new ColumnDefinition();
                        Grid_jumble.ColumnDefinitions.Add(gridCol1);
                        label[i] = new Label();
                        label[i].HorizontalAlignment = HorizontalAlignment.Center;
                        label[i].VerticalAlignment = VerticalAlignment.Center;
                        label[i].FontSize = 62;
                        label[i].Background = br2;
                        label[i].Foreground = br;
                        label[i].Width = 100;
                        label[i].Content = answer[i];
                        label[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                        label[i].SetValue(Grid.RowProperty, 0);
                        label[i].SetValue(Grid.ColumnProperty, col);
                        Grid_jumble.Children.Add(label[i]);
                        i++;
                    }
                }
            }
        }
    }
}
