using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
{
    public partial class FormSetBlow : Form
    {
        private ConfigSetting cfg;

        public FormSetBlow(ConfigSetting config)
        {
            InitializeComponent();
            cfg = config;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int channel = Convert.ToInt32(btn.Text.Replace("OUT", "")) - 1;
            if (btn.BackColor == Color.Khaki)
            {
                Utilities.HidController.Controller.Blow(channel);
                btn.BackColor = Color.PaleGreen;
            }
            else
            {
                Utilities.HidController.Controller.Blow(channel,false);
                btn.BackColor = Color.Khaki;
            }
        }

        private void FormSetBlow_Load(object sender, EventArgs e)
        {
            Utilities.HidController.Controller.Start(0, 0, 0,0);
            cfg = (cfg != null)?cfg:SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);

            numericUpDown10.Value= trackBar10.Value = cfg.Blow;
        }

        private void FormSetBlow_FormClosing(object sender, FormClosingEventArgs e)
        {
            cfg.Blow = trackBar10.Value ;
            SettingsManager.Instance.SaveCurrent(cfg, System.Windows.Forms.Application.StartupPath);
            Utilities.HidController.Controller.Stop();

        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            Utilities.HidController.Controller.BlowSetTime((int)(numericUpDown10.Value = Convert.ToDecimal(trackBar10.Value)));
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            Utilities.HidController.Controller.BlowSetTime((int)(trackBar10.Value= Convert.ToInt32(numericUpDown10.Value)));
        }
    }
}
