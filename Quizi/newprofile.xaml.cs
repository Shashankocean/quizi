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
using System.IO;
namespace Quizi
{
    /// <summary>
    /// Interaction logic for newprofile.xaml
    /// </summary>
    public partial class newprofile : Window
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;

        exception exp = new exception();
        databasefiles dbfile;
        
        public newprofile()
        {
           
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            
        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape)
            {
                this.Close();
            }

        }

        private void create_click(object sender, RoutedEventArgs e)
        {
            if (TextNewProfile.Text != "-----" && TextNewProfile.Text != null && TextNewProfile.Text !="")
                try
                {

                    sqlite_conn = new SQLiteConnection("Data Source=newprofile.db;Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    // Lets insert something into our new table:
                    sqlite_cmd.CommandText = "INSERT INTO profiles (profilename) VALUES ('" + TextNewProfile.Text + "');";
                    // And execute this again ;D
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                    MessageBoxQuiz msg = new MessageBoxQuiz();
                    msg.display("Inserted Successfully","Message");
                    msg.ShowDialog();
                    dbfile = new databasefiles(TextNewProfile.Text + "database.db", TextNewProfile.Text + "units.db", "customize.db", TextNewProfile.Text + "deleted.db", TextNewProfile.Text + "jumble.db");
                    Database db = new Database();
                    db.Show();
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "ERROR new profile " + exp.stack_trace(ex));
                }
            else
                label_mssg.Visibility = Visibility.Visible;
            
        }
       
        private void clearText(object sender, RoutedEventArgs e)
        {
            TextNewProfile.Text = "";
        }
        
    }
}
