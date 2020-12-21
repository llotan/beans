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
    public class colorRED
    {

        public static Bitmap Run(Bitmap arg, ConfigSetting cfg)
        {
            var value = DetectColorExist(arg, cfg);
            var intensity = methods.common.GetIntensity(value);

            //            Image<Gray,byte> value1= new Image<Gray, byte>(value .Size );
            //            CvInvoke.FastNlMeansDenoising(value, value1,1f);
            //            var intensity = methods.common.GetIntensity(value1);

            ((beans.blob_tag)arg.Tag).check = !(intensity <= cfg.filterBAD_Intensivity);
            ((beans.blob_tag)arg.Tag).intensivity = intensity;


            if (((beans.blob_tag)arg.Tag).check)
            {
                ((beans.blob_tag) arg.Tag).color = Color.Red;
            }

            return arg;
        }

        public static double  GetIntensity(Image<Bgr, Byte> frame, ConfigSetting cfg)
        {
            var value = DetectColorExist(frame.Bitmap, cfg);
            var intensity = methods.common.GetIntensity(value);
            return intensity;
        }

        public static Image<Gray, Byte> DetectColorExist(Bitmap front, ConfigSetting cfg)
        {


            Image<Hsv, Byte> imageHSV = new Image<Hsv, Byte>(front);

            Image<Gray, Byte> image1 = new Image<Gray, Byte>(front);
            Image<Gray, Byte> image2 = new Image<Gray, Byte>(front);


            CvInvoke.InRange(imageHSV, new ScalarArray(new MCvScalar(cfg.filterRED_H1Begin, cfg.filterRED_S1Begin, cfg.filterRED_V1Begin )),
                new ScalarArray(new MCvScalar(cfg.filterRED_H1End, cfg.filterRED_S1End, cfg.filterRED_V1End)), image1);

            CvInvoke.InRange(imageHSV, new ScalarArray(new MCvScalar(cfg.filterRED_H2Begin, cfg.filterRED_S2Begin, cfg.filterRED_V2Begin)),
                new ScalarArray(new MCvScalar(cfg.filterRED_H2End, cfg.filterRED_S2End, cfg.filterRED_V2End)), image2);


            Image<Gray, Byte> image = image1 | image2;

            return image;

        }

    }
}
