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
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace Quizi
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        SQLiteConnection sqlite_connsetting;
        SQLiteCommand sqlite_cmdsetting;
        SQLiteDataReader sqlite_datareadersetting;
        exception exp;
        databasefiles dbfile = new databasefiles();

        int totaltime;
        public Settings()
        {
            InitializeComponent();
            exp = new exception();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            var gridView = new GridView();
            this.listviewControles.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Actions",
                DisplayMemberBinding = new Binding("action")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Keys",
                DisplayMemberBinding = new Binding("keys")
            });
            this.listviewControles.Items.Add(new MyItem { action = "Next question", keys = "Right arrow" });
            this.listviewControles.Items.Add(new MyItem { action = "Previous question", keys = "Left arrow" });
            this.listviewControles.Items.Add(new MyItem { action = "Option A ", keys = "A" });
            this.listviewControles.Items.Add(new MyItem { action = "Option B", keys = "B" });
            this.listviewControles.Items.Add(new MyItem { action = "Option C", keys = "C" });
            this.listviewControles.Items.Add(new MyItem { action = "Option D", keys = "D" });
            this.listviewControles.Items.Add(new MyItem { action = "Timer", keys = "T" });
            this.listviewControles.Items.Add(new MyItem { action = "Pause timer", keys = "Space" });
            this.listviewControles.Items.Add(new MyItem { action = "Jumble Window", keys = "2" });
            this.listviewControles.Items.Add(new MyItem { action = "Add score", keys = "P" });
            this.listviewControles.Items.Add(new MyItem { action = "subtract score", keys = "M" });
            this.listviewControles.Items.Add(new MyItem { action = "Exit", keys = "Esc" });

            try
            {
                if (File.Exists("customize.db") == false)//--checking, database file present or not
                {
                    using (sqlite_connsetting = new SQLiteConnection("Data Source=customize.db;Version=3;New=True;Compress=True;"))
                    {
                        sqlite_connsetting.Open();
                        sqlite_cmdsetting = sqlite_connsetting.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmdsetting.CommandText = "CREATE TABLE custom (id integer PRIMARY KEY, timer integer);";
                        // Now lets execute the SQL ;D
                        sqlite_cmdsetting.ExecuteNonQuery();
                        sqlite_connsetting.Close();
                        sqlite_connsetting = new SQLiteConnection("Data Source=customize.db;Version=3;New=False;Compress=True;");
                        // open the connection:
                        sqlite_connsetting.Open();
                        // create a new SQL command:
                        sqlite_cmdsetting = sqlite_connsetting.CreateCommand();
                        // Lets insert something into our new table:
                        sqlite_cmdsetting.CommandText = "INSERT INTO custom (id,timer) VALUES (1,30);";
                        // And execute this again ;D
                        sqlite_cmdsetting.ExecuteNonQuery();
                        this.sqlite_connsetting.Close();
                    }
                    LabelNoteTimer.Content = "Default Timer is set to 30";
                }
                else
                {
                    using (sqlite_connsetting = new SQLiteConnection("Data Source=customize.db;Version=3;New=False;Compress=True;"))
                    {
                    // open the connection:
                    sqlite_connsetting.Open();
                    // create a new SQL command:
                    sqlite_cmdsetting = sqlite_connsetting.CreateCommand();
                    // Lets insert something into our new table:
                    sqlite_cmdsetting.CommandText = "SELECT timer FROM custom WHERE id = 1";
                    // And execute this again 
                    sqlite_datareadersetting = sqlite_cmdsetting.ExecuteReader();
                    while (sqlite_datareadersetting.Read())
                    {
                        totaltime = Int32.Parse(sqlite_datareadersetting[0].ToString());//counting  number of question so that we can fetch all questions to an structure for fast access

                    }

                   this.sqlite_connsetting.Close();
                    LabelNoteTimer.Content = "Timer is set to " + totaltime.ToString() + " sec";
                }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR settings " + exp.stack_trace(ex));
            }

        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                
                Menu men = new Menu();
                men.Show();
                this.Close();

            }
        }

        private void SetTimer_click(object sender, RoutedEventArgs e)
        {
            if (TextBoxTimer.Text != "")
            {
                try
                {
                    using (sqlite_connsetting = new SQLiteConnection("Data Source=customize.db;Version=3;New=False;Compress=True;"))
                    {
                        // open the connection:
                        sqlite_connsetting.Open();
                        // create a new SQL command:
                        sqlite_cmdsetting = sqlite_connsetting.CreateCommand();
                        // Lets insert something into our new table:
                        sqlite_cmdsetting.CommandText = "UPDATE custom set timer = " + Int32.Parse(TextBoxTimer.Text) + " where id=1";
                        // And execute this again ;D
                        sqlite_cmdsetting.ExecuteNonQuery();
                        sqlite_connsetting.Close();
                    }
                    LabelNoteTimer.Content = "Timer is set to " + TextBoxTimer.Text+" sec.";
                    LabelNoteTimer.Foreground = Brushes.LightGray;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR settings" + exp.stack_trace(ex));
                }
                
            }
            else
            {
                LabelNoteTimer.Content = "* Please check the given value";
                LabelNoteTimer.Foreground = Brushes.Red;
            }
        }
        private void SetScore_click(object sender, RoutedEventArgs e)
        {
            if (TextBoxTimer.Text != "")
            {
                try
                {
                    using (sqlite_connsetting = new SQLiteConnection("Data Source=customize.db;Version=3;New=False;Compress=True;"))
                    {
                        // open the connection:
                        sqlite_connsetting.Open();
                        // create a new SQL command:
                        sqlite_cmdsetting = sqlite_connsetting.CreateCommand();
                        // Lets insert something into our new table:
                        sqlite_cmdsetting.CommandText = "UPDATE custom set score_per_ques = " + Int32.Parse(TextBoxScore.Text) + " where id=1";
                        // And execute this again ;D
                        sqlite_cmdsetting.ExecuteNonQuery();
                        sqlite_connsetting.Close();
                    }
                    LabelNoteScore.Content = "Score is set to " + TextBoxScore.Text + " .";
                    LabelNoteScore.Foreground = Brushes.LightGray;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR settings" + exp.stack_trace(ex));
                }

            }
            else
            {
                LabelNoteTimer.Content = "* Please check the given value";
                LabelNoteTimer.Foreground = Brushes.Red;
            }
        }

    }
}
