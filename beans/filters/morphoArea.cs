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
using beans.measure;
using Emgu.CV.Util;

namespace beans.filters
{
    public class morphoArea
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            ((beans.blob_tag)arg.Tag).area = new square(CvInvoke.ContourArea(((beans.blob_tag)arg.Tag).contour));

            return arg;
        }


    }
}
