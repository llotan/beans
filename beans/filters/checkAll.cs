using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Util;

namespace beans.filters
{
    public class checkAll
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            ((beans.blob_tag) arg.Tag).check = true;

            return arg;
        }

        public static double GetIntensity(Image<Bgr, Byte> frame, ConfigSetting cfg)
        {
           return 255;
        }

    }
}
