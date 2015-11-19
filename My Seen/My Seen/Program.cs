using System;
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
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            CultureInfoTool.SetCulture(Properties.Settings.Default.LastLanguage);

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
