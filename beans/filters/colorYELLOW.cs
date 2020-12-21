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
    public class colorYELLOW
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            var value = DetectColorExist(arg, cfg);
            var intensity = methods.common.GetIntensity(value);

            ((beans.blob_tag)arg.Tag).check = !(intensity <= cfg.filterBAD_Intensivity);
            ((beans.blob_tag)arg.Tag).intensivity = intensity;
            
            if (((beans.blob_tag)arg.Tag).check)
            {
                ((beans.blob_tag) arg.Tag).color = Color.Yellow;
            }

            return arg;
        }

        public static double GetIntensity(Image<Bgr, Byte> frame, ConfigSetting cfg)
        {
            var value = DetectColorExist(frame.Bitmap, cfg);
            var intensity = methods.common.GetIntensity(value);
            return intensity;
        }

        private static Image<Gray, Byte> DetectColorExist(Bitmap front, ConfigSetting cfg)
        {

            Image<Hsv, Byte> imageHSV = new Image<Hsv, Byte>(front);

            Image<Gray, Byte> image1 = new Image<Gray, Byte>(front);

            CvInvoke.InRange(imageHSV, new ScalarArray(new MCvScalar(20,100,100 )),
                new ScalarArray(new MCvScalar(30, 255, 255)), image1);

            return image1;

        }

    }
}
