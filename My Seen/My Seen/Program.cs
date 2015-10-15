using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySeenLib;

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
            //Application.Run(new Main_Form());

            CultureInfoTool.SetCulture(Properties.Settings.Default.LastLanguage);

            for (; ; )
            {
                Main_Form form = new Main_Form();
                form.ShowDialog();
                try
                {
                    if (form.isRestart)
                    {
                        continue;
                    }
                }
                catch
                {
                    break;
                }
                break;
            }
        }
    }
}
