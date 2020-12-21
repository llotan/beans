using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hid
{
    public static class HID_Device_Instance
    {
        private static HID_Device_PC _hid_Device_PC = null;
        private static HID_Device_PC _HID_Device_PC
        {
            get
            {
                if (null == _hid_Device_PC)
                {
                    _hid_Device_PC = new HID_Device_PC();
                    _hid_Device_PC.HID_Inst(HID_Device_Const.DEVICE_VID, HID_Device_Const.DEVICE_PID, HID_Device_Const.REPORT_ID_WRITE, HID_Device_Const.REPORT_ID_READ);
                }
                return _hid_Device_PC;
            }
        }

        private static byte[] Buffer_USB_RX = new byte[HID_Device_Const.PAGE];

        public static bool isConnected
        {
            get
            {
                return _HID_Device_PC.isConnected;
            }
        }

//        public static void HID_Inst_Comand(ushort VID, ushort PID, byte IDw, byte IDr)
//        {
//            _HID_Device_PC.HID_Inst(VID, PID, IDw, IDr);
//        }

        public static void HID_Send_Comand(Int16 Comand, UInt16 Data)
        {
            byte[] ConvArray = BitConverter.GetBytes(Data);
            Buffer_USB_RX[HID_Device_Const.REG_40] = 0;
            switch (Comand)
            {

                case HID_Device_Const.REG_1: Buffer_USB_RX[1] = ConvArray[0]; Buffer_USB_RX[2] = ConvArray[1]; break;
                case HID_Device_Const.REG_2: Buffer_USB_RX[3] = ConvArray[0]; Buffer_USB_RX[4] = ConvArray[1]; break;
                case HID_Device_Const.REG_3: Buffer_USB_RX[5] = ConvArray[0]; Buffer_USB_RX[6] = ConvArray[1]; break;
                case HID_Device_Const.REG_4: Buffer_USB_RX[7] = ConvArray[0]; Buffer_USB_RX[8] = ConvArray[1]; break;
                case HID_Device_Const.REG_5: Buffer_USB_RX[9] = ConvArray[0]; Buffer_USB_RX[10] = ConvArray[1]; break;
                case HID_Device_Const.REG_6: Buffer_USB_RX[11] = ConvArray[0]; Buffer_USB_RX[12] = ConvArray[1]; break;
                case HID_Device_Const.REG_7: Buffer_USB_RX[13] = ConvArray[0]; Buffer_USB_RX[14] = ConvArray[1]; break;
                case HID_Device_Const.REG_8: Buffer_USB_RX[15] = ConvArray[0]; Buffer_USB_RX[16] = ConvArray[1]; break;
                case HID_Device_Const.REG_9: Buffer_USB_RX[17] = ConvArray[0]; Buffer_USB_RX[18] = ConvArray[1]; break;
                case HID_Device_Const.REG_10: Buffer_USB_RX[19] = ConvArray[0]; Buffer_USB_RX[20] = ConvArray[1]; break;
                case HID_Device_Const.REG_11: Buffer_USB_RX[21] = ConvArray[0]; Buffer_USB_RX[22] = ConvArray[1]; break;
                case HID_Device_Const.REG_12: Buffer_USB_RX[23] = ConvArray[0]; Buffer_USB_RX[24] = ConvArray[1]; break;
                case HID_Device_Const.REG_13: Buffer_USB_RX[25] = ConvArray[0]; Buffer_USB_RX[26] = ConvArray[1]; break;
                case HID_Device_Const.REG_14: Buffer_USB_RX[27] = ConvArray[0]; Buffer_USB_RX[28] = ConvArray[1]; break;
                case HID_Device_Const.REG_15: Buffer_USB_RX[29] = ConvArray[0]; Buffer_USB_RX[30] = ConvArray[1]; break;

                //case HID_Device_Const.REG_31: Buffer_USB_RX[HID_Device_Const.REG_31] = ConvArray[0]; break;
                //case HID_Device_Const.REG_32: Buffer_USB_RX[HID_Device_Const.REG_32] = ConvArray[0]; break;
                // OUTPUT ON-OFF
                case HID_Device_Const.REG_30: Buffer_USB_RX[HID_Device_Const.REG_30] = Convert.ToByte(HID_Device_Const.OUTPUT0_BIT); break;// 1. 2. 3. 4. 5. 6. 7. 8
                case HID_Device_Const.REG_31: Buffer_USB_RX[HID_Device_Const.REG_31] = Convert.ToByte(HID_Device_Const.OUTPUT1_BIT); break;// 9.10.11.12.13.14.15.16
                case HID_Device_Const.REG_32: Buffer_USB_RX[HID_Device_Const.REG_32] = Convert.ToByte(HID_Device_Const.OUTPUT2_BIT); break;//17.18.19.20.21.22.23.24


                case HID_Device_Const.REG_33: Buffer_USB_RX[HID_Device_Const.REG_33] = ConvArray[0]; break;
                case HID_Device_Const.REG_34: Buffer_USB_RX[HID_Device_Const.REG_34] = ConvArray[0]; break;
                case HID_Device_Const.REG_35: Buffer_USB_RX[HID_Device_Const.REG_35] = ConvArray[0]; break;
                case HID_Device_Const.REG_36: Buffer_USB_RX[HID_Device_Const.REG_36] = ConvArray[0]; break;
                case HID_Device_Const.REG_37: Buffer_USB_RX[HID_Device_Const.REG_37] = ConvArray[0]; break;
                case HID_Device_Const.REG_38: Buffer_USB_RX[HID_Device_Const.REG_38] = ConvArray[0]; break;
                case HID_Device_Const.REG_39: Buffer_USB_RX[HID_Device_Const.REG_39] = ConvArray[0]; break;
                case HID_Device_Const.REG_40: Buffer_USB_RX[HID_Device_Const.REG_40] = ConvArray[0]; break;
                case HID_Device_Const.REG_41: Buffer_USB_RX[HID_Device_Const.REG_41] = ConvArray[0]; break;
                case HID_Device_Const.REG_42: Buffer_USB_RX[HID_Device_Const.REG_42] = ConvArray[0]; break;
                case HID_Device_Const.REG_43: Buffer_USB_RX[HID_Device_Const.REG_43] = ConvArray[0]; break;
                case HID_Device_Const.REG_44: Buffer_USB_RX[HID_Device_Const.REG_44] = ConvArray[0]; break;

                case HID_Device_Const.REG_16: Buffer_USB_RX[HID_Device_Const.REG_45] = ConvArray[0]; Buffer_USB_RX[HID_Device_Const.REG_46] = ConvArray[1]; break;
                case HID_Device_Const.REG_17: Buffer_USB_RX[HID_Device_Const.REG_47] = ConvArray[0]; Buffer_USB_RX[HID_Device_Const.REG_48] = ConvArray[1]; break;

                case HID_Device_Const.REG_47: Buffer_USB_RX[HID_Device_Const.REG_47] = ConvArray[0]; break;
                case HID_Device_Const.REG_48: Buffer_USB_RX[HID_Device_Const.REG_48] = ConvArray[0]; break;
                case HID_Device_Const.REG_49: Buffer_USB_RX[HID_Device_Const.REG_49] = ConvArray[0]; break;
                case HID_Device_Const.REG_50: Buffer_USB_RX[HID_Device_Const.REG_50] = ConvArray[0]; break;
                case HID_Device_Const.REG_51: Buffer_USB_RX[HID_Device_Const.REG_51] = ConvArray[0]; break;
                case HID_Device_Const.REG_52: Buffer_USB_RX[HID_Device_Const.REG_52] = ConvArray[0]; break;
                case HID_Device_Const.REG_53: Buffer_USB_RX[HID_Device_Const.REG_53] = ConvArray[0]; break;
                case HID_Device_Const.REG_54: Buffer_USB_RX[HID_Device_Const.REG_54] = ConvArray[0]; break;
                case HID_Device_Const.REG_55: Buffer_USB_RX[HID_Device_Const.REG_55] = ConvArray[0]; break;
                case HID_Device_Const.REG_56: Buffer_USB_RX[HID_Device_Const.REG_56] = ConvArray[0]; break;
                case HID_Device_Const.REG_57: Buffer_USB_RX[HID_Device_Const.REG_57] = ConvArray[0]; break;
                case HID_Device_Const.REG_58: Buffer_USB_RX[HID_Device_Const.REG_58] = ConvArray[0]; break;
                case HID_Device_Const.REG_59: Buffer_USB_RX[HID_Device_Const.REG_59] = ConvArray[0]; break;
                case HID_Device_Const.REG_60: Buffer_USB_RX[HID_Device_Const.REG_60] = ConvArray[0]; break;

            }

            _HID_Device_PC.HID_Write(Buffer_USB_RX);
        }

        public static byte[] HID_Read_Comand()
        {
            return _HID_Device_PC.HID_Read();
        }

    }

}
