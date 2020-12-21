using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hid
{
    public interface IDeviceC1
    {
        void OnLoad(System.Object eventSender, System.EventArgs eventArgs);
        void OnClosed(System.Object eventSender, System.EventArgs eventArgs);
        bool ReadAndWriteToDevice(int top_light_level, int back_light_level, int frequency, int VIBRATION_TABLE_FREQUENCY, int front_blow_level, int back_blow_level);
        bool WriteToDeviceReg3X(int valve, bool is_set);
        void WriteToDeviceReg14(int msec);
        void OnDeviceChange(Message m);
        void WriteToDeviceReg12(int msec);
        void WriteToDeviceReg40();
        bool ReadFromDevice30(int valve, out int b, out int s);
    }

}
