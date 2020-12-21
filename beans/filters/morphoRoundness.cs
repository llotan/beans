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
    public class morphoRoundness
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            var A = ((beans.blob_tag) arg.Tag).area = ((blob_tag) arg.Tag).area ?? new square(CvInvoke.ContourArea(((beans.blob_tag) arg.Tag).contour));
            var P = ((beans.blob_tag)arg.Tag).perimetr = ((blob_tag)arg.Tag).perimetr ?? new distance(CvInvoke.ArcLength(((beans.blob_tag)arg.Tag).contour, true));

            ((beans.blob_tag) arg.Tag).roundness = (P.pixels * P.pixels) / (A.pixels * 4 * Math.PI);

            return arg;
        }


    }
}
