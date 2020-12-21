using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace beans
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
            label2.Text = cfg.ProgramName + " " + Application.ProductVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            using (FormShowTxt frm = new FormShowTxt(System.IO.Path.Combine(Application.StartupPath, "history.txt")))
            {
                frm.Show_();
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            using (FormShowTxt frm = new FormShowTxt(System.IO.Path.Combine(Application.StartupPath, "nlog.txt")))
            {
                frm.Show_();
            }
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {

        }
    }
}