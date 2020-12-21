

#region description
///////////////////////////////////////////////////////////////////////
///                          simple "blob" detection                ///
/// ******************************************************************
/// what is blob - Bitmap RGB ( in Tag contain ( blob_tag)object
/// ******************************************************************
//
/// Depends:
///         nuget VVVV.EmguCV.2.4.2.1
//cublas32_42_9.dll                      32 or 64
//cudart32_42_9.dll
//cufft32_42_9.dll
//cvextern.dll
//Emgu.CV.dll
//Emgu.CV.UI.dll
//Emgu.Util.dll
//npp32_42_9.dll
//opencv_core242.dll
//opencv_imgproc242.dll
///******************************************************************
///
/// Example use:
/// 
/// Flokal.Camera.IDSMS drv { get; set; }
/// ...
/// var detector = new blobs_opencv.detect_blobs();
/// detector.NewBlobs += new detect_blobs.DetectNewBlobHandle(addNewBlobs);
/// ...
/// drv = new IDSMS();
/// drv.Init(0, -1, -1, 24, null, pictureBox1.Handle); // -1 - liner mode
/// drv.TurnON();
/// drv.ImageCaptured += new Flokal.Camera.CameraBridgeEventHandler(CameraCapture);
/// ...
/// private void CameraCapture(object source, CameraBridgeEventArgs e)
/// {
///    Bitmap front = (Bitmap)e.ImageMaster; // or ImageSlave
///    detector.Core(newframe, DateTime.Now);
/// }
/// ...
/// private void addNewBlobs(List<Bitmap> bitmaps)
/// {
///    // TO-DO: Your activity ...
/// }
///
/// *******************************************************************
///                         Microptik 2019.06.05
/// ///////////////////////////////////////////////////////////////////
#endregion

#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing ;
using beans;
using beans.filters;
using beans.methods;
using Emgu.CV.Freetype;
using Emgu.CV.Util;
using Polenter.Serialization;
using System.ComponentModel;
#endregion

namespace beans
{
    public class detect_blobs
    {
        #region variables

        const bool notUsing = false, Using = true;

        public delegate void DetectNewBlobHandle(List<Bitmap> blobs);

        public event DetectNewBlobHandle NewBlobs;

        public delegate void ShowImageHandle(Bitmap b);

        public event ShowImageHandle ShowFrameRgb;
        public event ShowImageHandle ShowFrameBin;

        private int heightOfFrame;
        private int MinHeightBeaninFrame;
        private bool isMaster;
        private List<Bitmap> blobs = new List<Bitmap>();
        private List<Image<Bgr, byte>> frames;
        private List<DateTime> frameTimes;
        private List<Func<Bitmap, ConfigSetting, Bitmap>> listOfFilter;
        private Func<Image<Bgr, Byte>, ConfigSetting, double> lineColorEmpty;
        private Image<Bgr, Byte> imgMultyFrame;
        private bool isNewMyltyFrame = true;
        public ThresholdType ThresholdType_METHOD;
        public byte ThresholdType_VAL;
        private byte ThresholdType_MAX;
        private ColorRange colorRange;
        private ConfigSetting cfg;

        private Image<Bgr, byte> lastFrame;
        private DateTime timeCalc;

        #endregion

        public detect_blobs()
        {
            MinHeightBeaninFrame = 3;
            isMaster = true;

            cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);

            ThresholdType_METHOD = (ThresholdType) cfg.BinMethod;
            ThresholdType_VAL = (byte) cfg.BinVal;
            ThresholdType_MAX = 255;
            
            colorRange = new ColorRange("Red");

            listOfFilter = (new filters.manager()).get();
            lineColorEmpty = (new filters.manager()).getColorEmpty();
        }

        public detect_blobs(ColorRange cr, ThresholdType tm = ThresholdType.BinaryInv, byte tv = 170, byte tmx = 255,
            int minHeightBeaninFrame = 2, bool ismaster = true)
        {
            MinHeightBeaninFrame = minHeightBeaninFrame;
            isMaster = ismaster;

            cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);

            ThresholdType_METHOD = (ThresholdType) cfg.BinMethod;
            ThresholdType_VAL = (byte) cfg.BinVal;
            ThresholdType_MAX = tmx;
            
            colorRange = cr;

            listOfFilter = (new filters.manager()).get();
            lineColorEmpty = (new filters.manager()).getColorEmpty();

        }

        // create img with sample(s) from liner img (ex. loop from camera)
        public void Core(Bitmap newframe, DateTime frameTime)
        {
            Core(new Image<Bgr, Byte>(newframe), frameTime);
        }

        // create img with sample(s) from liner img (ex. loop from camera). opencv ver
        public void Core(Image<Bgr, Byte> newframe, DateTime frameTime)
        {

            heightOfFrame = newframe.Height;

            if (isNewMyltyFrame)
            {
                blobs = new List<Bitmap>();
                isNewMyltyFrame = false;
            }

            if (frames == null) frames = new List<Image<Bgr, byte>>();
            frames.Add(newframe);

            if (frameTimes == null) frameTimes = new List<DateTime>();
            frameTimes.Add(frameTime);

            if (common.LineIsEmpty(newframe, cfg) && (lastFrame != null && common.LineIsEmpty(lastFrame, cfg)))
//            if (common.LineIsEmpty(newframe, cfg, lineColorEmpty) && (lastFrame != null && common.LineIsEmpty(lastFrame, cfg, lineColorEmpty)))
            {

                if (frames.Count > MinHeightBeaninFrame)
                {
                    try
                    {
                        try
                        {
                            ShowFrameRgb?.Invoke(imgMultyFrame.Clone().Bitmap); // send main app RGB img with sample(s)
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

                        DateTime timeDetect = DateTime.Now;

                        blobs = DetectBlobs(imgMultyFrame);
                        FillConcurentTube(blobs);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }


//#if (TRACE)
//                    Flokal.Protocol.Logging.Trace($"lm_detect_blobs.Core(...)", $"Frames:{frames.Count}, Detecting time:", timeDetect);
//
//                    Flokal.Protocol.Logging.Trace($"lm_detect_blobs.Core(...)", $"Frames:{frames.Count}, Recognazing time:", timeCalc);
//#endif

                    imgMultyFrame.Dispose();
                    imgMultyFrame = null;
                    isNewMyltyFrame = true;
                }
                else
                {
                    timeCalc = DateTime.Now;
                    imgMultyFrame = newframe;
                }

                frameTimes.Clear();
                frames.Clear();

            }
            else
            {
                imgMultyFrame = LineMerge(newframe, imgMultyFrame);
            }

            lastFrame = newframe.Copy();
        }

        // transfer detected blobs to main app  
        private void FillConcurentTube(List<Bitmap> blobs)
        {
            try
            {
                if (blobs != null && blobs.Count > 0)
                {
                    //blobs = blobs.OrderBy(x => ((blob_tag) x.Tag).time).ToList();
                    NewBlobs?.Invoke(blobs);// send main app detected blobs
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // detect blobs from merged image
        private List<Bitmap> DetectBlobs(Image<Bgr, byte> img_bgr)
        {
            Image<Bgr, byte> img_bgr_source = null;
            Image<Gray, Byte> img_gray_source = null;
            Image<Gray, Byte> img_gray = common.Binaraze(img_bgr, ThresholdType_METHOD, ThresholdType_VAL, ThresholdType_MAX);
            Image<Gray, Byte> img_canny = new Image<Gray, byte>(img_gray.Size);
            int heightSource = img_bgr.Height;

            try
            {
                
                int minBlobSizePerPxl = heightOfFrame * MinHeightBeaninFrame;

                UMat hier = new UMat();
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

                CvInvoke.FindContours(img_gray, contours, hier, Emgu.CV.CvEnum.RetrType.Tree,
                    Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);

                if (contours.Size > 1)
                    contours = mergeContours(contours);

                if (contours.Size > 1) // save source image, look in loop -> /* if (i > 0) */
                {
                    img_bgr_source = img_bgr.Clone();
                    img_gray_source = img_gray.Clone();
                }

                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle r = CvInvoke.BoundingRectangle(contours[i]);

                    if (r.Width > minBlobSizePerPxl && r.Height > minBlobSizePerPxl)
                    {
                        Bitmap bmp_blob, bmp_blob_r;
                        Image<Gray, byte> img_gray_blob;

                        if (i > 0) // restore source image
                        {
                            img_bgr = img_bgr_source.Clone();
                            img_gray = img_gray_source.Clone();
                        }

                        img_bgr.ROI = img_gray.ROI = r; // prepare  to crop img of sample

                        if (Using) // base contour version
                        {
                            bmp_blob_r = (img_bgr.Copy( /*img_gray*/)).Bitmap; // crop
                            img_gray_blob = img_gray.Copy();                   // crop
                            bmp_blob_r.Tag = new blob_tag(LineCalcFixTime(img_gray, r), r, img_bgr.Copy(),
                                img_gray_blob, contours[i]);
                        }


                        if (notUsing) // biggest field of view for look more red plx., optimazed
                        {
                            img_bgr = valves.ROI(img_bgr, r, isMaster, heightSource); // special crop

                            img_gray_blob = common.Binaraze(img_bgr, ThresholdType_METHOD, ThresholdType_VAL,
                                    ThresholdType_MAX);

                            bmp_blob_r = img_bgr.Bitmap;
                            bmp_blob_r.Tag = new blob_tag(LineCalcFixTime(img_gray, r), r, img_bgr.Copy(),
                                    img_gray_blob, contours[i]);
                    
                        }

                        // TO-DO: CONTOUR(mophoparameters) for red is unreal becouce common.Binaraze(...) set for white sample. [lm_detect_blobs_opencv.cs]

                        if (!common.LineIsEmpty(img_bgr, cfg, 5))
                        {
                            foreach (var func in listOfFilter)
                                    bmp_blob_r = func(bmp_blob_r, cfg); // run filter RED, mophoparameters   ...

                            blobs.Add(bmp_blob_r);

                            if (ShowFrameBin != null)
                            {
                                img_canny.Draw(r, new Gray(255), 1);               // draw rectange
                                //CvInvoke.DrawContours(img_canny,contours, i, new MCvScalar(255)); // draw contour ( bad for red samples )
                            }

                        }
                    }
                }

                 ShowFrameBin?.Invoke(img_canny.Clone().Bitmap);  // send main app bin img

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return blobs;

        }

        private VectorOfVectorOfPoint mergeContours(VectorOfVectorOfPoint _contours)
        {
            var dist = cfg.widthOfCameraView / cfg.ValvesCount/* / 2.5*/;
            List<VectorOfPoint> list = new List<VectorOfPoint>();

            List<VectorOfPoint> contours = new List<VectorOfPoint>();
            for (int i = 0; i < _contours.Size; i++) contours.Add(_contours[i]);

            List<Moments> moments = new List<Moments>();
            for (int i = 0; i < _contours.Size; i++) moments.Add(CvInvoke.Moments(_contours[i], false));


            for (int i = 0; i < contours.Count; i++)
            {

                if (contours[i] != null && moments[i].M00 != 0    )
                {

                    VectorOfPoint n = contours[i];

                    //for (int j = 0; j < contours.Count; j++)
                    for (int j = i; j < contours.Count; j++)
                    {
                        if (i != j && contours[j] != null && moments[j].M00 != 0)
                        {
                            if (find_if_close(moments[i], moments[j], dist, cfg.widthOfCameraView ))
                            {
                                n.Push(contours[j]);

                                contours[j] = null;
                            }
                        }
                    }

                    list.Add(n);

                }
            }
        

            VectorOfVectorOfPoint res = new VectorOfVectorOfPoint(list.ToArray());

            if (res .Size>1)
            {
                ;
            }
            return res;
        }

        private bool find_if_close(Moments m1, Moments m2, double dist, int width )
        {
            var c1 = new Point((int)(m1.M10 / m1.M00),(int)(m1.M01 / m1.M00));

            var c2 = new Point((int)(m2.M10 / m2.M00), (int)(m2.M01 / m2.M00));
            double x;
            try
            {
//                x = morphoMinEncloseCircle.ComputeDistance(c1, c2);
                x = Math.Abs(c1.X - c2.X);//morphoMinEncloseCircle.ComputeDistance(c1, c2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return (x < width ? x : 0 ) < dist;
        }

        private DateTime LineCalcFixTime(Image<Gray, byte> iB, Rectangle rectangle)
        {
            int bottomOfBlob = /*rectangle.Y + */rectangle.Height;
            int inxFrame = (iB.Height - bottomOfBlob) / heightOfFrame + 1;
            return frameTimes[inxFrame];
        }


        private Image<Bgr, Byte> LineMerge(Image<Bgr, Byte> image1, Image<Bgr, Byte> image2)
        {
            int ImageWidth = 0;
            int ImageHeight = 0;

            if (image2 == null) return image1;

            //get max width
            if (image1.Width > image2.Width)
                ImageWidth = image1.Width;
            else
                ImageWidth = image2.Width;

            //calculate new height
            ImageHeight = image1.Height + image2.Height;

            //declare new image (large image).
            Image<Bgr, Byte> imageResult = new Image<Bgr, Byte>(ImageWidth, ImageHeight);


            imageResult.ROI = new Rectangle(0, 0, image1.Width, image1.Height);
            image1.CopyTo(imageResult);
            imageResult.ROI = new Rectangle(0, image1.Height, image2.Width, image2.Height);
            image2.CopyTo(imageResult);
            imageResult.ROI = Rectangle.Empty;

            return imageResult;
        }

        //private int ccnntt = 0;


    }


    public class blob_tag
    {
        [ExcludeFromSerialization]
        public Image<Bgr, byte> Image_bgr { get; set; }
        [ExcludeFromSerialization]
        public Image<Gray, byte> Image_bin{get;set;}

        public VectorOfPoint contour{get;set;}

        public long index { get; set; }
        public Rectangle rec{get;set;}
        public DateTime time{get;set;}
        public bool check{get;set;}
        public Color color{get;set;}
        
        // morpho
        public measure.square  area{get;set;}
        public measure.distance perimetr{get;set;}
        public measure.distance Rmax { get; set; }
        public measure.distance Rmin { get; set; }

        public Moments moments{get;set;}
        public bool convexity{get;set;}
        public measure.square convexity_area { get; set; }
        public measure.distance convexity_perimetr { get; set; }
        public double convexity_compactness { get; set; }

        public CircleF minEncloseCircle{get;set;}
        public double orientation { get; set; }
        public double roundness { get; set; }
        public double compactness { get; set; }

        public double elongation { get; set; }
        public      measure.distance maxAxis { get; set; }
        public measure.distance minAxis { get; set; }



        public double intensivity;



        public int blowInterval; //milisecond
        public int valve;

        public Point center
        {
            get
            {

                Point res = new Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);
//                try
//                {
//                    moments = moments ?? CvInvoke.Moments(contour, false);
//                    int cx = (int)(moments.M10 / moments.M00);
//                    int cy = (int)(moments.M01/ moments.M00);
//                    res = new Point(cx, cy);
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e);
//                }
                return res;
            }
        }

        public blob_tag( )
        {
            Image_bin = null ;
            index = -1;
        }


        public blob_tag(blob_tag tag)
        {
            index = -1;
            time = tag.time;
            rec = tag .rec;
            check = tag.check;
            Image_bin = tag.Image_bin;

        }
        public blob_tag(DateTime t, Rectangle r, Image<Gray,byte> i, VectorOfPoint v)
        {
            index = -1;

            time = t;
            rec = r;
            Image_bin = i;
            contour = v;
        }

        public blob_tag(DateTime t, Rectangle r, Image<Bgr, byte> ic, Image<Gray,byte> ib, VectorOfPoint v)
        {
            index = -1;

            time = t;
            rec = r;
            Image_bgr = ic;
            Image_bin = ib;
            contour = v;
        }
        
        public blob_tag(DateTime t)
        {
            index = -1;

            time = t;
        }

        public override string ToString()
        {
            int n = 10;
            /* string  res = Rec:".PadRight(n) + rec.ToString() + "\r\n";*/
            string res = ((index  != -1) ? "Sample #".PadRight(n) + index  + "\r\n" : "");
            res += ((area != null) ? "Area:".PadRight(n) + area + "\r\n" : "");
                res += ((perimetr!=null) ? "Perimeter:".PadRight(n) + perimetr + "\r\n" : "");
                res += ((elongation != 0) ? "Elongation:".PadRight(n) + elongation + "\r\n" : "");
                res += ((roundness != 0) ? "Roundness:".PadRight(n) + roundness + "\r\n" : "");
                res += ((maxAxis != null) ? "Rmax:".PadRight(n) + maxAxis + "\r\n" : "");
                res += ((minAxis != null) ? "Rmin:".PadRight(n) + minAxis + "\r\n" : "");
              //  res += ((orientation  != null) ? "Angle:".PadRight(n) + orientation + "\r\n" : "");

                res += "Detect:".PadRight(n) + $"{time.Minute}:{time.Second}:{time.Millisecond}\r\n";
                
                res += ((!color.IsEmpty) ? "Color:".PadRight(n) + color + "\r\n" : "");
                res += ((intensivity != 0) ? "Intensivity:".PadRight(n) + intensivity.ToString("f2") + "\r\n" : "");

            res += "Checked:".PadRight(n) + check + "\r\n";
            if (check) res += "Valve:".PadRight(n) + valve;
            
#if (DEMO)
//            res += "Center:".PadRight(n) + center;
            res += "Valve:".PadRight(n) + valve;
#endif

                return res;

        }

    }

    public class ColorRange
    {
        public List<UInt16[]> fr;
        public List<UInt16[]> to;

        public ColorRange()
        {
            init();
        }

        public ColorRange(String  color)
        {
            init();

            switch (color)
            {
                case "Red":
                    Add(new ushort[] { 0, 100, 0 }, new ushort[] { 10, 255, 255 });
                    Add(new ushort[] { 170, 100, 0 }, new ushort[] { 180, 255, 255 });
                    //Add(new ushort[] { 0, 70, 50 }, new ushort[] { 10, 255, 255 });
                    //Add(new ushort[] { 170, 70, 50 }, new ushort[] { 170, 255, 255 });
                    break;

                case "Blue":
                    //throw new Exception("Range of color don't implement");
                    break;

                case "Green":
                    //throw new Exception("Range of color don't implement");
                    break;
            }
        }
        public void Add(UInt16[] f, UInt16[] t)
        {
            fr.Add(f);
            to.Add(t);
        }

        void init()
        {
            fr = new List<ushort[]>();
            to = new List<ushort[]>();
        }





    }
}
