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

using System.Windows.Threading;
using System.Data;
using System.Data.SQLite;
using System.Media;
using System.Threading;

namespace Quizi
{
    /// <summary>
    /// Interaction logic for PlayGame.xaml
    /// </summary>
    public partial class PlayGame : Window
    {//--Data bases
        
        int timervalue , next = 0, noofunits, noquestion, curr_unit = 1, j , del = 1,level = 1, checkans, lockedA = 1, lockedB = 1, lockedC = 1, lockedD = 1,lockedT=1, chckans;
        public static int score = 0,total_score=0;
        double totaltime=30;
        int[,] noques_unit;
        int[] deleted, doneunit;
        private DispatcherTimer timer;
        private DispatcherTimer timer2;
        public TimeSpan TimeoutToHide { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime LastMouseMove { get; private set; }
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        exception exp = new exception();
        Database db = new Database();
        ButtonBackground bbg = new ButtonBackground();
        playsounds play = new playsounds();
        SoundPlayer playclock,playalaram;

        private void activated_event(object sender, EventArgs e)
        {
            label_score.Content = total_score.ToString();
        }

        private void button_score_p_Click(object sender, RoutedEventArgs e)
        {
            total_score += score;
            label_score.Content = total_score.ToString();
        }

        private void button_score_m_Click(object sender, RoutedEventArgs e)
        {
            total_score -= score;
            label_score.Content = total_score.ToString();
        }

        private void gotfocus(object sender, RoutedEventArgs e)
        {
            label_score.Content = total_score.ToString();
        }

        private void button_jumble_Click(object sender, RoutedEventArgs e)
        {
            jumbelPlay jumbl = new jumbelPlay();
            jumbl.ShowDialog();
        }

        public struct mcq
        {
            public string question, optionA, optionB, optionC, optionD;
        };
        mcq[] obj;
        string[, ,] arr; 
        public PlayGame()
        {
            InitializeComponent();
            buttonBackgroundDefault();
            countquestions();
            TimeoutToHide = TimeSpan.FromSeconds(5);
            this.MouseMove += new MouseEventHandler(mouse_move_event);
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            playclock = new SoundPlayer(playsound.Ticking_2);
            playalaram = new SoundPlayer(playsound.Alaram);

        }

        private void mouse_move_event(object sender, MouseEventArgs e)
        {
            LastMouseMove = DateTime.Now;
            if (IsHidden)
            {
                this.Cursor = Cursors.Arrow;
                button_jumble.Visibility = Visibility.Visible;
                button_score_m.Visibility = Visibility.Visible;
                button_score_p.Visibility = Visibility.Visible;
                IsHidden = false;
            }
            timer2 = new DispatcherTimer();
            timer2.Tick += new EventHandler(timertick2);
            timer2.Interval = new TimeSpan(0, 0, 0,1);
            timer2.Start();
        }

        private void timertick2(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - LastMouseMove;
            if (elapsed >= TimeoutToHide && !IsHidden)
            {
                this.Cursor = Cursors.None;
                button_jumble.Visibility = Visibility.Hidden;
                button_score_m.Visibility = Visibility.Hidden;
                button_score_p.Visibility = Visibility.Hidden;
                IsHidden = true;
            }
        }

        public void buttonBackgroundDefault()
        {
            Grid_A.Background = bbg.brushtrueA_C_Default();
            Grid_C.Background = bbg.brushtrueA_C_Default();
            Grid_B.Background = bbg.brushtrueB_D_Default();
            Grid_D.Background = bbg.brushtrueB_D_Default();
        }
        public void countquestions()
        {
            
           // MessageBox.Show("Database Loading......", "Loading..");
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source='"+databasefiles.units+"';Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();

                sqlite_cmd.CommandText = "SELECT count(id) FROM newunits;";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    noofunits = Int32.Parse(sqlite_datareader["count(id)"].ToString());//counting  number of question so that we can fetch all questions to an structure for fast access

                }
                sqlite_conn.Close();
                //MessageBox.Show("number of units = "+noofunits);
                deleted = new int[noofunits+1];
                doneunit = new int[noofunits + 1];
                sqlite_conn = new SQLiteConnection("Data Source='"+databasefiles.deleted+"';Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();

                sqlite_cmd.CommandText = "SELECT del FROM deletedTB ;";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    deleted[del] = Int32.Parse(sqlite_datareader[0].ToString());//counting  number of question so that we can fetch all questions to an structure for fast access
                    del++;
                }
                sqlite_conn.Close();
                sqlite_conn = new SQLiteConnection("Data Source=customize.db;Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();

                sqlite_cmd.CommandText = "SELECT * FROM custom where id=1 ;";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    totaltime = Int32.Parse(sqlite_datareader[1].ToString());
                    score = Int32.Parse(sqlite_datareader[2].ToString());


                }
                sqlite_conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR " + exp.stack_trace(ex));
            }
            db.load();
            loadmcq(noofunits);
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += timertick;
            
        }

        void loadmcq(int unitnos)
        {
            string unit;
            arr = new string[50, 7, unitnos + 1];
            noques_unit = new int[unitnos + 1, 2];

            obj = new mcq[10];
            try
            {
                for (int j = 1; j <= unitnos; j++)
                {
                   if(deleted[j]!=1)
                   {unit = "unit_" + j;
                    sqlite_conn = new SQLiteConnection("Data Source='"+databasefiles.database+"';Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn.Open();
                    sqlite_cmd = sqlite_conn.CreateCommand();

                    sqlite_cmd.CommandText = "SELECT count(questionno) FROM '" + unit + "';";

                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (sqlite_datareader["count(questionno)"].ToString() == "")
                            noquestion = 0;
                        else
                        {
                            noquestion = Int32.Parse(sqlite_datareader["count(questionno)"].ToString());//counting  number of question so that we can fetch all questions to an array Of string for fast access
                            noques_unit[j, 1] = noquestion;
                            //MessageBox.Show("number of question in unit "+j+" = " + noquestion);

                        }
                    }

                    sqlite_conn.Close();



                    for (int i = 1; i <= noquestion; i++)
                    {
                        sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                        // open the connection:
                        sqlite_conn.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "select * from '" + unit + "' where questionno= " + i + " ;";
                        // Now lets execute the SQL ;D
                        sqlite_datareader = sqlite_cmd.ExecuteReader();

                        while (sqlite_datareader.Read())
                        {
                            arr[i, 1, j] = (string)sqlite_datareader["question"];
                            arr[i, 2, j] = (string)sqlite_datareader["optionA"];
                            arr[i, 3, j] = (string)sqlite_datareader["optionB"];
                            arr[i, 4, j] = (string)sqlite_datareader["optionC"];
                            arr[i, 5, j] = (string)sqlite_datareader["optionD"];
                            arr[i, 6, j] = (string)sqlite_datareader["ans"];
                        }
                        sqlite_conn.Close();
                    }
                   }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() , " ERROR in Loading....MCQ(loadmcq)" + exp.stack_trace(ex ));
            }

        }

        void timertick(object sender, EventArgs e)
        {
            j = timervalue;
            timervalue--;

            if (timervalue > 90)
            {
                j = j / 100;
                label_timer.Content = j.ToString();
                double t = (timervalue / totaltime) * 100f;
                progressb.Value = Math.Round(t, 2) - 0.01;
             }
            else
            {
                
                    label_timer.Content = "0";
                    timer.Stop();
                    progressb.Value = 0.0;
                    playalaram.Play();
                    playclock.Stop();
                    lockedT = 1;
                
            }
            
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int width = (int)Grid_question.ActualWidth;
            int dic = (width / 2) - 6;
            Grid_A.Width = dic;
            Grid_B.Width = dic;
            Grid_C.Width = dic;
            Grid_D.Width = dic;

            
            
        }
        
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (noofunits != 0)
            {
                if(e.Key==Key.D2)
                {
                    jumbelPlay jp = new jumbelPlay();
                    jp.label_score.Content = total_score.ToString();
                    jp.ShowDialog();
                    
                }
                if (e.Key == Key.Right)
                {
                    buttonBackgroundDefault();
                    if (next >= 0)
                    {
                        ++next;
                        checkans = next;
                        display(next);
                    }
                    else
                    {
                        next = 1;
                        checkans = next;
                    }
                }
                if (e.Key == Key.Left)
                {
                    
                    LabelFinish.Visibility = System.Windows.Visibility.Hidden;
                    buttonBackgroundDefault();
                    if (next > 1)
                    {
                        --next;
                        checkans = next;
                        displayback(next);
                    }
                    else if (next == 1)
                    {

                        if (curr_unit > 1)
                        {
                            
                            level--;
                            curr_unit--;
                            next = noques_unit[curr_unit, 1] + 1;
                            --next;
                            checkans = next;
                            displayback(next);
                        }
                    }
                    else
                    {
                        if (curr_unit != 1 && curr_unit >0)
                        {
                            
                            level--;
                            curr_unit--;
                            next = noques_unit[curr_unit, 1] + 1;
                            --next;
                            checkans = next;
                            displayback(next);
                        }
                    }

                }
                if(e.Key==Key.R)
                {
                    timervalue = (int)totaltime * 100;
                    timer.Stop();
                    progressb.Value = 0.0;
                    label_timer.Content = "";
                    playclock.Stop();
                    lockedT = 1;
                    
                }
                if (e.Key == Key.T)
                {
                    
                    if (lockedT==1)
                    {
                        timervalue = (int)totaltime * 100;
                        timer.Start();
                        playclock.PlayLooping();
                        lockedT = 0;
                    }
                    else
                    {
                        timer.Stop();
                        playclock.Stop();
                        lockedT = 1;
                    }
                }
                if (e.Key == Key.A)
                {
                    if (lockedA == 1)
                    {
                        lockedA = 0;
                        Grid_A.Background = bbg.brushtrueA_C_Locked();
                        play.playlock();

                    }
                    else
                    {
                        if (checkanswer(checkans, "a") == true)
                        {
                            Grid_A.Background = bbg.brushtrueA_C_true();
                            total_score += score;
                            label_score.Content = total_score.ToString();
                        }
                        else
                        {
                            Grid_A.Background = bbg.brushtrueA_C_false();
                        }
                        lockedA = 1;
                    }
                }
                if (e.Key == Key.B)
                {
                    if (lockedB == 1)
                    {
                        lockedB = 0;
                        Grid_B.Background = bbg.brushtrueB_D_Locked();
                        play.playlock();
                        
                    }
                    else
                    {

                        if (checkanswer(checkans, "b") == true)
                        {
                            Grid_B.Background = bbg.brushtrueB_D_true();
                            total_score += score;
                            label_score.Content = total_score.ToString();
                        }
                        else
                        {
                            Grid_B.Background = bbg.brushtrueB_D_false();
                        }
                        lockedB = 1;
                    }
                }
                if (e.Key == Key.C)
                {
                    if (lockedC == 1)
                    {
                        Grid_C.Background = bbg.brushtrueA_C_Locked();
                        play.playlock();
                        lockedC = 0;
                    }
                    else
                    {

                        if (checkanswer(checkans, "c") == true)
                        {
                            Grid_C.Background = bbg.brushtrueA_C_true();
                            total_score += score;
                            label_score.Content = total_score.ToString();
                        }

                        else
                        {
                            Grid_C.Background = bbg.brushtrueA_C_false();
                        }
                        lockedC = 1;
                    }
                }
                if (e.Key == Key.D)
                {
                    if (lockedD == 1)
                    {
                        Grid_D.Background = bbg.brushtrueB_D_Locked();
                        play.playlock();
                        lockedD = 0;
                    }
                    else
                    {

                        if (checkanswer(checkans, "d") == true)
                        {
                            Grid_D.Background = bbg.brushtrueB_D_true();
                            total_score += score;
                            label_score.Content = total_score.ToString();
                        }
                        else
                        {
                            Grid_D.Background = bbg.brushtrueB_D_false();
                        }
                        lockedD = 1;
                    }
                }
                if (e.Key == Key.P)
                {
                    total_score += score;
                    label_score.Content = total_score.ToString();
                }
                if (e.Key == Key.M)
                {
                    total_score -= score;
                    label_score.Content =total_score.ToString();
                }


            }
            if(e.Key==Key.Escape)
            {
                playclock.Stop();
                this.Close();

            }

        }
        private bool doneunits(int check)
        {
            for(int index=1;index<noofunits+1;index++)
            {
                if (doneunit[index] == check)
                    return true;
            }
            return false;
        }
        private void display(int n)
        {
            
            if (noofunits != 0)
            {
                if (deleted[curr_unit] != 1)
                {
                    if (doneunits(curr_unit)==false )
                    {
                        Label[] label = new Label[noofunits + 4];
                        label[level] = new Label();
                        label[level].Content = "LEVEL" + level;
                        label[level].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        label[level].FontSize = 18;
                        label[level].Foreground = System.Windows.Media.Brushes.AliceBlue;
                        label[level].FontWeight = System.Windows.FontWeights.Bold;
                        Grid[] grid = new Grid[noofunits + 1];
                        grid[level] = new Grid();
                        grid[level].Name = "grid" + level;
                        grid[level].Background = bbg.levelBackground();
                        grid[level].Children.Add(label[level]);
                        StackPanelLevel.Children.Add(grid[level]);
                        doneunit[curr_unit] = curr_unit;
                    }

                }


                else
                {
                    if (deleted.Length-1 > curr_unit)
                    {
                        curr_unit++;
                        display(n);
                    }
                }
            }
            //var obj = new mcq[noofquest];
            if (n < noques_unit[curr_unit, 1])
            {
                chckans = curr_unit;
                try
                {
                    //MessageBox.Show("current unit =" + j + " noques_unit =" + noques_unit[j, 1] + " question =" + n + ":" + arr[n, 1, j]);
                    // MessageBox.Show(n.ToString());
                    Lable_ques.Content = arr[n, 1, curr_unit];
                    Label_A.Content = arr[n, 2, curr_unit];
                    Label_B.Content = arr[n, 3, curr_unit];
                    Label_C.Content = arr[n, 4, curr_unit];
                    Label_D.Content = arr[n, 5, curr_unit];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " ERROR in display()");
                }

            }
            else if (n == noques_unit[curr_unit, 1])
            {
                chckans = curr_unit;
                Lable_ques.Content = arr[n, 1, curr_unit];
                Label_A.Content = arr[n, 2, curr_unit];
                Label_B.Content = arr[n, 3, curr_unit];
                Label_C.Content = arr[n, 4, curr_unit];
                Label_D.Content = arr[n, 5, curr_unit];

                if (curr_unit < noofunits)
                {

                    curr_unit++;
                    next = 0;
                   // MessageBox.Show("welcome to next unit = " + curr_unit);
                    level++;
                }
                else if (curr_unit == noofunits)
                {
                   // MessageBox.Show("END Of The Game");
                    LabelFinish.Visibility = System.Windows.Visibility.Visible;

                    next = noques_unit[curr_unit, 1];
                    MessageBox.Show("score =" + total_score);
                }
            }
            else
            {
                if (curr_unit < noofunits)
                {

                    curr_unit++;
                    next = 0;
                   // MessageBox.Show("welcome to next unit = " + curr_unit);
                    
                    level++;
                }
                else if (curr_unit == noofunits)
                {
                    curr_unit--;// for loading last units last question
                   // MessageBox.Show("END Of The Game");
                    LabelFinish.Visibility = System.Windows.Visibility.Visible;
                    next = noques_unit[curr_unit, 1];
                    MessageBox.Show("score =" + total_score);

                }
            }

        }

        private void displayback(int n)
        {
            int j = curr_unit;

            //var obj = new mcq[noofquest];
            if (n >= 1 && noofunits!=0)
            {
                try
                {
                    // MessageBox.Show("current unit =" + j + " noques_unit =" + noques_unit[j, 1] + " question =" + n + ":" + arr[n, 1, j]);
                    // MessageBox.Show(n.ToString());
                    Lable_ques.Content = arr[n, 1, j];
                    Label_A.Content = arr[n, 2, j];
                    Label_B.Content = arr[n, 3, j];
                    Label_C.Content = arr[n, 4, j];
                    Label_D.Content = arr[n, 5, j];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " ERROR "+exp.stack_trace(ex));
                }

            }
       }
        private Boolean checkanswer(int ques,string option)
        {
            string optioncheck = option.ToUpper();
            try
            {


                if (arr[ques, 6, chckans] == option || arr[ques, 6, chckans] == optioncheck)
                {
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error "+exp.stack_trace(ex));
                return false;
            }
        }
        
    }
}
