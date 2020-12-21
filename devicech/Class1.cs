using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
//using System.Drawing;
///using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Hid
{
    public static class HID_Device_Const
    {
        public const ushort DEVICE_VID = 1155;
        public const ushort DEVICE_PID = 22352;

        public const byte PAGE = 66;
        public const byte REPORT_ID_READ = 1;
        public const byte REPORT_ID_WRITE = 2;

        public const Int16 REG_1 = 1;
        public const Int16 REG_2 = 2;
        public const Int16 REG_3 = 3;   // PWM Channels-1
        public const Int16 REG_4 = 4;   // PWM Channels-1
        public const Int16 REG_5 = 5;   // PWM Channels-2
        public const Int16 REG_6 = 6;   // PWM Channels-2
        public const Int16 REG_7 = 7;   // PWM Channels-3
        public const Int16 REG_8 = 8;   // PWM Channels-3
        public const Int16 REG_9 = 9;   // PWM Channels-4
        public const Int16 REG_10 = 10; // PWM Channels-4
        public const Int16 REG_11 = 11; // PWM Channels-5
        public const Int16 REG_12 = 12; // PWM Channels-5
        public const Int16 REG_13 = 13; // PWM Channels-6
        public const Int16 REG_14 = 14; // PWM Channels-6
        public const Int16 REG_15 = 15; // PWM Channels-7
        public const Int16 REG_16 = 16; 
        public const Int16 REG_17 = 17; 

        public const Int16 REG_30 = 30;

        public const Int16 REG_31 = 31;
        public const Int16 REG_32 = 32; // LED6
        public const Int16 REG_33 = 33; // LED5
        public const Int16 REG_34 = 34; // LED4
        public const Int16 REG_35 = 35; // LED1
        public const Int16 REG_36 = 36; // LED2
        public const Int16 REG_37 = 37; // LED3
        public const Int16 REG_38 = 38;
        public const Int16 REG_39 = 39; // IMPUT1
        public const Int16 REG_40 = 40; // IMPUT2
        public const Int16 REG_41 = 41; // IMPUT3
        public const Int16 REG_42 = 42; // IMPUT4
        public const Int16 REG_43 = 43; // IMPUT5
        public const Int16 REG_44 = 44; // IMPUT6
        public const Int16 REG_45 = 45; // Frequency PWM Channels-1
        public const Int16 REG_46 = 46;    
        public const Int16 REG_47 = 47; // Frequency PWM Channels-4
        public const Int16 REG_48 = 48;
        public const Int16 REG_49 = 49;
        public const Int16 REG_50 = 50; 
        public const Int16 REG_51 = 51;
        public const Int16 REG_52 = 52;
        public const Int16 REG_53 = 53;
        public const Int16 REG_54 = 54;
        public const Int16 REG_55 = 55;
        public const Int16 REG_56 = 56;
        public const Int16 REG_57 = 57;
        public const Int16 REG_58 = 58;
        public const Int16 REG_59 = 59;
        public const Int16 REG_60 = 60;

        public const UInt16 ON = 0xFFFF;
        public const UInt16 OFF = 0;

        public static Byte OUTPUT0_BIT = 0;
        public static Byte OUTPUT1_BIT = 0;
        public static Byte OUTPUT2_BIT = 0;

        public const byte BIT0_RES = 0xFE;
        public const byte BIT1_RES = 0xFD;
        public const byte BIT2_RES = 0xFB;
        public const byte BIT3_RES = 0xF7;
        public const byte BIT4_RES = 0xEF;
        public const byte BIT5_RES = 0xDF;
        public const byte BIT6_RES = 0xBF;
        public const byte BIT7_RES = 0x7F;

        public const byte BIT0_SET = 0x01;
        public const byte BIT1_SET = 0x02;
        public const byte BIT2_SET = 0x04;
        public const byte BIT3_SET = 0x08;
        public const byte BIT4_SET = 0x10;
        public const byte BIT5_SET = 0x20;
        public const byte BIT6_SET = 0x40;
        public const byte BIT7_SET = 0x80;


    }

    internal class HID_Device_PC
    {
        #region WinAPI

        [DllImport("user32.dll")]
        static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        [DllImport(@"setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string fileName, uint fileAccess, uint fileShare, FileMapProtection securityAttributes, uint creationDisposition, uint flags, IntPtr overlapped);

        [DllImport("kernel32.dll")]
        static extern bool WriteFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("hid.dll")]
        static extern void HidD_GetHidGuid(ref Guid Guid);

        [DllImport("hid.dll", SetLastError = true)]
        static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        static extern bool HidD_GetAttributes(IntPtr DeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true)]
        static extern uint HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        [DllImport("hid.dll", SetLastError = true)]
        static extern int HidP_GetButtonCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_BUTTON_CAPS[] ButtonCaps, ref ushort ButtonCapsLength, IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        static extern int HidP_GetValueCaps(HIDP_REPORT_TYPE ReportType, [In, Out] HIDP_VALUE_CAPS[] ValueCaps, ref ushort ValueCapsLength, IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        static extern int HidP_MaxUsageListLength(HIDP_REPORT_TYPE ReportType, ushort UsagePage, IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        static extern int HidP_SetUsages(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection, short Usages, ref int UsageLength, IntPtr PreparsedData, IntPtr Report, int ReportLength);

        [DllImport("hid.dll", SetLastError = true)]
        static extern int HidP_SetUsageValue(HIDP_REPORT_TYPE ReportType, ushort UsagePage, short LinkCollection, ushort Usage, ulong UsageValue, IntPtr PreparsedData, IntPtr Report, int ReportLength);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalFree(object hMem);

        [DllImport("hid.dll", SetLastError = true)]
        static extern bool HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        #endregion

        #region Init Variable

        IntPtr hardwareDeviceInfo;

        private int SW_SHOW = 5;
        private bool cancel = true;
        private bool HID_quit = false;
        private int nbrDevices;
        private int iHIDD;
        public bool isConnected = false;

        private ushort DEVICE_VID;
        private ushort DEVICE_PID;
        private ushort USAGE_PAGE;
        private ushort USAGE;
        private byte REPORT_IDr;
        private byte REPORT_IDw;

        private const int DIGCF_DEFAULT = 0x00000001;
        private const int DIGCF_PRESENT = 0x00000002;
        private const int DIGCF_ALLCLASSES = 0x00000004;
        private const int DIGCF_PROFILE = 0x00000008;
        private const int DIGCF_DEVICEINTERFACE = 0x00000010;
        
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint GENERIC_EXECUTE = 0x20000000;
        private const uint GENERIC_ALL = 0x10000000;
        
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint FILE_SHARE_DELETE = 0x00000004;

        private const uint CREATE_NEW = 1;
        private const uint CREATE_ALWAYS = 2;
        private const uint OPEN_EXISTING = 3;
        private const uint OPEN_ALWAYS = 4;
        private const uint TRUNCATE_EXISTING = 5;
        
        private const int HIDP_STATUS_SUCCESS = 1114112;
        private const int DEVICE_PATH = 260;
        private const int INVALID_HANDLE_VALUE = -1;

        enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        enum HIDP_REPORT_TYPE : ushort
        {
            HidP_Input = 0x00,
            HidP_Output = 0x01,
            HidP_Feature = 0x02,
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LIST_ENTRY
        {
            public IntPtr Flink;
            public IntPtr Blink;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DEVICE_LIST_NODE
        {
            public LIST_ENTRY Hdr;
            public IntPtr NotificationHandle;
            public HID_DEVICE HidDeviceInfo;
            public bool DeviceOpened;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVICE_INTERFACE_DATA
        {
            public Int32 cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private UIntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid classGuid;
            public UInt32 devInst;
            public IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HIDP_CAPS
        {
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 Usage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 UsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 InputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 OutputReportByteLength;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public UInt16[] Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberLinkCollectionNodes;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberInputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberOutputDataIndices;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureButtonCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureValueCaps;
            [MarshalAs(UnmanagedType.U2)]
            public UInt16 NumberFeatureDataIndices;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ButtonData
        {
            public Int32 UsageMin;
            public Int32 UsageMax;
            public Int32 MaxUsageLength;
            public Int16 Usages;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ValueData
        {
            public ushort Usage;
            public ushort Reserved;

            public ulong Value;
            public long ScaledValue;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct HID_DATA
        {
            [FieldOffset(0)]
            public bool IsButtonData;
            [FieldOffset(1)]
            public byte Reserved;
            [FieldOffset(2)]
            public ushort UsagePage;
            [FieldOffset(4)]
            public Int32 Status;
            [FieldOffset(8)]
            public Int32 ReportID;
            [FieldOffset(16)]
            public bool IsDataSet;

            [FieldOffset(17)]
            public ButtonData ButtonData;
            [FieldOffset(17)]
            public ValueData ValueData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_Range
        {
            public ushort UsageMin, UsageMax;
            public ushort StringMin, StringMax;
            public ushort DesignatorMin, DesignatorMax;
            public ushort DataIndexMin, DataIndexMax;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_NotRange
        {
            public ushort Usage, Reserved1;
            public ushort StringIndex, Reserved2;
            public ushort DesignatorIndex, Reserved3;
            public ushort DataIndex, Reserved4;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct HIDP_BUTTON_CAPS
        {
            [FieldOffset(0)]
            public ushort UsagePage;
            [FieldOffset(2)]
            public byte ReportID;
            [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [FieldOffset(4)]
            public short BitField;
            [FieldOffset(6)]
            public short LinkCollection;
            [FieldOffset(8)]
            public short LinkUsage;
            [FieldOffset(10)]
            public short LinkUsagePage;
            [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [FieldOffset(16), MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] Reserved;

            [FieldOffset(56)]
            public HIDP_Range Range;
            [FieldOffset(56)]
            public HIDP_NotRange NotRange;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct HIDP_VALUE_CAPS
        {
            [FieldOffset(0)]
            public ushort UsagePage;
            [FieldOffset(2)]
            public byte ReportID;
            [FieldOffset(3), MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;
            [FieldOffset(4)]
            public ushort BitField;
            [FieldOffset(6)]
            public ushort LinkCollection;
            [FieldOffset(8)]
            public ushort LinkUsage;
            [FieldOffset(10)]
            public ushort LinkUsagePage;
            [FieldOffset(12), MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [FieldOffset(13), MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [FieldOffset(14), MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [FieldOffset(15), MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [FieldOffset(16), MarshalAs(UnmanagedType.U1)]
            public bool HasNull;
            [FieldOffset(17)]
            public byte Reserved;
            [FieldOffset(18)]
            public short BitSize;
            [FieldOffset(20)]
            public short ReportCount;
            [FieldOffset(22)]
            public ushort Reserved2a;
            [FieldOffset(24)]
            public ushort Reserved2b;
            [FieldOffset(26)]
            public ushort Reserved2c;
            [FieldOffset(28)]
            public ushort Reserved2d;
            [FieldOffset(30)]
            public ushort Reserved2e;
            [FieldOffset(32)]
            public int UnitsExp;
            [FieldOffset(36)]
            public int Units;
            [FieldOffset(40)]
            public int LogicalMin;
            [FieldOffset(44)]
            public int LogicalMax;
            [FieldOffset(48)]
            public int PhysicalMin;
            [FieldOffset(52)]
            public int PhysicalMax;

            [FieldOffset(56)]
            public HIDP_Range Range;
            [FieldOffset(56)]
            public HIDP_NotRange NotRange;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct HID_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DEVICE_PATH)]
            public string DevicePath;
            public IntPtr HidDevice;
            public bool OpenedForRead;
            public bool OpenedForWrite;
            public bool OpenedOverlapped;
            public bool OpenedExclusive;

            public IntPtr Ppd;
            public HIDP_CAPS Caps;
            public HIDD_ATTRIBUTES Attributes;

            public IntPtr[] InputReportBuffer;
            public HID_DATA[] InputData;
            public Int32 InputDataLength;
            public HIDP_BUTTON_CAPS[] InputButtonCaps;
            public HIDP_VALUE_CAPS[] InputValueCaps;

            public IntPtr[] OutputReportBuffer;
            public HID_DATA[] OutputData;
            public Int32 OutputDataLength;
            public HIDP_BUTTON_CAPS[] OutputButtonCaps;
            public HIDP_VALUE_CAPS[] OutputValueCaps;

            public IntPtr[] FeatureReportBuffer;
            public HID_DATA[] FeatureData;
            public Int32 FeatureDataLength;
            public HIDP_BUTTON_CAPS[] FeatureButtonCaps;
            public HIDP_VALUE_CAPS[] FeatureValueCaps;
        }

        #endregion

        byte WriteData = 0;
        bool CheckNewDevice = false;

        internal HID_Device_PC()
        {
        }

        void Entry(object sender, EventArgs Data)
        {
            Thread HIDThread;

            HIDThread = new Thread(new ThreadStart(HID));
            HIDThread.IsBackground = true;
            HIDThread.Start();
        }

        void HID()
        {
            HID_DEVICE[] pDevice = new HID_DEVICE[1];

            while (true)
            {
                Thread.Sleep(1);

                if (nbrDevices != FindNumberDevices() || CheckNewDevice)
                {
                    nbrDevices = FindNumberDevices();
                    pDevice = new HID_DEVICE[nbrDevices];
                    FindKnownHidDevices(ref pDevice);

                    var i = 0;
                    while (i < nbrDevices)
                    {
                        var count = 0;

                        if (pDevice[i].Attributes.VendorID == DEVICE_VID && DEVICE_VID != 0)
                            count++;
                        if (pDevice[i].Attributes.ProductID == DEVICE_PID)
                            count++;
                        if (pDevice[i].Caps.UsagePage == USAGE_PAGE)
                            count++;
                        if (pDevice[i].Caps.Usage == USAGE)
                            count++;

                        if (count == 4)
                        {
                            iHIDD = i;
                            isConnected = true;

                            break;
                        }
                        else
                            isConnected = false;

                        i++;
                    }

                    CheckNewDevice = false;
                }

                if (isConnected)
                {
                    // Read(pDevice[iHIDD]);
                }
            }
        }

        void Write(HID_DEVICE HidDevice)
        {
            byte[] Report = new byte[HidDevice.Caps.OutputReportByteLength];
            uint tmp = 0;

            try
            {
                Report[0] = REPORT_IDw;
                Report[1] = WriteData;
            }
            catch
            {

            }

            WriteFile(HidDevice.HidDevice, Report, HidDevice.Caps.OutputReportByteLength, ref tmp, IntPtr.Zero);
        }

        internal void HID_Write(byte[] Data)
        {


            HID_DEVICE[] pDevice = new HID_DEVICE[1];
           nbrDevices = FindNumberDevices();
           pDevice = new HID_DEVICE[nbrDevices];
           FindKnownHidDevices(ref pDevice);

           byte[] test = new byte[5];

            var idx = 0;
            while (idx < nbrDevices)
            {
                var count = 0;

                if (pDevice[idx].Attributes.VendorID == DEVICE_VID && DEVICE_VID != 0)
                    count++;
                if (pDevice[idx].Attributes.ProductID == DEVICE_PID)
                    count++;
                if (count == 2)
                {
                    isConnected = true; break;
                }
                else
                    idx++;
                isConnected = false;
            }


            if (isConnected == true)
            {
                byte[] Report = new byte[pDevice[idx].Caps.InputReportByteLength];

                byte i = 0;
                while (i < pDevice[idx].Caps.InputReportByteLength)
                {

                    try
                    {
                        Report[i] = Data[i];
                        i++;
                    }
                    catch { }
                }
                Report[0] = REPORT_IDw;
                uint tmp = 0;
                bool result = WriteFile(pDevice[idx].HidDevice, Report, pDevice[idx].Caps.OutputReportByteLength, ref tmp, IntPtr.Zero);
            }
        }

        internal byte[] HID_Read()
        {

            HID_DEVICE[] pDevice = new HID_DEVICE[1];
            nbrDevices = FindNumberDevices();
            pDevice = new HID_DEVICE[nbrDevices];
            FindKnownHidDevices(ref pDevice);

            byte[] test = new byte[5];
            var idx = 0;
            while (idx < nbrDevices)
            {
                var count = 0;

                if (pDevice[idx].Attributes.VendorID == DEVICE_VID && DEVICE_VID != 0)
                    count++;
                if (pDevice[idx].Attributes.ProductID == DEVICE_PID)
                    count++;
                if (count == 2)
                {
                    isConnected = true; break;
                }
                else
                    idx++;
                isConnected = false;
            }


            if (isConnected == true)
            {
                string meseg = "";  //TEST meseg
                byte[] Data_USB = new byte[66];

                byte[] Report = new byte[pDevice[idx].Caps.InputReportByteLength];
                uint tmp = 0;

                try
                { Report[0] = REPORT_IDr; }
                catch { }
                ReadFile(pDevice[idx].HidDevice, Report, pDevice[idx].Caps.InputReportByteLength, ref tmp, IntPtr.Zero);
                try
                {  //extBox16.Clear();
                }
                catch { }

                byte i = 0;
                while (i < pDevice[idx].Caps.InputReportByteLength)
                {
                    try
                    {
                        Data_USB[i] = Convert.ToByte(Report[i++].ToString());
                        //  meseg +=  Report[i++].ToString();
                    }
                    catch { }
                }
                return Data_USB;
                //MessageBox.Show(meseg);
            }
            return test;
        }

        internal void HID_Inst(ushort VID, ushort PID, byte IDw, byte IDr)
        {
            HID_DEVICE[] pDevice = new HID_DEVICE[1];
            DEVICE_VID = VID;
            DEVICE_PID = PID;
            REPORT_IDw = IDw;
            REPORT_IDr = IDr;
            nbrDevices = FindNumberDevices();
            pDevice = new HID_DEVICE[nbrDevices];
            FindKnownHidDevices(ref pDevice);

            var i = 0;
            while (i < nbrDevices)
            {
                var count = 0;

                if (pDevice[i].Attributes.VendorID == DEVICE_VID && DEVICE_VID != 0)
                    count++;
                if (pDevice[i].Attributes.ProductID == DEVICE_PID)
                    count++;
                //if (pDevice[i].Caps.UsagePage == USAGE_PAGE)
                //   count++;
                //  if (pDevice[i].Caps.Usage == USAGE)
                //  count++;

                if (count == 2)
                {
                    isConnected = true; break;
                }
                else { i++; isConnected = false; }

            }
        }

        int FindNumberDevices()
        {
            Guid hidGuid = new Guid();
            SP_DEVICE_INTERFACE_DATA deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
            int index = 0;

            HidD_GetHidGuid(ref hidGuid);

            //
            // Open a handle to the plug and play dev node.
            //
            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            hardwareDeviceInfo = SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

            index = 0;
            while (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, IntPtr.Zero, ref hidGuid, index, ref deviceInfoData))
            {
                index++;
            }

            return (index);
        }

        int FindKnownHidDevices(ref HID_DEVICE[] HidDevices)
        {
            int iHIDD;
            int RequiredLength;

            Guid hidGuid = new Guid();
            SP_DEVICE_INTERFACE_DATA deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
            SP_DEVICE_INTERFACE_DETAIL_DATA functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();

            HidD_GetHidGuid(ref hidGuid);

            //
            // Open a handle to the plug and play dev node.
            //
            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            hardwareDeviceInfo = SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

            iHIDD = 0;
            while (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, IntPtr.Zero, ref hidGuid, iHIDD, ref deviceInfoData))
            {
                RequiredLength = 0;

                //
                // allocate a function class device data structure to receive the
                // goods about this particular device.
                //
                SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfo, ref deviceInfoData, IntPtr.Zero, 0, ref RequiredLength, IntPtr.Zero);

                if (IntPtr.Size == 8)
                    functionClassDeviceData.cbSize = 8;
                else if (IntPtr.Size == 4)
                    functionClassDeviceData.cbSize = 5;

                //
                // Retrieve the information from Plug and Play.
                //
                SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfo, ref deviceInfoData, ref functionClassDeviceData, RequiredLength, ref RequiredLength, IntPtr.Zero);

                //
                // Open device with just generic query abilities to begin with
                //
                OpenHidDevice(functionClassDeviceData.DevicePath, ref HidDevices, iHIDD);

                iHIDD++;
            }

            return iHIDD;
        }

        void OpenHidDevice(string DevicePath, ref HID_DEVICE[] HidDevice, int iHIDD)
        {
            try
            {

                /*++
                RoutineDescription:
                Given the HardwareDeviceInfo, representing a handle to the plug and
                play information, and deviceInfoData, representing a specific hid device,
                open that device and fill in all the relivant information in the given
                HID_DEVICE structure.
                --*/

                HidDevice[iHIDD].DevicePath = DevicePath;

                //
                //  The hid.dll api's do not pass the overlapped structure into deviceiocontrol
                //  so to use them we must have a non overlapped device.  If the request is for
                //  an overlapped device we will close the device below and get a handle to an
                //  overlapped device
                //
                CloseHandle(HidDevice[iHIDD].HidDevice);
                HidDevice[iHIDD].HidDevice = CreateFile(HidDevice[iHIDD].DevicePath, GENERIC_READ | GENERIC_WRITE,
                    FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, IntPtr.Zero);
                HidDevice[iHIDD].Caps = new HIDP_CAPS();
                HidDevice[iHIDD].Attributes = new HIDD_ATTRIBUTES();

                //
                // If the device was not opened as overlapped, then fill in the rest of the
                //  HidDevice structure.  However, if opened as overlapped, this handle cannot
                //  be used in the calls to the HidD_ exported functions since each of these
                //  functions does synchronous I/O.
                //
                HidD_FreePreparsedData(ref HidDevice[iHIDD].Ppd);
                HidDevice[iHIDD].Ppd = IntPtr.Zero;
                HidD_GetPreparsedData(HidDevice[iHIDD].HidDevice, ref HidDevice[iHIDD].Ppd);
                HidD_GetAttributes(HidDevice[iHIDD].HidDevice, ref HidDevice[iHIDD].Attributes);
                HidP_GetCaps(HidDevice[iHIDD].Ppd, ref HidDevice[iHIDD].Caps);

                //MessageBox.Show(GetLastError().ToString());

                //
                // At this point the client has a choice.  It may chose to look at the
                // Usage and Page of the top level collection found in the HIDP_CAPS
                // structure.  In this way --------*it could just use the usages it knows about.
                // If either HidP_GetUsages or HidP_GetUsageValue return an error then
                // that particular usage does not exist in the report.
                // This is most likely the preferred method as the application can only
                // use usages of which it already knows.
                // In this case the app need not even call GetButtonCaps or GetValueCaps.
                //
                // In this example, however, we will call FillDeviceInfo to look for all
                //    of the usages in the device.
                //
                //FillDeviceInfo(ref HidDevice);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {DevicePath},{HidDevice},{iHIDD}");


            }

        }

    }

}
