using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
{
    public partial class ucDeviceSetting : UserControl
    {
        private ConfigSetting cfg;


        public delegate void DataChangedHandle(ConfigSetting cfg);

        public event DataChangedHandle DataChanged;


        public ucDeviceSetting()
        {
            InitializeComponent();
        }


        private void ucDeviceSetting_Resize(object sender, EventArgs e)
        {
            Control uc = (Control) sender;
            grBackLight.Width = grBlow.Width = groupBox10.Width = groupBox9.Width = uc.Width / 4 - 4;
        }


        private void ucDeviceSetting_Load(object sender, EventArgs e)
        {
            cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);

            trVibro.Value = cfg.VibroVal;
            trBackLight.Value = cfg.BackLigth;
            trFrontLight.Value = cfg.FrontLight;
            trBlow.Value = cfg.Blow;
            trackBar1.Value = cfg.VibroVal2;
            trBlowTime.Value = cfg.BlowInterval;
            trSensor.Value= cfg.SensorTime;


            var parent = this.Parent;
            while (parent != null)
            {
                if (parent is Form)
                {
                    ((Form) parent).Closing += new CancelEventHandler(ParentClosing);
                    break;
                }
                else
                {
                    parent = parent.Parent;
                }
            }
        }

        public void ParentClosing(object sender, CancelEventArgs e)
        {
            SettingsManager.Instance.SaveCurrent(cfg, System.Windows.Forms.Application.StartupPath);
        }


        private void TrVibro_ValueChanged(object sender, EventArgs e)
        {
            lbVibro.Text = trVibro.Value.ToString();
             cfg.VibroVal = trVibro.Value;
            DataChanged?.Invoke(cfg);

#if (!TEST)
            Utilities.HidController.Controller.Start(trFrontLight.Value, (int)trBackLight.Value, trVibro.Value,
                trackBar1.Value);
#endif

        }


        private void TrBlow_ValueChanged(object sender, EventArgs e)
        {
            cfg.Blow = trBlow.Value;
            lbBlow.Text = trBlow.Value.ToString();
            DataChanged?.Invoke(cfg);


        }

        private void trBlowTime_ValueChanged(object sender, EventArgs e)
        {
            cfg.BlowInterval = trBlowTime.Value;
            lbBlowTime.Text = cfg.BlowInterval.ToString();
            DataChanged?.Invoke(cfg);
            Utilities.HidController.Controller.BlowSetTime(cfg.BlowInterval);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            labelVibroSpeed.Text = trackBar1.Value.ToString();
            cfg.VibroVal2 = trackBar1.Value;
            DataChanged?.Invoke(cfg);
#if (!TEST)
            Utilities.HidController.Controller.Start(trFrontLight.Value, (int)trBackLight.Value, trVibro.Value,
                trackBar1.Value);
#endif
        }

        private void trBackLight_ValueChanged(object sender, EventArgs e)
        {
            lbBackLight.Text = trBackLight.Value.ToString();
            cfg.BackLigth = trBackLight.Value;
            DataChanged?.Invoke(cfg);
#if (!TEST)
            Utilities.HidController.Controller.Start(trFrontLight.Value, (int)trBackLight.Value, trVibro.Value,
                trackBar1.Value);
#endif
        }

        private void trFrontLight_ValueChanged(object sender, EventArgs e)
        {
            lbFrontLight.Text = trFrontLight.Value.ToString();
            cfg.FrontLight = trFrontLight.Value;
            DataChanged?.Invoke(cfg);

        }

        private void trSensor_Scroll(object sender, EventArgs e)
        {
            lbSensor.Text = trSensor.Value.ToString();
            cfg.SensorTime = trSensor.Value;
            DataChanged?.Invoke(cfg);
            Utilities.HidController.Controller.SensorSetTime(cfg.SensorTime );
        }
    }
}