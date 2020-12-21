using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
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

            ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);

            string url = "http://microptik.org/software/beans/";
            bool isUpdated = false;

            if (ApplicationUpdater.Performer.Check(Application.StartupPath, url, Application.ProductVersion)) // if is updates
            {
                if (ApplicationUpdater.Performer.Msg())
                {
                    var shortPath = new System.Text.StringBuilder(255);
                    Flokal.Common.Methods.Files.GetShortPathName(Application.StartupPath, shortPath, 255);
                    string arg = url + " " + shortPath.ToString() + " " + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe" + " " + Application.ProductVersion;
                    string exe = System.IO.Path.Combine(Application.StartupPath, "ApplicationUpdater.exe");
                    Flokal.Common.Methods.Proc.ExecuteProgram(exe, arg, true);

                    isUpdated = true;
                }
            }

            if (!isUpdated)
            {
                if (true /*!cfg.isFirstLaunch*/)
                {
                    Application.Run(new FormLife());       // standart launch program
                }
                else
                {
                    Flokal.Common.Methods.Proc.GrantAccess(Application.StartupPath);
                    Application.Run(new FormSetting(true));
                    cfg.isFirstLaunch = false;
                    SettingsManager.Instance.SaveCurrent(cfg, System.Windows.Forms.Application.StartupPath);
                }
            }
            else
            {
                ApplicationUpdater.Performer.MsgFinish();
            }

        }
    }
}
