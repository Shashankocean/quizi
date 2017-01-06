using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Quizi
{
    class exception
    {
        StackTrace stacktrace;
        public string stack_trace(Exception ex)
        {
            stacktrace = new StackTrace(ex, true);
            string exp = ex.StackTrace.Substring(ex.StackTrace.Length - 8, 8);
            return exp;
        }
    }
}
