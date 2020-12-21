using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace beans.methods
{
    class valves
    {
        public static List<int[]> rangeM, rangeS;
        static ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
        public static int StepMaster, StepSlave;


        public static List<int[]> init()
        {
            rangeM =    new List<int[]>();
            rangeS =    new List<int[]>();
            int step = StepMaster = StepSlave = (cfg.widthOfCameraView - cfg.ValveLeftShift - cfg.ValveRightShift) / cfg.ValvesCount;

            int currPos = cfg.ValveLeftShift;
            for (int i = 0; i < cfg.ValvesCount; i++)
            {
                rangeM.Add(new int[] {currPos , (currPos + step) - 1});
                rangeS.Add(new int[] {currPos , (currPos += step) - 1});
            }
                return rangeM;
        }

        public static List<int[]> init(int widthM, int widthS)
        {
            // M
            rangeM =    new List<int[]>();
            int step = StepMaster = (widthM ) / cfg.ValvesCount;

            int currPos = cfg.ValveLeftShift;
            for (int i = 0; i < cfg.ValvesCount; i++)
            {
                rangeM.Add(new int[] {currPos , (currPos += step) - 1});
            }

            // S
            rangeS = new List<int[]>();
            step = StepSlave = (widthS) / cfg.ValvesCount;

            currPos = cfg.ValveLeftShift;
            for (int i = 0; i < cfg.ValvesCount; i++)
            {
                rangeS.Add(new int[] { currPos, (currPos += step) - 1 });
            }

            return rangeM;
        }

        public static int calc(Bitmap blob, bool isMaster = true)
        {
            int res = -1;
            int j = 0;

            Point center = ((blob_tag) blob.Tag ).center;

            if (rangeM == null)
            {
                init();
            }

            var range = isMaster ? rangeM : rangeS;

            if (isMaster)
            {
                for (int i = 0; i < range.Count; i++)
                {
                    if (range[i][0] <= center.X && center.X < range[i][1])
                    {
                        res = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = range.Count-1; i >= 0 ; i--)
                {
                    if (range[i][0] < center.X && center.X <= range[i][1])
                    {
                        res = j;
                        break;
                    }

                    j++;
                }
            }

            return res; //(isMaster) ? res : rotate( res );
        }

        public static int rotate(int valve)
        {
            return rangeM.Count - valve - 1;
        }

        public static Bitmap show(Bitmap b,bool isMaster = true )
        {
            var fnt = new Font("Times new Roman", 8, FontStyle.Regular);


            if (rangeM == null)
            {
                init();
            }



            var range = isMaster ? rangeM : rangeS;


            if (cfg.isValvesShow)
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    for (int i = 1; i < range.Count ; i++)
                    {
                        g.DrawLine(Pens.Gray, new Point(range[i][0], 4), new Point(range[i][0], b.Height - 4));
                        g.DrawString(range[i][0].ToString(), fnt, Brushes.Red, range[i][0]-20, 0);
                        g.DrawString(((isMaster) ? i : rotate(i)).ToString(), fnt, Brushes.Green, range[i][0]+40, b.Height/4*3);

                    }
                }
            }

            return b;
        }
        // cut sample from image 
        public static Bitmap ROI(Image<Bgr, byte> imgBgr, Bitmap bmp_blob, bool isMaster, bool isUseValve)
        {
            Rectangle limitate;

            if (isUseValve) // cut cut sample from image between valve's border
            {
                int valve = valves.calc(bmp_blob, isMaster);
                limitate  = new Rectangle(rangeM[valve][0], 0, rangeM[valve][1] - rangeM[valve][0], imgBgr.Height);
            }
            else           // cut sample from image as               [StepX/2 <- center -> StepX/2]
            {
                Point c = ((blob_tag) bmp_blob.Tag).center;
                int s  = isMaster ? StepMaster : StepSlave;
                int x1 = (c.X - s / 2) < 0 ? 0 : c.X - s / 2;

                limitate = new Rectangle(x1, 0,s, imgBgr.Height);
            }

            imgBgr.ROI = limitate;
            return imgBgr.Copy().Bitmap;
        }

        // cut sample from image as               [StepX/2 <- center -> StepX/2]
        public static Image<Bgr, byte> ROI(Image<Bgr, byte> imgBgr, Rectangle rec, bool isMaster, int heightSource)
        {
            Image<Bgr, byte> res = null;

            try
            {

                int centerX = rec.X + rec.Width / 2;
                int stepValve = isMaster ? StepMaster : StepSlave;
                int xStart = (centerX - stepValve / 2) < 0 ? 0 : centerX - stepValve / 2;

                imgBgr.ROI = new Rectangle(xStart, 0, stepValve, heightSource - 1);
                res = imgBgr.Copy();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return res;
        }

    }
}
