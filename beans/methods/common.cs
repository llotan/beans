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
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV.Util;

namespace beans.methods
{
    public class common
    {
        public static bool LineIsEmpty(Bitmap frame, ConfigSetting cfg, byte intensity = 3)
        {
            return LineIsEmpty(new Image<Bgr, Byte>(frame), cfg, intensity);                                                                                                                            
        }

        public static bool LineIsEmpty(Image<Bgr, Byte> frame, ConfigSetting cfg, byte intensity = 1)
        {
            var gray = Binaraze(frame, (ThresholdType)cfg.BinMethod, cfg.BinVal, 255);
            return (LineIsEmpty(gray, intensity));
        }


        public static bool LineIsEmpty(Image<Bgr, Byte> frame, ConfigSetting cfg, Func<Image<Bgr, Byte> , ConfigSetting , double> advance,byte intensity = 1)
        {
            var gray = Binaraze(frame, (ThresholdType)cfg.BinMethod, cfg.BinVal, 255);
            var value1 = GetIntensity(gray);
            var value2 = advance == null ? 0 : advance(frame, cfg);
            var res = value1 > value2 ? value1 : value2;
            return (res <= intensity);
        }

        public static bool LineIsEmpty(Image<Gray, Byte> frame_gray, byte intensity = 0)
        {
            return (GetIntensity( frame_gray) <= intensity);
        }

        public static double  GetIntensity(Image<Gray, Byte> frame_gray)
        {
           double  ret= (frame_gray.GetAverage().Intensity );
//           Flokal.Protocol.Logging.Trace($"intensivity:{ret}");

            return ret;
        }

        public static Image<Gray, Byte> Binaraze(Bitmap b, ThresholdType tht = ThresholdType.BinaryInv, Double th = 170, Double max = 255)
        {
            return Binaraze(new Image<Gray, Byte>(b), tht, th, max);
        }

        public static  Image<Gray, Byte> Binaraze(Image<Bgr, Byte> rgb, ThresholdType tht = ThresholdType.BinaryInv, Double th = 170, Double max = 255)
        {
            return Binaraze(rgb.Convert<Gray, Byte>(), tht, th, max);
        }

        public static Image<Gray, Byte> Binaraze(Image<Gray, Byte> g, ThresholdType tht = ThresholdType.BinaryInv, Double th = 170, Double max = 255)
        {
            var bin = new Image<Gray, Byte>(g.Width, g.Height, new Gray(0));
            CvInvoke.Threshold(g, bin, th, max, tht);
            return bin;
        }

        public static  List<Bitmap> PrepareLinesBitmaps(string fn = @"..\..\..\..\sample.bmp", int heightOfFrame = 4)
        {
            List<Bitmap> ret, res = new List<Bitmap>();
            ret = new List<Bitmap>();

            if (File.Exists(fn))
            {
                

            Bitmap b = new Bitmap(fn);
            int w = b.Width - 1;

            for (int t = 0; t < b.Height; t += heightOfFrame)
            {
                int h = b.Height - (t + heightOfFrame) < 0 ? b.Height - (t + heightOfFrame) : heightOfFrame;
                Bitmap bi = b.Clone(new Rectangle(0, t, w, h), PixelFormat.Format24bppRgb);
                res.Add(bi);
            }

            }
            else
            {
                MessageBox.Show("Image "+fn + " not exist");
            }

            for (int i = res .Count- 1; i >= 0; i--)
            {
                ret.Add(res[i]);
            }

            return res;
        }

        public static Bitmap mirror(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            //            BitmapData sd = img.LockBits(new Rectangle(0, 0, w, h),
            //                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            //            int bytes = sd.Stride * sd.Height;
            //            byte[] buffer = new byte[bytes];
            //            byte[] result = new byte[bytes];
            //            Marshal.Copy(sd.Scan0, buffer, 0, bytes);
            //            img.UnlockBits(sd);
            //            int current, flipped = 0;
            //            for (int y = 0; y < h; y++)
            //            {
            //                for (int x = 4; x < w; x++)
            //                {
            //                    current = y * sd.Stride + x * 4;
            //                    flipped = y * sd.Stride + (w - x) * 4;
            //                    for (int i = 0; i < 3; i++)
            //                    {
            //                        result[flipped + i] = buffer[current + i];
            //                    }
            //                    result[flipped + 3] = 255;
            //                }
            //            }
            //            Bitmap resimg = new Bitmap(w, h);
            //            BitmapData rd = resimg.LockBits(new Rectangle(0, 0, w, h),
            //                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            //            Marshal.Copy(result, 0, rd.Scan0, bytes);
            //            resimg.UnlockBits(rd);
            Bitmap resimg = img.Clone(new Rectangle(0, 0, w, h), PixelFormat.Format24bppRgb);
            resimg.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return resimg;
        }


        public static void  clearFolder(string  pth)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(pth);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static     Bitmap generateEmptyLine(ConfigSetting cfg)
        {
            return new Bitmap(cfg.widthOfCameraView, cfg.heightOfCameraView, PixelFormat.Format24bppRgb);
        }

        private static Bitmap generateValueLine(ConfigSetting cfg)
        {
            Bitmap b = generateEmptyLine(cfg);
            using (Graphics g = Graphics.FromImage(b))
            {
                for(int h=0;h<b.Height;h++) g.DrawLine(new Pen( Color.Red), new Point(5,h),new Point(b.Height*15+5,h)  );
                //for(int h=0;h<b.Height;h++) g.DrawLine(new Pen( Color.Red), new Point(b.Height * 15 + 10, h),new Point(2*b.Height*15+10,h)  );
            }
            return b;
        }


        public static List<Bitmap> generateSample(ConfigSetting cfg)
        { 
            List<Bitmap> b = new List<Bitmap>();
            b.Add(common.generateEmptyLine(cfg));
            b.Add(common.generateEmptyLine(cfg));

            for (int i = 0; i < 15; i++) b.Add(common.generateValueLine(cfg));

            b.Add(common.generateEmptyLine(cfg));
            b.Add(common.generateEmptyLine(cfg));

            return b;
        }

    }
}
