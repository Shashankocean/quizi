using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Quizi
{
    class Delete
    {
        SQLiteConnection sqlite_conn2;
        SQLiteConnection sqlite_conn1;
        exception exp = new exception();
        SQLiteCommand sqlite_cmd1;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        Database db = new Database();
        databasefiles dbfiles = new databasefiles();
        public Boolean check(int question, string combo_delUnit)
        {
            int check = 0;
            try
            {
                sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                sqlite_conn2.Open();
                sqlite_cmd = sqlite_conn2.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * from '" + combo_delUnit + "' where questionno = " + question + " ;";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                //sqlite_datareader = sqlite_cmd.ExecuteReader();
                // 
                // Print out the content of the text field:

                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    check++;
                }
                if (sqlite_datareader.Read() == false && check == 0)
                {
                    return false;
                }
                sqlite_conn2.Close();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no question = " + question + " in " + combo_delUnit, ex.Message.ToString() + "," + exp.stack_trace(ex).ToString());
                return false;
            }

        }

        public Boolean delete_unit(int valid, string combo_delUnit, int question)
        {
            

            if (valid == 1)
            {
                if (check(question, combo_delUnit))
                {
                    try
                    {
                        sqlite_conn1 = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                        sqlite_conn1.Open();
                        sqlite_cmd1 = sqlite_conn1.CreateCommand();
                        sqlite_cmd1.CommandText = "DELETE from '" + combo_delUnit + "' where questionno = " + question + " ;";
                        sqlite_cmd1.ExecuteNonQuery();
                        sqlite_conn1.Close();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ": Error while deleting question " + question + " from unit" + combo_delUnit, "ERROR, delete.cs" + exp.stack_trace(ex));
                        return false;
                    }
                }
                else
                    return false;
            }
            else
            {
                try
                {
                    sqlite_conn1 = new SQLiteConnection("Data Source='" + databasefiles.database + "';Version=3;New=False;Compress=True;");
                    sqlite_conn1.Open();
                    sqlite_cmd1 = sqlite_conn1.CreateCommand();
                    sqlite_cmd1.CommandText = "DELETE FROM '" + combo_delUnit + "';";
                    sqlite_cmd1.ExecuteNonQuery();
                    sqlite_conn1.Close();


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error while deleting Unit ,delete.cs " + exp.stack_trace(ex));
                    return false;
                }

                //---delete unit name from unit database---------------------------------
              /*  try
                {

                    sqlite_conn1 = new SQLiteConnection("Data Source=units.db;Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn1.Open();
                    // create a new SQL command:
                    sqlite_cmd1 = sqlite_conn1.CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd1.CommandText = "Delete from newunits where unit = '" + combo_delUnit + "';";
                    // Now lets execute the SQL ;
                    sqlite_cmd1.ExecuteNonQuery();
                    sqlite_conn1.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while deleting unit, delete.cs" + exp.stack_trace(ex));
                    return false;
                }*/
                try
                {
                    sqlite_conn2 = new SQLiteConnection("Data Source='" + databasefiles.deleted + "';Version=3;New=False;Compress=True;");
                    // open the connection:
                    sqlite_conn2.Open();
                    // create a new SQL command:
                    sqlite_cmd = sqlite_conn2.CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = "UPDATE deletedTB set del = 1 where unit = '"+combo_delUnit+"';";
                    // Now lets execute the SQL ;D
                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn2.Close();
                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(),"ERROR :"+exp.stack_trace(ex));
                    return false;
                }
            }

        }
    }
}
