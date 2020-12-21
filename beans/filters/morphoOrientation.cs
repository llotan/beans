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
    public class morphoOrientation
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            RotatedRect elps = CvInvoke.FitEllipse(((beans.blob_tag)arg.Tag).contour);

            ((beans.blob_tag) arg.Tag).orientation  = elps.Angle;
            ((beans.blob_tag)arg.Tag).maxAxis = new distance(elps.MinAreaRect().Width);
            ((beans.blob_tag)arg.Tag).minAxis = new distance(elps.MinAreaRect().Height);

            return arg;
        }


    }
}
