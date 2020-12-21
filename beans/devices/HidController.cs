using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using Hid;
using Flokal.Protocol;

namespace Utilities
{
    public class HidController
    {
        private static IDeviceC1 _device = null; 
        private static HidController _controller = null;
        public static HidController Controller
        {
            get
            {
                if (null == _controller)
                    _controller = new HidController();
                return _controller;
            }
        }

        private HidController()
        {
            _device = new DeviceCh();
        }

        public void OnClosed(System.Object eventSender, System.EventArgs eventArgs)
        {
            try
            {
                Stop();
                _device.OnClosed(eventSender, eventArgs);
            }
            catch (Exception ex)
            {
                Logging.Error("Device error. " + ex.Message);
                throw ex;
            }
        }

        public void OnLoad(System.Object eventSender, System.EventArgs eventArgs)
        {
            try
            {
                _device.OnLoad(eventSender, eventArgs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Device connect error.");
                Logging.Error("Device connect error. " + ex.Message);
                throw ex;
            }

        }

        public void OnDeviceChange(Message m)
        {
            try
            {
                _device.OnDeviceChange(m);
            }
            catch (Exception ex)
            {
                Logging.Error("Device error. " + ex.Message);
                throw ex;
            }
        }

        private int _top_light_level = 0, _back_light_level = 0, _frequency = 0, _front_blow_level = 0, _back_blow_level = 0; 

        private bool Set(int top_light_level, int back_light_level, int frequency,  int vibro_table, int front_blow_level = 0, int back_blow_level = 0)
        {
            bool result = true;
            try
            {
                // Comparing new and current values to avoid too frequent appeal to the device driver.
                if ( _top_light_level != top_light_level ||
                     _back_light_level != back_light_level ||
                     _frequency != frequency ||
                     _front_blow_level != front_blow_level ||
                     _back_blow_level != back_blow_level )
                {
#if !NO_DEVICE
                    result = _device.ReadAndWriteToDevice(top_light_level, back_light_level, frequency,  vibro_table,front_blow_level, back_blow_level);
#endif
                }
                if ( result )
                {
            _top_light_level = top_light_level;
            _back_light_level = back_light_level;
            _frequency = frequency;
                    _front_blow_level = front_blow_level;
                    _back_blow_level = back_blow_level;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error during access to device";
                Logging.Error(msg, ex);
                Trace.WriteLine(msg + " : " + ex.Message);
#if DEBUG
                using (ThreadExceptionDialog exceptionDlg = new ThreadExceptionDialog(ex)) exceptionDlg.ShowDialog();
#else
                    MessageBox.Show(ex.Message, "Warning");
#endif
                result = false;
            }
            return result;
        }

        public bool Start(int top_light_level, int back_light_level, int frequency,int vibro_table)
        {
            bool status = Set(top_light_level, back_light_level, frequency, vibro_table);
            if (false == status)
            {
                string message = "The device is not start";
                Logging.Error(message);
                Trace.WriteLine(message);
            }
            return status;
        }


        public bool Stop()
        {
            bool status = Set(0, 0, 0, 0);
            if (false == status)
            {
                string message = "The device is not stop";
                Logging.Error(message);
                Trace.WriteLine(message);
            }
            return status;
        }

        // old
        public bool Blow(int front_blow_level, int back_blow_level)
        {
            return Set(_top_light_level, _back_light_level, _frequency, front_blow_level, back_blow_level);
        }

        // actual
        public bool Blow(int chanel, bool is_set=true)
        {
            return _device.WriteToDeviceReg3X(chanel, is_set);
        }

        public void BlowSetTime(int msec)
        {
            _device.WriteToDeviceReg14(msec   );
        }

        public void SensorSetTime(int msec)
        {
            _device.WriteToDeviceReg12(msec   );

        }

        public void SensorsClear()
        {
            //TO-DO: SensorsClear() NEED REALIZ !!!
            _device.WriteToDeviceReg40( );

        }

        public bool SensorStatusRead(int valve, out int b,out int s)
        {
            return _device.ReadFromDevice30(valve,out b,out s);
        }
    }

}
