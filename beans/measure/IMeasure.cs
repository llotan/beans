using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans.measure
{
    interface IMeasure
    {
        double pixels { get; set; }
        double mkm();
        double mm();
        double cm();

        string pixels_to_string(string format = "F0");
        string  mkm_to_string(string format = "F0");
        string  mm_to_string(string format = "F2");
        string  cm_to_string(string format = "F2");

    }


    public enum MeasureType { none, pixel,  micrometer, milimeter, cantimeter }

    public class measureBase : IMeasure
    {
        public static double factor; // set once when startup app

        public virtual double pixels { get{return 0;} set{} }
        public virtual double mkm(){return 0;}
        public virtual double mm(){return 0;}
        public virtual double cm(){return 0;}

        public virtual string pixels_to_string(string format = "F0"){return "";}
        public virtual string mkm_to_string(string format = "F0"){return "";}
        public virtual string mm_to_string(string format = "F2"){return "";}
        public virtual string cm_to_string(string format = "F2"){return "";}

        public static MeasureType measureType = MeasureType.none;

        public double value
        {
            get
            {
                double ret = pixels;

                switch (measureRead())
                {
                    case MeasureType.micrometer:
                        ret = mkm();
                        break;
                    case MeasureType.milimeter:
                        ret = mm();
                        break;
                    case MeasureType.cantimeter:
                        ret = cm();
                        break;
                }

                return ret;
            }
        }

        public override string ToString()
        {
            string ret = pixels_to_string();

            switch (measureRead())
            {
                case MeasureType.micrometer:
                    ret = mkm_to_string();
                    break;
                case MeasureType.milimeter:
                    ret = mm_to_string();
                    break;
                case MeasureType.cantimeter:
                    ret = cm_to_string();
                    break;
            }

            return ret;
        }

        public static MeasureType measureRead()
        {
            MeasureType ret = measureType;

            if (measureType == MeasureType.none)
            {
                ret = measureType = (MeasureType)SettingsManager.Instance.GetCurrent(Application.StartupPath).MeasureType;
            }

            return ret;
        }

        public static void factorRead()
        {

            if (factor == 0)
            {
                factor = SettingsManager.Instance.GetCurrent(Application.StartupPath).MeasureFactor;
            }

        }

    }

    public class distance :  measureBase//, IMeasure
    {
        private double pxl;
        private const double mkm_to_mm = 0.001000;
        private const double mm_to_cm = 0.0100;

        public distance() { }
        public distance(double _pxl){this.pixels = _pxl;factorRead();}

        public distance(double value, MeasureType type)
        {
            factorRead();
            type = (type == MeasureType.none) ? measureRead() : type;

            switch (type)
            {
                case MeasureType.pixel:
                    pixels = value;
                    break;
                case MeasureType.micrometer:
                    pixels = value / factor ;
                    break;
                case MeasureType.milimeter:
                    pixels = value / factor / mkm_to_mm;
                    break;
                case MeasureType.cantimeter:
                    pixels = value / factor / mkm_to_mm / mm_to_cm;
                    break;
            }

        }

 
        public override double pixels {get { return pxl; }set { pxl = value; }}

        public override double mkm(){return pxl * factor;}

        public override double mm(){return mkm() * mkm_to_mm ;}

        public override double cm(){return mm() * mm_to_cm;}

        public override string pixels_to_string(string format = "F0") {return pixels.ToString(format)+" pxl";}
        public override string mkm_to_string(string format = "F0") {return mkm().ToString(format)+ " μm"; }
        public override string mm_to_string(string format = "F2") {return mm().ToString(format)+" mm";}
        public override string cm_to_string(string format = "F2"){return cm().ToString(format)+" sm";}

        
    }

    public class square : measureBase
    {
        private double pxl;
        private const double mkm_to_mm = 0.001000;
        private const double mm_to_cm = 0.0100;

        public square()
        {
        }

        public square(double _pxl) { this.pixels = _pxl; factorRead();}

        public square(double value, MeasureType type)
        {
            factorRead();
            type = (type == MeasureType.none) ? measureRead() : type;

            switch (type)
            {
                case MeasureType.pixel:
                    pixels = value;
                    break;
                case MeasureType.micrometer:
                    pixels = value / (factor*factor);
                    break;
                case MeasureType.milimeter:
                    pixels = value / (factor*factor) / mkm_to_mm;
                    break;
                case MeasureType.cantimeter:
                    pixels = value / (factor*factor) / mkm_to_mm / mm_to_cm;
                    break;
            }

        }

        public override double  pixels { get { return pxl; } set { pxl = value; } }

        public override double mkm() { return pxl * factor * factor; }

        public override double mm() { return mkm() * 0.001; }

        public override double cm() { return mm() * 0.01; }

        public override string pixels_to_string(string format = "F0") { return pixels.ToString(format) + " pxl²"; }
        public override string mkm_to_string(string format = "F0") { return mkm().ToString(format) + " μm²"; }
        public override string mm_to_string(string format = "F2") { return mm().ToString(format) + " mm²"; }
        public override string cm_to_string(string format = "F2") { return cm().ToString(format) + " cm²"; }

    }

}
