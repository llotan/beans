using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace beans
{
    //*************************************************************************************************************************************************** Collections object for save *******************************
    [XmlRoot("root", Namespace = "")]
    public class ConfigSetting //: ISettingMotors
    {
        public ConfigSetting()
        {
            ProgramName = Application.ProductName;
            isFirstLaunch = true;
            OneCam = true;
            BinMethod = 1;
            BinVal = 50;
            VibroVal = 1;
            VibroVal2 = 250;
            BackLigth = 1;
            FrontLight = 1;
            Blow = 1;
            BlowInterval = 10;
            XShiftMS = YShiftMS = 0;
            XDelta = YDelta = 10;
            msDelta = 1;
            widthOfCameraView = 1000;

            BlobViewItemHeight = 96;
            BlobViewItemWidth = 96;
            isShowFrameRgb = true;
            isShowFrameBin = true;

            Report = new ReportsSetting();
            CameraMaster = "4102882321"; // "4103043267";

            BlowInterval = 200;
            ValvesCount = 9;
            isValvesShow = true;
            ValveLeftShift = ValveRightShift = 0;

            // RED
            filterRED_H1Begin = 0;
            filterRED_S1Begin = 100;
            filterRED_V1Begin = 0;
            filterRED_H1End = 10;
            filterRED_S1End = 255;
            filterRED_V1End = 255;

            filterRED_H2Begin = 170;
            filterRED_S2Begin = 100;
            filterRED_V2Begin = 0;
            filterRED_H2End = 180;
            filterRED_S2End = 255;
            filterRED_V2End = 255;

            filterBAD_Intensivity = 100;

            MeasureFactor = 1;
            MeasureType = 2;
        }

        public override string ToString()
        {
            return Application.ProductName + " Settings";
        }

        [XmlElement]
        public string ProgramName;

        [XmlElement]
        public bool isFirstLaunch;

        [XmlElement]
        [DescriptionAttribute("If using one camera set TRUE, two set FALSE"),
         CategoryAttribute("Hardware")]
        public bool OneCam;

        [XmlElement]
        [DescriptionAttribute("How many pixels is in micrometer"),
       CategoryAttribute("Measurement")]
        public double  MeasureFactor;

        [XmlElement]
        [DescriptionAttribute("enum MeasureType { none, pixel,  micrometer, milimeter, cantimeter }"),
       CategoryAttribute("Measurement")]
        public int  MeasureType;


        [XmlElement]
        [DescriptionAttribute(" Binarization method 0-6"),
         CategoryAttribute("Recognation")]
        public int BinMethod;

        [XmlElement]
        [DescriptionAttribute(" Binarization Value 0-255"),
         CategoryAttribute("Recognation")]
        public int BinVal;

        [XmlElement]
        [DescriptionAttribute(" Use valves in sync process"),
         CategoryAttribute("MasterSlave synchronization")]
        public bool isUseValvesInSync { get; set; }


        [XmlElement]
        [DescriptionAttribute("Speed of vibro table"),
         CategoryAttribute("Hardware")]
        public int VibroVal{get;set;}

        [XmlElement]
        [DescriptionAttribute("Frecensy of vibro table"),
         CategoryAttribute("Hardware")]
        public int VibroVal2 { get; set; }

        [XmlElement]
        [DescriptionAttribute("Illumination first light"),
         CategoryAttribute("Hardware")]
        public int BackLigth{get;set;}

        [XmlElement]
        [DescriptionAttribute("Illumination second light"),
         CategoryAttribute("Hardware")]
        public int FrontLight{get;set;}

        [XmlElement]
        [DescriptionAttribute("Delay for sensor"),
         CategoryAttribute("Hardware")]
        public int SensorTime { get; set; }

        [XmlElement]
        [DescriptionAttribute("How long time valve of blow be open, msec"),
         CategoryAttribute("Hardware")]
        public int Blow{get;set;}

        [XmlElement]
        [DescriptionAttribute("Delay in msec between detect and blow"),
         CategoryAttribute("Hardware")]
        public int BlowInterval {get;set;}

    [XmlElement]
        [DescriptionAttribute(""),
         CategoryAttribute("Valve's lanes")]
        public int ValvesCount { get; set; }

        [XmlElement]
        [DescriptionAttribute(""),
         CategoryAttribute("Valve's lanes")]
        public int ValveLeftShift { get; set; }
        [XmlElement]
        [DescriptionAttribute(""),
         CategoryAttribute("Valve's lanes")]
        public int ValveRightShift { get; set; }
        [XmlElement]
        [DescriptionAttribute(""),
         CategoryAttribute("Interface")]
        public bool isValvesShow { get; set; }


        [XmlElement]
        [DescriptionAttribute("Horz distanse between master and slave image center"),
         CategoryAttribute("MasterSlave synchronization")]
        public int XShiftMS;

        [XmlElement]
        [DescriptionAttribute("Vertical distanse between master and slave image center"),
         CategoryAttribute("MasterSlave synchronization")]
        public int YShiftMS;

        [XmlElement]
        [DescriptionAttribute("Enable error shifting by horizontal"),
         CategoryAttribute("MasterSlave synchronization")]
        public int XDelta { get; set; }

        [XmlElement]
        [DescriptionAttribute("Enable error shifting by vertical"),
         CategoryAttribute("MasterSlave synchronization")]
        public int YDelta { get; set; }

        [XmlElement]
        [DescriptionAttribute("Enable error shifting by millisecond"),
         CategoryAttribute("MasterSlave synchronization")]
        public int msDelta { get; set; }

        [XmlElement]
        [DescriptionAttribute("width of camera view"),
         CategoryAttribute("MasterSlave synchronization")]
        public int widthOfCameraView;

        public int heightOfCameraView=4;


        [XmlElement]
        [DescriptionAttribute("Show RGB frames of detected samples"),
         CategoryAttribute("Interface")]
        public bool isShowFrameRgb { get; set; }

        [XmlElement]
        [DescriptionAttribute("Show BIN frames of detected samples"),
         CategoryAttribute("Interface")]
        public bool isShowFrameBin { get; set; }

        [XmlElement]
        public bool isSetProcess { get; set; }

        [XmlElement]
        [DescriptionAttribute("H begin, range 1"),
         CategoryAttribute("Recognation RED color")]
        public int filterRED_H1Begin{get;set;}

        [XmlElement]
        [DescriptionAttribute("S begin, range 1"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_S1Begin{get;set;}

        [XmlElement]
        [DescriptionAttribute("V begin, range 1"),
         CategoryAttribute("Recognation RED color")]
        public byte  filterRED_V1Begin{get;set;}

        [XmlElement]
        [DescriptionAttribute("H end, range 1  (0...360)"),
         CategoryAttribute("Recognation RED color")]
        public int filterRED_H1End{get;set;}

        [XmlElement]
        [DescriptionAttribute("S end, range 1"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_S1End{get;set;}

        [XmlElement]
        [DescriptionAttribute("V end, range 1"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_V1End{get;set;}

        [XmlElement]
        [DescriptionAttribute("H begin, range 2  (0...360)"),
         CategoryAttribute("Recognation RED color")]
        public int filterRED_H2Begin { get; set; }

        [XmlElement]
        [DescriptionAttribute("S begin, range 2"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_S2Begin { get; set; }

        [XmlElement]
        [DescriptionAttribute("V begin, range 2"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_V2Begin { get; set; }

        [XmlElement]
        [DescriptionAttribute("H end, range 2  (0...360)"),
         CategoryAttribute("Recognation RED color")]
        public int filterRED_H2End { get; set; }

        [XmlElement]
        [DescriptionAttribute("S end, range 2"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_S2End { get; set; }

        [XmlElement]
        [DescriptionAttribute("V end, range 2"),
         CategoryAttribute("Recognation RED color")]
        public byte filterRED_V2End { get; set; }

        [XmlElement]
        [DescriptionAttribute("Intesivity, how many 'bad plx' in image (0...255)"),
         CategoryAttribute("Recognation")]
        public byte filterBAD_Intensivity { get; set; }

        [XmlElement]
        [DescriptionAttribute("Reports setting"),
         CategoryAttribute("Report")]
        public ReportsSetting Report { get; set; }

        [XmlElement]
        [DescriptionAttribute("SerialNo of master camera"),
         CategoryAttribute("Hardware")]
        public string CameraMaster {
            get;
            set;
        }

        


        [XmlElement]
        [DescriptionAttribute("BlobView icon size"),
         CategoryAttribute("Interface")]
        public byte  BlobViewItemHeight { get; set; }

        [XmlElement]
        [DescriptionAttribute("BlobView icon size"),
         CategoryAttribute("Interface")]
        public byte  BlobViewItemWidth { get; set; }

    }

    //************************************************************************************************************************************************************************************************

    public class ReportsSetting
    {

        [Description("Width images in reports"),
         Category("Report Setting")]
        public int ImgWidth { get; set; }

        [Description("Height images in reports"),
         Category("Report Setting")]
        public int ImgHeigth { get; set; }

        [Description("Oriental locate src & bin images"),
         Category("Report Setting")]
        public bool ImgOriental { get; set; } // true- horizontal

        [Description("Width histo in reports"),
         Category("Report Setting")]
        public int HistoWidth { get; set; }

        [Description("Height histo in reports"),
         Category("Report Setting")]
        public int HistoHeigth { get; set; }

        [Description("Formula symbol"),
         Category("Report Setting")]
        public string FormulaSymbol { get; set; }

        [Description("Formula last coefficient"),
         Category("Report Setting")]
        public decimal FormulaLastCoef { get; set; }

        public ReportsSetting()
        {
            ImgWidth = 820;
            ImgHeigth = 650;
            ImgOriental = true;

            HistoWidth = 800;
            HistoHeigth = 400;

            FormulaSymbol = "Formula";
            FormulaLastCoef = 1;
        }

        public override string ToString()
        {
            return "Reports Settings";
        }
    }
     
    //*************************************************************************************************************************************************** Argument for events ****************************************
    /// <summary>
    /// Класс аргументов для SettingsEventHandler
    /// </summary>
    public class ConfigSettingArgs : EventArgs
    {
        private readonly ConfigSetting settings;

        public ConfigSettingArgs(ConfigSetting e)
        {
            this.settings = e;
        }

        /// <summary>
        /// Настройки
        /// </summary>
        public ConfigSetting Settings { get { return this.settings; } }
    }

    //**************************************************************************************************************************************************** Delegat for events ****************************************
    /// <summary>
    /// Class delegate for events related to the acquisition settings
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SettingsEventHandler(object sender, ConfigSettingArgs e);


}
