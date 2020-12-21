using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BuckSoft.Controls.FtpBrowseDialog;

namespace publisher
{
    public partial class Form1 : Form
    {
        string url = "ftp://microptik.org";
        string ftpUsername = "programer@microptik.org";
        string ftpPassword = "r4aFS4Haz5OmsmmtQ8Qk";
        string version;

        const string ZIPEXT = ".zip";
        const string DOWNLOADNM = "download";
        const string BACKUPNM = "backup";


        public Form1()
        {
            InitializeComponent();
            textBox2.Text = Path.Combine(Application.StartupPath, "beans.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FtpBrowseDialog ftpFolderBrowsed = new FtpBrowseDialog(url.Replace("ftp://", ""),"", 21, ftpUsername, ftpPassword,true);
            if(ftpFolderBrowsed.ShowDialog()== DialogResult.OK)
            {
                textBox1.Text = ftpFolderBrowsed.SelectedFileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // set ver
            version = get_curr_ver(textBox2.Text);

            // zip
            label3.Text = "wait, ziping ...";
            string localFile = Backup(Path.GetDirectoryName(textBox2.Text), version);
            string updateFile = CreateLocalVersionFile(Path.GetDirectoryName(localFile), "updateVersion.txt", version);

            // upload
            label3.Text = "wait, uploading ...";
            Application.DoEvents();

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                string url_full = url + "/" + textBox1.Text + "/" + version + ZIPEXT; 
                client.UploadFile(url_full, WebRequestMethods.Ftp.UploadFile, localFile);

                url_full = url + "/" + textBox1.Text + "/" + "updateVersion.txt";
                client.UploadFile(url_full, WebRequestMethods.Ftp.UploadFile, updateFile);
            }

            label3.Text = "done !!!";
            timer1.Start();
        }

        string  get_curr_ver(string fn)
        {
            string res = "0.0.0.0"; 
            if(File.Exists(fn))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fn);
                res = fvi.FileVersion;
            }
            return res;
        }





        public string  Backup(string appPath, string oldVer)
        {

            string  res = oldVer+ZIPEXT ;

            string tmpPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            string backupRootPath = Path.Combine(appPath, BACKUPNM);
            bool wasBackup = false;
            string downloadRootPath = Path.Combine(appPath, DOWNLOADNM);
            bool wasDownload = false;

            try
            {
                {
                    if (Directory.Exists(backupRootPath))
                    {
                        this.CopyFolder(backupRootPath, Path.Combine(tmpPath, BACKUPNM));
                        Directory.Delete(backupRootPath, true);
                        wasBackup = true;
                    }

                    if (Directory.Exists(downloadRootPath))
                    {
                        this.CopyFolder(downloadRootPath, Path.Combine(tmpPath, DOWNLOADNM));
                        Directory.Delete(downloadRootPath, true);
                        wasDownload = true;
                    }
                }

                try
                {

                    string backupPath = Path.Combine(backupRootPath, Path.GetFileNameWithoutExtension(textBox2.Text));
                    if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

                    string backupPathVer = Path.Combine(backupPath, oldVer);
                    if (!Directory.Exists(backupPathVer)) Directory.CreateDirectory(backupPathVer);

                    string zipName = Path.Combine(backupPathVer, oldVer + ZIPEXT);
                    res = zipName;
                    if (File.Exists(zipName)) File.Move(zipName, Path.Combine(backupPathVer, oldVer + "-" + Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ZIPEXT));

                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipName))
                    {

                        zip.AddDirectory(appPath);
                        zip.Save();
                    }

                    Flokal.Protocol.Logging.Info("Updater: backup ver " + oldVer + " completed.");

                }
                catch (Exception ex)
                {
                    Flokal.Protocol.Logging.Error("Updater.Backup(): " + ex.Message);
                }
            }
            catch (Exception ex)

            {
                Flokal.Protocol.Logging.WriteShowError(ex.Message);
            }
            finally
            {
                if (wasBackup)
                {
                    this.CopyFolder(Path.Combine(tmpPath, BACKUPNM), backupRootPath);
                }

                if (wasDownload)
                {
                    this.CopyFolder(Path.Combine(tmpPath, DOWNLOADNM), downloadRootPath);
                }

                if (wasBackup || wasDownload)
                {
                    Directory.Delete(tmpPath, true);
                }





            }

            return res;

        }



        void CopyFolder(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);

            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyFolder(directory, Path.Combine(targetDir, Path.GetFileName(directory)));

        }


        public static string CreateLocalVersionFile(string folderPath, string fileName, string version)
        {
            if (!new System.IO.DirectoryInfo(folderPath).Exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string path = folderPath + "\\" + fileName;

            if (new System.IO.FileInfo(path).Exists)
            {
                new System.IO.FileInfo(path).Delete();
            }

            if (!new System.IO.FileInfo(path).Exists)
            {
                System.IO.File.WriteAllText(path, version);
            }
            return path;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "";
            timer1.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = openFileDialog1.FileName;
        }
    }

}
