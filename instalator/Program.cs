using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace instalator
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

            string url            = "http://microptik.org/software/beans/";
            string programName    = "beans.exe";
            string productVersion = "0.0.0.1";
            string companyPath    = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "Microptik");
            GrantAccess(companyPath);
            string programPath    = Path.Combine(companyPath, programName.Replace(".exe",""));
            GrantAccess(programPath);

            if (ApplicationUpdater.Performer.Check(programPath, url, productVersion)) // if is updates
            {
                if (ApplicationUpdater.Performer.Msg())
                {
                    var shortPath = new System.Text.StringBuilder(255);
                    GetShortPathName(programPath, shortPath, 255);
                    string prgLink = Path.Combine(shortPath.ToString(), programName);

                    string arg = url + " " + shortPath.ToString() + " " + programName + " " + productVersion;
                    string exe = System.IO.Path.Combine(Application.StartupPath, "ApplicationUpdater.exe");
                    ExecuteProgram(exe, arg, true);

                    CreateShortcut(prgLink);
                }
            }
        }


        const int MAX_PATH = 255;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
         string path,
            [MarshalAs(UnmanagedType.LPTStr)]
         StringBuilder shortPath,
            int shortPathLength
            );


        public static void ExecuteProgram(string fileName, string arguments, bool isWait = false)
        {
            Process prc = null;
            string output = string.Empty;

            try
            {
                // Устанавливаем параметры запуска процесса
                prc = new Process();

                prc.StartInfo.FileName = fileName;
                prc.StartInfo.Arguments = arguments;
                //prc.StartInfo.CreateNoWindow = false;

                // Старт
                prc.Start();

                // Ждем пока процесс не завершится
                if (isWait) prc.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
            finally
            {
                if (prc != null) prc.Close();
            }
        }

        public static void GrantAccess(string folderNM)
        {
            bool exists = System.IO.Directory.Exists(folderNM);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(folderNM);
                Console.WriteLine("The Folder is created Sucessfully");
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
            DirectoryInfo dInfo = new DirectoryInfo(folderNM);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }

        private static void CreateShortcut(string linkName)
        {

            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fileName = Path.GetFileNameWithoutExtension(linkName);
            string scname = deskDir + "\\" + fileName + ".url";

            if (File.Exists(scname)) File.Delete(scname);

            using (StreamWriter writer = new StreamWriter(scname))
            {
                string app = linkName;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }

    }
}
