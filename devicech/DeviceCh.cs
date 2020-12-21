using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hid
{
    public class DeviceCh : IDeviceC1
    {
        const short TOP_LIGHT = HID_Device_Const.REG_3;
        const short BACK_LIGHT = HID_Device_Const.REG_2;//4;
        const short VIBRATION_TABLE = HID_Device_Const.REG_5;
        const UInt16 VIBRATION_TABLE_FREQUENCY = 250;
        const short FREQUENCY = HID_Device_Const.REG_17;
        const short FRONT_BLOW = HID_Device_Const.REG_6;
        const short BACK_BLOW = HID_Device_Const.REG_7;
        const UInt16 MAX_VALUE = 500;
        const UInt16 BLOW_VALUE_MAX = MAX_VALUE;
        const UInt16 MAX_FREQUENCY = 6000;

        private int _top_light_level = 0, _back_light_level = 0, _frequency = 0, _front_blow_level = 0, _back_blow_level = 0, _vibro_table=0;

        private UInt16 ByteToUInt16(int value)
        {
            return Convert.ToUInt16(value * MAX_VALUE / Byte.MaxValue);
        }

        private UInt16 WordToUInt16(int value)
        {
            return Convert.ToUInt16(value * MAX_FREQUENCY / UInt16.MaxValue);
        }

        private UInt16 BoolToUInt16(int value)
        {
            return value > 0 ? BLOW_VALUE_MAX : (UInt16)0;
        }

        private bool ReadAndWriteToDevice(UInt16 top_light_level, UInt16 back_light_level, UInt16 frequency, UInt16 VIBRATION_TABLE_FREQUENCY, UInt16 front_blow_level, UInt16 back_blow_level)
        {
            bool connected = HID_Device_Instance.isConnected;
            if (connected)
            {
                try
                {
                    // Comparing new and current values to avoid too frequent appeal to the device driver.
                    if (_top_light_level != top_light_level)
                    {
                        HID_Device_Instance.HID_Send_Comand(TOP_LIGHT, top_light_level);
                        _top_light_level = top_light_level;
                    }
                    if (_back_light_level != back_light_level)
                    {
                        HID_Device_Instance.HID_Send_Comand(BACK_LIGHT, back_light_level);
                        _back_light_level = back_light_level;
                    }

                    if (_vibro_table != VIBRATION_TABLE_FREQUENCY) 
                    {
                        HID_Device_Instance.HID_Send_Comand(VIBRATION_TABLE, frequency > 0 ? VIBRATION_TABLE_FREQUENCY : (UInt16)0);
                        _vibro_table = VIBRATION_TABLE_FREQUENCY;
                    }
                    if (_frequency != frequency )
                    {
                        HID_Device_Instance.HID_Send_Comand(FREQUENCY, frequency);
                        _frequency = frequency;
                    }
                    if (_front_blow_level != front_blow_level)
                    {
                        HID_Device_Instance.HID_Send_Comand(FRONT_BLOW, front_blow_level);
                        _front_blow_level = front_blow_level;
                    }
                    if (_back_blow_level != back_blow_level)
                    {
                        HID_Device_Instance.HID_Send_Comand(BACK_BLOW, back_blow_level);
                        _back_blow_level = back_blow_level;
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error during access to device";
#if DEBUG
                    using (ThreadExceptionDialog exceptionDlg = new ThreadExceptionDialog(ex)) exceptionDlg.ShowDialog();
#else
                    MessageBox.Show(ex.Message, "Warning");
#endif
                    connected = false;
                }
            }
            return connected;
        }

        public void WriteToDeviceReg40()
        {
            HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_40, 1/*0x01*/);
        }

        public bool WriteToDeviceReg3X(int val, bool is_set = true )
        {
            bool connected = HID_Device_Instance.isConnected;
            if (connected)
            {
                try
                {
                    switch (val)
                    {
                        case 0:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    (HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT0_SET));
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT0_RES);

                            break;

                        case 1:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT1_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT1_RES);

                            break;

                        case 2:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT2_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT2_RES);

                            break;

                        case 3:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT3_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT3_RES);

                            break;

                        case 4:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT4_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT4_RES);

                            break;

                        case 5:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT5_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT5_RES);

                            break;

                        case 6:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT6_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT6_RES);

                            break;

                        case 7:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT0_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT |= HID_Device_Const.BIT7_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_30,
                                    HID_Device_Const.OUTPUT0_BIT &= HID_Device_Const.BIT7_RES);

                            break;


                        case 8:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT0_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT0_RES);

                            break;

                        case 9:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT1_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT1_RES);

                            break;

                        case 10:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT2_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT2_RES);

                            break;

                        case 11:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT3_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT3_RES);

                            break;

                        case 12:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT4_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT4_RES);

                            break;

                        case 13:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT5_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT5_RES);

                            break;

                        case 14:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT6_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT6_RES);

                            break;

                        case 15:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT1_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT |= HID_Device_Const.BIT7_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_31,
                                    HID_Device_Const.OUTPUT1_BIT &= HID_Device_Const.BIT7_RES);

                            break;


                        case 16:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT0_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT0_RES);

                            break;

                        case 17:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT1_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT1_RES);

                            break;

                        case 18:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT2_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT2_RES);

                            break;

                        case 19:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT3_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT3_RES);

                            break;

                        case 20:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT4_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT4_RES);

                            break;

                        case 21:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT5_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT5_RES);

                            break;

                        case 22:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT6_SET);
                            }
                            else
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT6_RES);

                            break;

                        case 23:
                            if (is_set)
                            {
                                HID_Device_Const.OUTPUT2_BIT = 0;
                                HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32,
                                    HID_Device_Const.OUTPUT2_BIT |= HID_Device_Const.BIT7_SET);
                            }
                           else HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_32, HID_Device_Const.OUTPUT2_BIT &= HID_Device_Const.BIT7_RES);

                            break;
                    }
                }

                catch (Exception ex)
                {
//                    string msg = "Error during access to device";
//#if DEBUG
//                    using (ThreadExceptionDialog exceptionDlg = new ThreadExceptionDialog(ex)) exceptionDlg.ShowDialog();
//#else
//                    MessageBox.Show(ex.Message, "Warning");
//#endif
                    connected = false;
                }
            }

            return connected;
        }

        public void WriteToDeviceReg14(int msec)
        {
            HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_14, (UInt16)msec);
        }

        public void WriteToDeviceReg12(int msec)
        {
            HID_Device_Instance.HID_Send_Comand(HID_Device_Const.REG_12, (UInt16)msec);
        }

        public bool ReadFromDevice30(int valve, out int b, out int s)
        {

            bool[] res=new bool[9] {false,false,false,false,false,false,false,false,false};
            b = s = 0;

            if (valve < 9)
            {

                byte[] Buffer_USB_TX = HID_Device_Instance.HID_Read_Comand();
                ///**** INPUT *****////
                if (Buffer_USB_TX.Length > 60)
                {
                    res[0] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT0_SET)) == 0) ? true : false;
                    res[1] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT1_SET)) == 0) ? true : false;
                    res[2] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT2_SET)) == 0) ? true : false;
                    res[3] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT3_SET)) == 0) ? true : false;
                    res[4] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT4_SET)) == 0) ? true : false;
                    res[5] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT5_SET)) == 0) ? true : false;
                    res[6] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT6_SET)) == 0) ? true : false;
                    res[7] = (((byte) (Buffer_USB_TX[30] & HID_Device_Const.BIT7_SET)) == 0) ? true : false;
                    res[8] = (((byte) (Buffer_USB_TX[31] & HID_Device_Const.BIT0_SET)) == 0) ? true : false;
//                res[9] = (((byte)(Buffer_USB_TX[31] & HID_Device_Const.BIT1_SET)) == 0) ? true : false;
                }

                b = Convert.ToInt16(Buffer_USB_TX[33]);
                s = Convert.ToInt16(Buffer_USB_TX[34]);
            }

            return res[valve];
        }

        #region IDeviceC1

        public void OnLoad(System.Object eventSender, System.EventArgs eventArgs)
        {
        }

        public void OnClosed(System.Object eventSender, System.EventArgs eventArgs)
        {
            ReadAndWriteToDevice(0, 0, 0, 0, 0,0);
        }

        public bool ReadAndWriteToDevice(int top_light_level, int back_light_level, int frequency,int VIBRATION_TABLE_FREQUENCY, int front_blow_level, int back_blow_level)
        {
            return ReadAndWriteToDevice(ByteToUInt16(top_light_level), ByteToUInt16(back_light_level), Convert.ToUInt16(frequency),
                Convert.ToUInt16(VIBRATION_TABLE_FREQUENCY),BoolToUInt16(front_blow_level), BoolToUInt16(back_blow_level));
        }

        public void OnDeviceChange(Message m)
        {
        }

        #endregion

    }

}
