using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Flokal.Camera;

namespace beans
{
    public partial class FormSync : Form
    {
        public Flokal.Camera.IDSMS drv { get; set; }
        delegate void showhandle(bool v);
        private Timer t;
        private TimeSpan duration;
        private Bitmap bmpM,bmpS;
        //bool res;
        //private detect_blobs detectorMaster, detectorSlave;
        //private event showhandle shhwlbl;
        //private double H, S, V;

        #region interface


        private void Button3_Click(object sender, EventArgs e)
        {
            Bitmap b = new Bitmap(@"..\..\..\sample.bmp");
            pbMC.Image = b;
//            pbMC.Image = LineBinaraze(b, ThresholdType.CV_ThresholdType_BINARY_INV, 170, 255).Bitmap;
//            LineDetectBlobs(new Image<Bgr, byte>(b));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            



        }

        //private void Button1_Click(object sender, EventArgs e)
        //{
        //    if (button1.Text.Contains("start"))
        //    {
        //        drv = new IDSMS();

        //        drv.Init(0, -1, -1, 24, null, pbMC.Handle);
        //        drv.TurnON();
        //        //drv .SetParamFromIni("4103043267.ini");
        //        drv.ImageCaptured += new Flokal.Camera.CameraBridgeEventHandler(CameraCapture);
        //        button1.Text = "stop";
        //    }
        //    else
        //    {
        //        drv.TurnOFF();
        //        drv.Release();
        //        button1.Text = "start";

        //    }
        //}



        private void CameraCapture(object source, CameraBridgeEventArgs e)
        {


            var front = (Bitmap)e.ImageMaster;
            var back = (Bitmap)e.ImageSlave;


            pbMC.Image = Flokal.Common.Methods.Image.Clone(front);

            DateTime start = DateTime.Now;

#if (CV)


            Image<Gray, Byte> image1 = DetectRedCV(front);
            Image<Gray, Byte> image2 = DetectRedCV(back);

            Image<Gray, Byte> image = image1 | image2;

            pbMB.Image = image.ToBitmap();
#else
            BitmapData frontData = front.LockBits(
                new Rectangle(0, 0, front.Width, front.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


//            pictureBox2.Image = RedThresholdTypeoldB(frontData, out res, 0,front.Height ,0,0, H, S, V /*, fr, till*/);

            front.UnlockBits(frontData);

#endif 

            duration = DateTime.Now - start;

        }





        public FormSync()
        {
            InitializeComponent();
            t = new Timer();
            t.Interval = 50;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            label2.Text = duration.Milliseconds.ToString();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FormLife f = new FormLife();
            f.ShowDialog();
        }

        private void addNewBlobs(List<Bitmap> bitmaps)
        {
            foreach (Bitmap item in bitmaps)
            {
                //blobs.Add(item);
            }
        }


        #endregion

        #region newrealiz

        private void Button1_Click(object sender, EventArgs e)
        {
            int inx = 0;
            ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
            DateTime now = DateTime.Now;

            CvInvoke.UseOpenCL = true;

            if (button1.Text.Contains("start"))
            {
                {
                    detect_blobs detectorMaster = new detect_blobs(new ColorRange("Red"), (ThresholdType) cfg.BinMethod,(byte) cfg.BinVal, 255);
                    detectorMaster.NewBlobs += new detect_blobs.DetectNewBlobHandle(masterBlobsHaveAdded);
                    detectorMaster.ShowFrameRgb +=new detect_blobs.ShowImageHandle(masterShowFrameRgb); // slowing - don't use
                    detectorMaster.ShowFrameBin += new detect_blobs.ShowImageHandle(masterShowFrameBin);

                    List<Bitmap> lM = methods.common.PrepareLinesBitmaps(@"..\..\..\..\merge"+inx+".jpg",4);
                    lM.Reverse();
                    //for (int i = lM.Count - 1; i > 0; i--) detectorMaster.Core(new Image<Bgr, byte>(lM[i]), DateTime.Now);
                    foreach (var item in lM)
                    {
                        detectorMaster.Core(new Image<Bgr, byte>(item), now);
                    }
                }                                                       

                {
                    detect_blobs detectorSlave = new detect_blobs(new ColorRange("Red"), (ThresholdType) cfg.BinMethod,(byte) cfg.BinVal, 255);
                    detectorSlave.NewBlobs += new detect_blobs.DetectNewBlobHandle(slaveBlobsHaveAdded);
                    detectorSlave.ShowFrameRgb +=new detect_blobs.ShowImageHandle(slaveShowFrameRgb); // slowing - don't use
                    detectorSlave.ShowFrameBin += new detect_blobs.ShowImageHandle(slaveShowFrameBin);

                    List<Bitmap> lS = methods.common.PrepareLinesBitmaps(@"..\..\..\..\merge" + inx + "s.jpg",4);
                    for (int i = lS.Count - 1; i > 0; i--) detectorSlave.Core(new Image<Bgr, byte>(lS[i]), now);
                }

                timerAccessToButton.Start();
            }
            else
            {
                button1.Text = "start";
            }
        }

        private void masterBlobsHaveAdded(List<Bitmap> blobs)
        {
            try
            {
                if (blobs.Count > 0)
                    bmpM = blobs[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private void slaveBlobsHaveAdded(List<Bitmap> blobs)
        {
            try
            {
                if (blobs.Count > 0)
                    bmpS = blobs[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private void masterShowFrameBin(Bitmap b)
        {
            pbMB.Image = b;
        }

        private void TimerAccessToButton_Tick(object sender, EventArgs e)
        {
            if (bmpM != null && bmpS != null)
            {
                button2.Visible = button3.Visible = true;
                timerAccessToButton.Stop();
            }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            blobs_ms_compare b = new blobs_ms_compare();
            Point p= b.Aqustment(bmpM, bmpS, pbMC.Image.Width);
            label1.Text = p.ToString();
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            blobs_ms_compare b = new blobs_ms_compare();
            label1.Text = b.Compare(bmpM, bmpS).ToString();
        }

        private void masterShowFrameRgb(Bitmap b)
        {
            pbMC.Image = b;
        }

        private void slaveShowFrameBin(Bitmap b)
        {
            pbSB.Image = b;
        }

        private void slaveShowFrameRgb(Bitmap b)
        {
            pbSC.Image = b;
        }

        #endregion

//        private int heightOfFrame = 4;
//        private List<Bitmap> PrepareLinesBitmaps(string fn = @"..\..\..\sample.bmp")
//        {
//            List<Bitmap> res = new List<Bitmap>();
//            Bitmap b = new Bitmap(fn);
//            int w = b.Width - 1;
//
//            for (int t = 0; t < b.Height; t += heightOfFrame)
//            {
//                int h = b.Height - (t + heightOfFrame) < 0 ? b.Height - (t + heightOfFrame) : heightOfFrame;
//                Bitmap bi = b.Clone(new Rectangle(0, t, w, h), PixelFormat.Format24bppRgb);
//                res.Add(bi);
//            }
//            return res;
//        }

    }

}
