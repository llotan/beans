#region description
//Image moments help you to calculate some features like center of mass of the object, area of the object etc
// look http://en.wikipedia.org/wiki/Image_moment
#endregion

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
    public class morphoMoment
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            ((beans.blob_tag)arg.Tag).moments = CvInvoke.Moments( ((beans.blob_tag)arg.Tag).contour, false );   

            return arg;
        }

    }
}
