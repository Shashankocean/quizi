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
    /// Interaction logic for jumble_insert.xaml
    /// </summary>
    public partial class jumble_insert : Window
    {
        SQLiteConnection sqlite_conn2;
        SQLiteCommand sqlite_cmd;
        exception exp = new exception();
        public jumble_insert()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
           
            if(e.Key==Key.Escape)
            {
                this.Hide();
            }
        }

        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textBox_jubmle_hint.Text) || String.IsNullOrWhiteSpace(textBox_jubmle_hint.Text))
            {
                MessageBox.Show("Give Hint.." + "please do");
            }
            else if (String.IsNullOrEmpty(textBox_jubmle_word.Text) || String.IsNullOrWhiteSpace(textBox_jubmle_word.Text))
            {
                MessageBox.Show("Give Word.." + "Please do");
            }
            else
            {
                try
                {
                    sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.jumble + "';Version=3;New=False;Compress=True;");
                    sqlite_conn2.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn2.CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = "INSERT INTO jumbleword VALUES ('" + textBox_jubmle_hint.Text + "','" + textBox_jubmle_word.Text + "');";
                    // Now lets execute the SQL ;D
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn2.Close();
                    MessageBoxQuiz msg = new MessageBoxQuiz();
                    msg.display("Successfully Inserted","Message");
                    msg.ShowDialog();
                    textBox_jubmle_hint.Text = "";
                    textBox_jubmle_word.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error while inserting jumble, " + exp.stack_trace(ex));
                }
            }
        }

        private void validation(object sender, RoutedEventArgs e)
        {
            if(textBox_jubmle_hint.Text == "Hint")
            {
                textBox_jubmle_hint.Text = "";
            }
            if(textBox_jubmle_word.Text=="Word")
            {
                textBox_jubmle_word.Text = "";
            }
        }
    }
}
