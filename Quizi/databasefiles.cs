using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizi
{
    
   public class databasefiles
    {
        public static string database, units, customize, deleted, jumble;
       
      public  databasefiles()
        {

        }
     public   databasefiles(string datab, string unit , string custom, string delete, string jumbleit)
        {

            database = datab;
            units = unit;
            customize = custom;
            deleted = delete;
            jumble = jumbleit;
        }
    }
}
