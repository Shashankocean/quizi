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
namespace Quizi
{
    /// <summary>
    /// Interaction logic for profile_delete.xaml
    /// </summary>
    public partial class profile_delete : Window
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;

        exception exp = new exception();
        public profile_delete()
        {
            InitializeComponent();


            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=newprofile.db;Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn.Open();
                // create a new SQL command:
                sqlite_cmd = sqlite_conn.CreateCommand();
                // Lets insert something into our new table:
                sqlite_cmd.CommandText = "SELECT profilename FROM profiles;";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    ListBoxProfiles.Items.Add(sqlite_datareader[0].ToString());
                }
                sqlite_conn.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR profilelist.addlist " + exp.stack_trace(ex));
            }
        }


        private void delete_profile(object sender, SelectionChangedEventArgs e)
        {
            MessageBoxQuiz msg = new MessageBoxQuiz();
            msg.display("are you sure you want to delete: "+ListBoxProfiles.SelectedItem.ToString(),"Deleting profile");
            this.Hide();
            msg.ShowDialog();

            
            if (MessageBoxQuiz.conform())
            {
                try
                {
                    sqlite_conn = new SQLiteConnection("Data Source=newprofile.db;Version=3;New=False;Compress=True;");
                    sqlite_conn.Open();
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "DELETE FROM profiles where profilename='" + ListBoxProfiles.SelectedItem.ToString() + "' ";
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error while Removing ,profile_delete " + exp.stack_trace(ex));

                }
            }

            profileList pl = new profileList();
            
            pl.Show();

        }

        private void button_cancle_Click(object sender, RoutedEventArgs e)
        {
            profileList pl = new profileList();
            this.Hide();
            pl.Show();
           
        }
    }
}
