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
    public class morphoElongation
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            var m = ((beans.blob_tag)arg.Tag).moments = ((beans.blob_tag)arg.Tag).moments ?? CvInvoke.Moments(((beans.blob_tag)arg.Tag).contour, true );
            var bigSqrt = Math.Sqrt((m.M20 - m.M02) * (m.M20 - m.M02) + 4 * m.M11 * m.M11);
            ((beans.blob_tag) arg.Tag).elongation = (m.M20 + m.M02 + bigSqrt) / (m.M20 + m.M02 - bigSqrt);
            return arg;
        }


    }
}
