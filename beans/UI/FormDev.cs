using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace beans.UI
{
    public partial class FormDev : Form
    {
        public ucDeviceSetting devSet;
        private ConfigSetting cfg;

        public FormDev()
        {
            InitializeComponent();

            cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);


            devSet = new ucDeviceSetting {Dock = DockStyle.Top};
                    Controls.Add(devSet);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            HidController.Controller.Start(cfg.FrontLight, (int)cfg.BackLigth,
                cfg.VibroVal,cfg.VibroVal2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HidController.Controller.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
            devSet.ParentClosing(null,null);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            HidController.Controller.Start(cfg.FrontLight, (int)cfg.BackLigth,
                0,0);

        }

        private void FormDev_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Screen.AllScreens[0].Bounds.Width-2, this.Height);
            this.Location=new Point(0, Screen.AllScreens[0].Bounds.Height - this.Height-40);
        }
    }
}
