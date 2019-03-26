using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFDecoder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ToolForm());
            }
            else if (args.Length >= 2)
            {
                string jsonpath = args[0];
                string splitpath = args[1];
                string processpath = (args.Length >= 3) ? args[2] : null;
                GFDecoder.DoSplitAndProcess(jsonpath, splitpath, processpath);
            }
        }
    }
}
