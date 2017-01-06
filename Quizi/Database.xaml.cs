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
    /// Interaction logic for Database.xaml
    /// </summary>
    public partial class Database : Window
    {
        private int questionno = 0;
        private string unit;
        char answer;
        public int[] deleted;
        //Data connection-----
        SQLiteConnection sqlite_conn, sqlite_conn2, sqlite_conn3;
        SQLiteCommand sqlite_cmd, sqlite_cmd3;
        SQLiteDataReader sqlite_datareader;
        //class-----
        exception exp = new exception();
        databasefiles dbfile = new databasefiles();

        SQLiteDataAdapter adapter;
        SQLiteCommandBuilder commandBuilder;
        DataTable mtable;
        Menu menu;
        public Database()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnButtonKeypress);

            try
            {
                if (File.Exists(databasefiles.units) == false)//--checking, unit database file present or not
                {
                    //MessageBox.Show("db not there");
                    using (sqlite_conn3 = new SQLiteConnection("Data Source='"+databasefiles.units+"';Version=3;New=True;Compress=True;")) 
                    {  // open the connection:
                        sqlite_conn3.Open();
                        // create a new SQL command:
                        sqlite_cmd3 = sqlite_conn3.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd3.CommandText = "CREATE TABLE newunits (id integer PRIMARY KEY, unit varchar(100) UNIQUE);";
                        // Now lets execute the SQL ;D
                        sqlite_cmd3.ExecuteNonQuery();
                        sqlite_conn3.Close();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
                if (File.Exists(databasefiles.database) == false)//--checking, database file present or not
                    {
                        using (sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=True;Compress=True;"))
                        {
                            sqlite_conn.Open();
                            sqlite_conn.Close();
                        }
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                if (File.Exists(databasefiles.deleted) == false)//--checking, database file present or not
                {
                    using (sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.deleted + "';Version=3;New=True;Compress=True;"))
                    {  // open the connection:
                        sqlite_conn2.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "CREATE TABLE deletedTB (id integer PRIMARY KEY, unit varchar(100) , del integer );";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn2.Close();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                if (File.Exists(databasefiles.jumble) == false)//--checking, database file present or not
                {
                    using (sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.jumble + "';Version=3;New=True;Compress=True;"))
                    {  // open the connection:
                        sqlite_conn2.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "CREATE TABLE jumbleword (id integer PRIMARY KEY, question varchar(200) , word varchar(100) );";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn2.Close();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                if (File.Exists(databasefiles.customize) == false)//--checking, database file present or not
                {
                    using (sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.customize + "';Version=3;New=True;Compress=True;"))
                    {
                        sqlite_conn.Open();
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "CREATE TABLE custom (id integer PRIMARY KEY, timer integer, score_per_ques integer);";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    using (sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.customize + "';Version=3;New=False;Compress=True;"))
                        // open the connection:
                        {sqlite_conn2.Open();

                    
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        // Lets insert something into our new table:
                        sqlite_cmd.CommandText = "INSERT INTO custom (id,timer,score_per_ques) VALUES (1,30,10);";
                        // And execute this again ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn2.Close();
                         }
                       GC.Collect();
                       GC.WaitForPendingFinalizers();
                    
                }
                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"ERROR Database.cs"+exp.stack_trace(ex));
            }
            
           
        }
        public void load()
        {
            
            loadunit();//--if file present, loading units from the unit database and listing units to combobox
        }
        
        private void OnButtonKeypress(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Escape)
            {


                menu = new Menu();
                this.Close();
            }

        }
        private void Bttn_Insert_Click(object sender, RoutedEventArgs e)
        {
            
            checkans();
            bool emptyc=false;
            string valid = null;
            valid = " " + validation();
            if (valid == " true")
            {
                
                try
                {
                    unit = Combo_select_unit.SelectedItem.ToString();
                    sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    //-----  sqlite_cmd.CommandText = "CREATE TABLE '"+unit+"'(questionno integer,question varchar(100),optionA varchar(100),optionB varchar(100),optionC varchar(100),optionD varchar(100),ans varchar(10)));";
                    // Now lets execute the SQL ;D
                    //-- sqlite_cmd.ExecuteNonQuery();

                    // But how do we read something out of our table ?
                    // First lets build a SQL-Query again:
                    sqlite_cmd.CommandText = "SELECT MAX(questionno) FROM '" + unit + "';";
                    // Now the SQLiteCommand object can give us a DataReader-Object:
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through the result lines:

                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {

                        if (sqlite_datareader[0].ToString() == "")
                        {
                            questionno = 0;
                            emptyc = true;
                        }
                        else
                        {
                            questionno = sqlite_datareader.GetInt32(0);
                            emptyc = false;
                        }
                    }
                    if (emptyc == true)
                    {
                        sqlite_conn.Close();
                        insertdata();
                    }
                    else
                    {
                        sqlite_conn.Close();
                        insertdata();
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error while checking Max, " + exp.stack_trace(ex));
                }
                
            }
            else
                MessageBox.Show(valid, "Fill the given Fields");

        }
        private void insertdata()
        {
            var result = "Yes";// MessageBox.Show("do you want to insert", "Conformation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            try
            {
                if (result.ToString() == "Yes")
                {
                    questionno++;
                    sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    // Lets insert something into our new table:
                    sqlite_cmd.CommandText = "INSERT INTO '" + unit + "' (questionno,question,optionA,optionB,optionC,optionD,ans) VALUES (" + questionno + ",'" + Text_question.Text + "','" + Text_opt_A.Text + "','" + Text_opt_B.Text + "','" + Text_opt_C.Text + "','" + Text_opt_D.Text + "','" + answer + "');";
                    // And execute this again ;D
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                    LabelSuccesInsert.Visibility = System.Windows.Visibility.Visible;
                    loadunit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message+": Unable to insert","ERROR "+exp.stack_trace(ex));
            }
        }
      
        private string validation()
        {//--cheking wther user has done one of these exception or not 
            if(Text_question.Text=="")
                return ("question") ;
            if (Text_opt_A.Text == "")
                return ("option A");
            if (Text_opt_B.Text == "")
                return ("option B");
            if (Text_opt_C.Text == "")
                return ("option C");
            if (Text_opt_D.Text == "")
                return ("option D");
            if (Radio_opt_A.IsChecked == false && Radio_opt_B.IsChecked == false && Radio_opt_C.IsChecked == false && Radio_opt_D.IsChecked == false)
                return ("Answers");
            if (Combo_select_unit.Text=="Select Unit")
                return ("select Unit");
            if(Combo_select_unit.Text=="")
                return ("Select Unit");
            if (Combo_select_unit.SelectedIndex==-1)
                return ("Select Unit");
               
           else
                return ("true");
        }
        private void checkans()
        {
            if (Radio_opt_A.IsChecked == true)
                answer = 'a';
            if (Radio_opt_B.IsChecked == true)
                answer = 'b';
            if (Radio_opt_C.IsChecked == true)
                answer = 'c';
            if (Radio_opt_D.IsChecked == true)
                answer = 'd';
            
        }

        private void Combo_select_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Combo_select_unit.Text = Combo_select_unit.SelectedItem.ToString();
            
        }

        private void Bttn_Add_new_unit_Click(object sender, RoutedEventArgs e)
        {
            int maxunit=0;
            try
                {
                if (Text_new_unit.Text == "" )
                {
                    MessageBox.Show("Input field can't be Empty");
                    return;

                }
                else if( Convert.ToInt32(Text_new_unit.Text) < 0 || Convert.ToInt32(Text_new_unit.Text) > 100)
                {
                    MessageBox.Show(" Input Number Between 1 - 100 ","Warning",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Information);
                    return;
                }
                    int check=Convert.ToInt32(Text_new_unit.Text);
                }
                catch(Exception ex)
            {
                MessageBox.Show(" Input Number Between 1 - 100: "+ex.Message,"Warning");
                return;
                }
            try
            {


                sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.units + "';Version=3;New=False;Compress=True;");
                // open the connection:
                sqlite_conn2.Open();
                // create a new SQL command:
                sqlite_cmd = sqlite_conn2.CreateCommand();
                // Let the SQLiteCommand object know our SQL-Query:
                sqlite_cmd.CommandText = "SELECT MAX(id) as id FROM newunits";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through the result lines:
                // Print out the content of the text field:

                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {

                    if (sqlite_datareader[0].ToString() != "")
                        maxunit = sqlite_datareader.GetInt32(0);
                    
                }

                sqlite_conn2.Close();

                    //--------------creating record of units------------------
                    int newunit = Convert.ToInt32(Text_new_unit.Text);
                if(maxunit < newunit)
                {
                    
                }
                else
                {
                    MessageBox.Show("Units already Created");
                    return;
                }
                    for (int k = maxunit+1; k <= newunit; k++)
                    {
                        string unit_text = "unit_" + k;
                        sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.units + "';Version=3;New=False;Compress=True;");
                        // open the connection:
                        sqlite_conn2.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "INSERT INTO newunits (id,unit) VALUES (" + k + ",'" + unit_text + "');";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn2.Close();

                        sqlite_conn = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                        sqlite_conn.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "CREATE TABLE '" + unit_text + "' (questionno integer PRIMARY KEY,question varchar(100) UNIQUE,optionA varchar(100) NOT NULL,optionB varchar(100) NOT NULL,optionC varchar(100) NOT NULL,optionD varchar(100) NOT NULL,ans varchar(10) NOT NULL);";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();

                        sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.deleted + "';Version=3;New=False;Compress=True;");
                        // open the connection:
                        sqlite_conn2.Open();
                        // create a new SQL command:
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        // Let the SQLiteCommand object know our SQL-Query:
                        sqlite_cmd.CommandText = "INSERT INTO deletedTB (id,unit,del) VALUES (" + k + ",'" + unit_text + "', 0);";
                        // Now lets execute the SQL ;D
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn2.Close();
                    }

                    clear_combo();
                    loadunit();
                
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unit already Created", "Error: " + ex.Message+": " + exp.stack_trace(ex));
            }
            
          
        }
        private void loadunit()
        {
            int r;
            clear_combo();
            listbox_questions.Items.Clear();
            listbox_units.Items.Clear();
            Load_unit load_unit = new Load_unit(ref combo_delUnit1,ref combo_delUnit2,ref Combo_select_unit,ref combo_grid);

            Label_total_units.Content = Combo_select_unit.Items.Count.ToString();
            for (r = 0; r < Combo_select_unit.Items.Count;r++ )
            {
                listbox_units.Items.Add(Combo_select_unit.Items[r].ToString());
                listbox_questions.Items.Add(load_unit.trackrecord(Combo_select_unit.Items[r].ToString()));
            }

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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Delete1_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                int delques = Convert.ToInt32(text_quesdel.Text);
                Delete del = new Delete();
               bool ok= del.delete_unit(1, combo_delUnit1.SelectedItem.ToString(), delques);
               
               //combo_delUnit1.Items.Clear();
               
               if (ok)
               {
                   LabelSuccesfuldelQues.Visibility = System.Windows.Visibility.Visible;
                   loadunit();
               }
               else
                   MessageBox.Show("Question number doesn't exsist","ERROR");
            }
            catch(Exception ex)
            {
                
                MessageBox.Show("Input a question Number: " + ex.Message.ToString(), "ERROR," + exp.stack_trace(ex));
            }
            
        }

        private void Button_delete2_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Delete del = new Delete();
                bool ok = del.delete_unit(2, combo_delUnit2.SelectedItem.ToString(), 0);
                combo_delUnit2.Items.Clear();
                clear_combo();
                if (ok)
                {
                    Labeldeletedunit.Visibility = System.Windows.Visibility.Visible;
                    loadunit();
                }
           }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + ":", "ERROR, " + exp.stack_trace(ex));
            }
        }
        
        private void combo_delUnit1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            label_quesdel_unit.Content = "INPUT QUESTION NUMBER FOR DELETE";
        }

        private void combo_delUnit2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Labeldeletedunit.Visibility = System.Windows.Visibility.Hidden;
        }

        private void question_numb(object sender, TextChangedEventArgs e)
        {

        }

        public void clear_combo()
        {
            try
            {
                InitializeComponent();
                combo_delUnit1.SelectedIndex = -1;
                
                combo_delUnit1.Items.Clear();
                combo_delUnit2.Items.Clear();
                combo_grid.Items.Clear();
                Combo_select_unit.Items.Clear();

                combo_delUnit1.Text = "Select Unit";
                combo_delUnit2.Text = "Select Unit";
                combo_grid.Text = "Select unit";
                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("error while clearing combo"+ex.Message,"ERROR,"+exp.stack_trace(ex));
            }
           }


        
        private void refresh(object sender, RoutedEventArgs e)
        {
            if(combo_grid.SelectedItem==null)
                return;
            sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                    sqlite_conn2.Open();
                    sqlite_cmd = sqlite_conn2.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * from "+combo_grid.SelectedItem.ToString()+";";
                   // sqlite_cmd.CommandText = "SELECT * from newunits;";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {// Print out the content of the text field:

                    }
                    
            adapter = new SQLiteDataAdapter(sqlite_cmd.CommandText,sqlite_conn2);
            mtable = new DataTable();
            mtable.TableName = combo_grid.SelectedItem.ToString();
            adapter.Fill(mtable);
           if (mtable.Columns.Contains("questionno"))
            {
                mtable.Columns["questionno"].ReadOnly = true;
            }

            if (mtable.Rows.Count == 0)
            {
                mtable.Rows.Add(new object[mtable.Columns.Count]);
            }
            
            DGrid.ItemsSource = mtable.DefaultView;
            
        }

        private void Button_save_click(object sender, RoutedEventArgs e)
        {
            try
            {
                commandBuilder = new SQLiteCommandBuilder(adapter);
                if (adapter == null) // If No Table Selected.
                    return;
                adapter.Update(mtable);
                sqlite_conn2.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"ERROR"+exp.stack_trace(ex));
            }
        }

       
        

        private void cell_edit(object sender, DataGridCellEditEndingEventArgs e)
        {
          //  MessageBox.Show(DGrid.CurrentColumn.DisplayIndex.ToString());
        }

        private void Text_question_GotFocus(object sender, RoutedEventArgs e)
        {
            Text_question.Text = "";
            LabelSuccesInsert.Visibility= System.Windows.Visibility.Hidden;
        }
        private void Text_optionA_GotFocus(object sender, RoutedEventArgs e)
        {
            Text_opt_A.Text = "";

        }
        private void Text_optionB_GotFocus(object sender, RoutedEventArgs e)
        {
            Text_opt_B.Text = "";

        }

        private void jumble_insert_click(object sender, RoutedEventArgs e)
        {
            jumble_insert jin = new jumble_insert();
            jin.ShowDialog();
        }

        private void btn_Jumble_Click(object sender, RoutedEventArgs e)
        {
            sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.jumble + "';Version=3;New=False;Compress=True;");
            sqlite_conn2.Open();
            sqlite_cmd = sqlite_conn2.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * from jumbleword";
            // sqlite_cmd.CommandText = "SELECT * from newunits;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {// Print out the content of the text field:

            }

            adapter = new SQLiteDataAdapter(sqlite_cmd.CommandText, sqlite_conn2);
            mtable = new DataTable();
            mtable.TableName = "jumbleword";
            adapter.Fill(mtable);
            
            if (mtable.Rows.Count == 0)
            {
                mtable.Rows.Add(new object[mtable.Columns.Count]);
            }

            DGrid.ItemsSource = mtable.DefaultView;
        }

        private void Text_optionC_GotFocus(object sender, RoutedEventArgs e)
        {
            Text_opt_C.Text = "";

        }
        private void Text_optionD_GotFocus(object sender, RoutedEventArgs e)
        {
            Text_opt_D.Text = "";

        }

        private void text_quesdel_gotfocus(object sender, RoutedEventArgs e)
        {
            text_quesdel.Text ="";
            LabelSuccesfuldelQues.Visibility = System.Windows.Visibility.Hidden;
        }
        

        
        
    }
}
