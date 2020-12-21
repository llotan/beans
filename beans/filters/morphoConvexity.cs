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
    public class morphoConvexity
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            var original = ((beans.blob_tag) arg.Tag).contour;
            ((beans.blob_tag)arg.Tag).convexity = CvInvoke.IsContourConvex( original );

            VectorOfPoint hull=new VectorOfPoint();
            CvInvoke.ConvexHull(original, hull, false);

            VectorOfPoint contur = new VectorOfPoint();
            CvInvoke .ApproxPolyDP(hull, contur, 0.001,true );

            ((beans.blob_tag) arg.Tag).convexity_area = new square(CvInvoke.ContourArea(contur));
            ((beans.blob_tag) arg.Tag).convexity_perimetr = new distance(CvInvoke.ArcLength(contur, true));

            return arg;
        }

    }
}
