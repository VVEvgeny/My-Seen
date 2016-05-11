using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MySeenLib.CultureInfoTool;

namespace My_Seen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetCulture(Properties.Settings.Default.LastLanguage);

            for (; ; )
            {
                var form = new MainForm();
                form.ShowDialog();
                try
                {
                    if (form.IsRestart)
                    {
                        continue;
                    }
                }
                catch
                {
                    // ignored
                }
                break;
            }
        }
    }
}
