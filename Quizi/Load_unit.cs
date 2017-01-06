using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace Quizi
{
    class Load_unit
    {
        SQLiteConnection sqlite_conn2;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        private int maxunit = 0;
        exception exp = new exception();
        Database db = new Database();
        databasefiles dbfiles = new databasefiles();

        public Load_unit(ref ComboBox combo_delUnit1,ref ComboBox combo_delUnit2,ref ComboBox Combo_select_unit,ref ComboBox combo_grid)
        {
            
            try
            {
                sqlite_conn2 = new SQLiteConnection("Data Source='"+databasefiles.units+"';Version=3;New=False;Compress=True;");
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
                
                db.deleted = new int[maxunit+2];

                for (int i = 1; i <= maxunit; i++)
                {
                    sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.deleted + "';Version=3;New=False;Compress=True;");
                    sqlite_conn2.Open();
                    sqlite_cmd = sqlite_conn2.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT del from deletedTB where id = " + i + ";";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {// Print out the content of the text field:

                        db.deleted[i] =Int32.Parse( sqlite_datareader[0].ToString());

                    }

                    sqlite_conn2.Close();

                    if (db.deleted[i] != 1)
                    {
                        sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.units + "';Version=3;New=False;Compress=True;");
                        sqlite_conn2.Open();
                        sqlite_cmd = sqlite_conn2.CreateCommand();
                        sqlite_cmd.CommandText = "SELECT unit from newunits where id=" + i + ";";
                        sqlite_datareader = sqlite_cmd.ExecuteReader();
                        while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                        {// Print out the content of the text field:

                            Combo_select_unit.Items.Add(sqlite_datareader.GetString(0));
                            combo_delUnit1.Items.Add(sqlite_datareader.GetString(0));
                            combo_delUnit2.Items.Add(sqlite_datareader.GetString(0));
                            combo_grid.Items.Add(sqlite_datareader.GetString(0));

                        }

                        sqlite_conn2.Close();
                        db.listbox_questions.Items.Add(" " + i);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error in Loading,Loadunit.cs" + exp.stack_trace(ex));
            }
            
        }
        public int trackrecord(string unit)
        {
            int maxques=0;
            
            try
            {


                sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn2.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn2.CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = "SELECT MAX(questionno) as questionno FROM '" + unit + "';";

                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through the result lines:
                    // Print out the content of the text field:

                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {
                        if (sqlite_datareader[0].ToString() != "")
                            maxques = sqlite_datareader.GetInt32(0);
                    }

                    sqlite_conn2.Close();

                    return maxques;
                
            }
            catch(Exception ex)
                {
                    MessageBox.Show(unit+" missing : either it is deleted or not created","ERROR load_unit "+exp.stack_trace(ex));
                     return 0;
                }
                
        }

    }
}
