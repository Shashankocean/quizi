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
namespace Quizi
{
    /// <summary>
    /// Interaction logic for profileList.xaml
    /// </summary>
    public partial class profileList : Window
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;

        exception exp = new exception();
        public string menu;
     public  static int noofprofile;
        public profileList()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            loadprofiles();
        }
        public void loadprofiles()
        {
            try
            {

                sqlite_conn = new SQLiteConnection("Data Source=newprofile.db;Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();

                sqlite_cmd.CommandText = "SELECT count(id) FROM profiles;";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    noofprofile = Int32.Parse(sqlite_datareader["count(id)"].ToString());

                }
                sqlite_conn.Close();
                addtolist();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR profilelist.loadprofile " + exp.stack_trace(ex));
            }
        }
        private void addtolist()
        {
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"ERROR profilelist.addlist "+exp.stack_trace(ex));
            }
        }

        private void selectProfile_changed(object sender, SelectionChangedEventArgs e)
        {
            
            databasefiles.database = ListBoxProfiles.SelectedItem.ToString() + "database.db";
            databasefiles.units = ListBoxProfiles.SelectedItem.ToString() + "units.db";
            databasefiles.deleted = ListBoxProfiles.SelectedItem.ToString() + "deleted.db";
            databasefiles.customize = ListBoxProfiles.SelectedItem.ToString() + "customize.db";
            databasefiles.jumble = ListBoxProfiles.SelectedItem.ToString() + "jumble.db";

            if (menu=="database")
            {
                Database datab = new Database();
                datab.Show();
                datab.load();
                this.Close();
            }
            else if(menu=="play")
            {
                PlayGame playgame = new PlayGame();
                playgame.Show();
                this.Close();
            }
        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }

        }

        private void ButtonAddProfile_click(object sender, RoutedEventArgs e)
        {
            newprofile np = new newprofile();
            this.Close();
            np.Show();
        }

        private void ButtonDeleteProfile_click(object sender, RoutedEventArgs e)
        {
            profile_delete pd = new profile_delete();
            this.Hide();
            pd.Show();
        }
    }
}
