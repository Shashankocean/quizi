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
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace Quizi
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        exception exp = new exception();
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        
        public Menu()
        {
            InitializeComponent();
            GridYesNo.Visibility = System.Windows.Visibility.Hidden;
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            try
            {
                if (File.Exists("newprofile.db") == false)//--checking, database file present or not
                {
                    using (sqlite_conn = new SQLiteConnection("Data Source=newprofile.db;Version=3;New=True;Compress=True;"))
                    {
                        sqlite_conn.Open();
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "CREATE TABLE profiles (id integer PRIMARY KEY AUTOINCREMENT, profilename varchar(100) NOT NULL  UNIQUE );";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR menu.menu " + exp.stack_trace(ex));
            }

            profileList pl = new profileList();
            pl.loadprofiles();

        }
         private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
             if(e.Key==Key.Escape)
             {
                 GridYesNo.Visibility = System.Windows.Visibility.Visible;
                 GridMenu.IsEnabled = false;
             }
        }

        private void playGame_click(object sender, RoutedEventArgs e)
        {

            if (profileList.noofprofile == 0)
            {
                MessageBoxQuiz mq = new MessageBoxQuiz();
                mq.display("Create new profile", "Message");
                mq.ShowDialog();
            }
            else
            {
                
                profileList pro = new profileList();
                pro.menu = "play";
                pro.ShowDialog();
            }
           
            
        }

        private void database_click(object sender, RoutedEventArgs e)
        {
            profileList pro = new profileList();
            pro.menu = "database";
            pro.ShowDialog();
           
        }

        private void newgame_click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                newprofile newprof = new newprofile();
                newprof.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR Menu.cs " + exp.stack_trace(ex));
            }
        }

        private void Settings_click(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings();
            setting.ShowDialog();
            this.Close();
        }

        private void Exit_click(object sender, RoutedEventArgs e)
        {
            GridYesNo.Visibility = System.Windows.Visibility.Visible;
            GridMenu.IsEnabled = false; ;
        }
        private void exit()
        {
            System.Environment.Exit(1);
        }

        private void yes_click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void dontexit(object sender, RoutedEventArgs e)
        {
            GridYesNo.Visibility = System.Windows.Visibility.Hidden;
            GridMenu.IsEnabled = true;
        }

        private void aboutus_Click(object sender, RoutedEventArgs e)
        {
            aboutus about = new aboutus();
            about.ShowDialog();
        }

        
    }
}
