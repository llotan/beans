using Mimax.RTF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessor;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;


namespace beans
{
    class reports
    {
        // standart report
        public static  void CreateStandartReport3(List<Bitmap> list/*,  int[] sample*/)
        {
            GC.Collect();

            MembersMorphoParameters mmp = MembersMorphoParameters.Deserialization();

            int inx = 0;
            Bitmap[] src = new Bitmap[list.Count];
            blob_tag[] blobs = new blob_tag[src.Length];

            foreach (Bitmap  image in list)
                src[inx++] = image;

            ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
            mmxRtfStore store = new mmxRtfStore();

            store.AddRTF(Application.StartupPath + @"\reports\blank.rtf");


            // draw images
            for (int i = 0; i < src.Length; i++)
            {
                blobs[i] = (blob_tag) src[i].Tag;
                Bitmap bsrc = blobs[i].Image_bgr.Bitmap; //src[i].Clone(new Rectangle(0,0,src[i].Width , src[i].Height), PixelFormat.Format24bppRgb);
                Bitmap bbin = blobs[i].Image_bin.Bitmap;


                if (false && bsrc.Width > cfg.Report.ImgWidth)
                {
                    Size size=new Size(cfg.Report.ImgWidth, cfg.Report.ImgHeigth);
                    bsrc = resize( bsrc, size , true );
                    bbin = resize(bbin, size, false);
                }

                store.AddText((cfg.Report.ImgOriental) ? "  Source / Binarised images:" : "  Source images:");
                store.AddParagraph();
                store.AddImage(bsrc);

                if (!cfg.Report.ImgOriental)
                {
                    store.AddText("  Binarised images:");
                    store.AddParagraph();
                }

                store.AddImage(bbin);
                store.AddParagraph();
            }

            // draw histograms
            string[,] toRep = new string[mmp.countToReport, blobs.Length];
            string[] colNames = new string[mmp.countToReport];
            int counter = 0;
            int j = 0;
            for (int i = 0; i < 16; i++)
            {
                if (mmp.mp[i].ToReport)
                {


                    string filtname = FilterName(mmp.mp[i].name,"mkm");
                    string s = "Histogram of " + filtname + " of particles ";
                    double[] z = Filter(mmp.mp[i].name, blobs);

                    colNames[counter] = mmp.mp[i].name;
                    string[] tmp = FilterTable(mmp.mp[i].name, blobs);
                    for (j = 0; j < blobs.Length; j++)
                        toRep[counter, j] = tmp[j];
                    counter++;
                    if (z != null)
                    {

                        store.AddHistogram(s, mmp.mp[i].name, "", "", z, 10, 5, 2, cfg.Report.HistoWidth, cfg.Report.HistoHeigth, true);
                        store.AddParagraph();
                    }
                }
            }
            counter--;

            // draw tables
            if (colNames.Length > 0)
            {
                string[] filname = FilterName(colNames,"mkm");
                store.AddTable(toRep, filname, "Morpho Parameters of particles");
            }

            // formula
            //FormulaArea fa = new FormulaArea(blobs, (new CoefRangeXML()).Exec());
            //decimal formula = fa.Exec();
            //store.AddText(fa.Formula, new Font("Courier New", 9, FontStyle.Regular));


            // save rep to tmp file
            store.SaveAndShow();

            GC.Collect();
        }

        public static void CreateLocalReport0(List<Bitmap> srcMaster, List<Bitmap> srcSlave,List <long []> link )
        {
            GC.Collect();

            MembersMorphoParameters mmp = MembersMorphoParameters.Deserialization();
            ConfigSetting cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
            mmxRtfStore store = new mmxRtfStore();

            blob_tag[] blobsMaster = new blob_tag[srcMaster.Count ];
            blob_tag[] blobsSlave  = new blob_tag[srcSlave.Count ];
            
            store.AddRTF(Application.StartupPath + @"\reports\blank.rtf");

            int good = 0;
            int bad  = 0;

            // draw images
            for (int i = 0; i < srcMaster.Count ; i++)
            {
                bool isPaire = (link != null && link.Count > i && link[i][1] > -1);
                
                blobsMaster[i] = (blob_tag)srcMaster[i].Tag;
                Bitmap bsrcMaster = blobsMaster[i].Image_bgr.Bitmap;//srcMaster[i].Clone(new Rectangle(0, 0, srcMaster[i].Width, srcMaster[i].Height), PixelFormat.Format24bppRgb);
                Bitmap bbinMaster = blobsMaster[i].Image_bin.Bitmap;
                Bitmap bsrcSlave = null;
                Bitmap bbinSlave = null;

                if (isPaire)
                {
                    Bitmap slaveBitmap = getSlave(srcSlave, link[i][1]);
                    blobsSlave[i] = (blob_tag) slaveBitmap.Tag;
                    bsrcSlave = blobsSlave[i].Image_bgr.Bitmap;
                     bbinSlave = blobsSlave[i].Image_bin.Bitmap;
                }

                if (false && bsrcMaster.Width > cfg.Report.ImgWidth)
                {
                    Size size = new Size(cfg.Report.ImgWidth, cfg.Report.ImgHeigth);
                    bsrcMaster = resize(bsrcMaster, size, true);
                    bbinMaster = resize(bbinMaster, size, false);
                    bsrcSlave = resize(bsrcSlave, size, true);
                    bbinSlave = resize(bbinSlave, size, false);
                }

                string resultat;
                if (blobsMaster[i].check || (isPaire && blobsSlave[i].check) )
                {
                    resultat = "bad";
                    bad++;
                }
                else
                {
                    resultat = "good";
                    good++;
                }
                // make report ...
                {
                    store.AddParagraph();

                    store.AddText($"Sample #{i + 1} is {resultat}");

                    store.AddParagraph();

                    store.AddImage(bsrcMaster);
                    store.AddImage(bbinMaster);
                    store.AddText(" - source / binarised images from camera #1");


                    store.AddParagraph();

                    if (isPaire)
                    {
                        store.AddImage(bsrcSlave);
                        store.AddImage(bbinSlave);
                        store.AddText(" - source / binarised images from camera #2");
                    }

                    //                    store.AddParagraph();
                }
                {
                    string[,] toRep = new string[mmp.countToReport, 2];
                    string[] colNames = new string[mmp.countToReport];
                    int counter = 0;
                    for (int ix = 0; ix < 16; ix++)
                    {
                        if (mmp.mp[ix].ToReport)
                        {
                            colNames[counter] = mmp.mp[ix].name;
                            blob_tag[] arr =(isPaire)? new blob_tag[]{blobsMaster[i],blobsSlave[i]} : new blob_tag[] { blobsMaster[i] };
                            string[] items = FilterTable(mmp.mp[ix].name, arr );
                            for (int j = 0; j < arr.Length; j++)
                                toRep[counter, j] = items[j];
                            counter++;
                        }
                    }
                    counter--;

                    // draw tables
                    if (colNames.Length > 0)
                    {
                        string[] filname = FilterName(colNames, "mkm");
                        store.AddTable(toRep, filname, $"morpho parameters of sample #{i+1}");
                    }

                }
            }


            store.AddParagraph();
            store.AddText($"Total conclusion by report: total detect samples - {srcMaster.Count} ( good - {good }, bad - {bad} )");


            // save rep to tmp file
            store.SaveAndShow();

            GC.Collect();
        }

        #region supports methods

        static Bitmap getSlave(List<Bitmap> slave, long  index)
        {
            Bitmap res = null;

            if (index != -1)
            {
                res = slave[(int)index];
            }

            return res;

        }

        private static string FilterName(string name, string meash)
        {

            string ret = "";



            switch (name)
            {
                case "Angle":
                    ret = name + " (degrees" + ")";
                    break;
                case "Area":
                    ret = name + " (sq." + meash + ")";
                    break;
                case "BigLen":
                    ret = name + " (" + meash + ")";
                    break;
                case "Compactness":
                    ret = name + " (units" + ")";
                    break;
                case "ConvArea":
                    ret = name + " (sq." + meash + ")";
                    break;
                case "ConvCompactness":
                    ret = name + " (units" + ")";
                    break;
                case "Convexity":
                    ret = name + " (units" + ")";
                    break;
                case "ConvPerimeter":
                    ret = name + " (" + meash + ")";
                    break;
                case "Elongation":
                    ret = name + " (units" + ")";
                    break;
                case "Perimeter":
                    ret = name + " (" + meash + ")";
                    break;
                case "Rmax":
                    ret = name + " (" + meash + ")";
                    break;
                case "Rmin":
                    ret = name + " (" + meash + ")";
                    break;
                case "RoughnessBArea":
                    ret = name + " (sq." + meash + ")";
                    break;
                case "Roundness":
                    ret = name + " (units" + ")";
                    break;
                case "SmallLen":
                    ret = name + " (" + meash + ")";
                    break;
                case "Rmax-Rmin":
                    ret = name + " (" + meash + ")";
                    break;
            }

            return ret;
        }
        private static string[] FilterName(string[] name, string meash)
        {

            string[] ret = new string[name.Length];
            //ImageProcessor3D.MembersScaleFactor msf = ImageProcessor3D.MembersScaleFactor.Deserialization();

            for (int i = 0; i < name.Length; i++)
                switch (name[i])
                {
                    case "Angle":
                        ret[i] = name[i] + " (degrees)";
                        break;
                    case "Area":
                        ret[i] = name[i] + " (sq." + meash + ")";
                        break;
                    case "BigLen":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "Compactness":
                        ret[i] = name[i] + " (units" + ")";
                        break;
                    case "ConvArea":
                        ret[i] = name[i] + " (sq." + meash + ")";
                        break;
                    case "ConvCompactness":
                        ret[i] = name[i] + " (units" + ")";
                        break;
                    case "Convexity":
                        ret[i] = name[i] + " (units" + ")";
                        break;
                    case "ConvPerimeter":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "Elongation":
                        ret[i] = name[i] + " (units" + ")";
                        break;
                    case "Perimeter":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "Rmax":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "Rmin":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "RoughnessBArea":
                        ret[i] = name[i] + " (sq." + meash + ")";
                        break;
                    case "Roundness":
                        ret[i] = name[i] + " (units" + ")";
                        break;
                    case "SmallLen":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                    case "Rmax-Rmin":
                        ret[i] = name[i] + " (" + meash + ")";
                        break;
                }

            return ret;
        }
        private static  double[] Filter(string name, blob_tag[] blobs)
        {
            ArrayList al = new ArrayList();
            double[] ret = null;

            for (int i = 0; i < blobs.Length; i++)
                switch (name)
                {
                    case "Angle":
                        al.Add(blobs[i].orientation);
                        break;
                    case "Area":
                        al.Add((blobs[i].area != null) ? blobs[i].area.value : 0);
                        break;
                    case "BigLen":
                        al.Add((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0);
                        break;
                    case "Compactness":
                            al.Add(blobs[i].compactness);
                        break;
                    case "ConvArea":
                        al.Add(((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0));
                        break;
                    case "ConvCompactness":
                            al.Add(blobs[i].convexity_compactness);
                        break;
                    case "Convexity":
                            al.Add(blobs[i].convexity);
                        break;
                    case "ConvPerimeter":
                        al.Add(((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0));
                        break;
                    case "Elongation":
                            al.Add(blobs[i].elongation);
                        break;
                    case "Perimeter":
                        al.Add(((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0));
                        break;
                    case "Rmax":
                        al.Add(((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0));
                        break;
                    case "Rmin":
                        al.Add(((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0));
                        break;
                    //case "RoughnessBArea":
                    //    al.Add(blobs[i].RoughnessBAreaK);
                    //    break;
                    case "Roundness":
                            al.Add(blobs[i].roundness);
                        break;
                    case "SmallLen":
                        al.Add(((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0));
                        break;
                    case "Rmax-Rmin":
                        al.Add(((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0) - ((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0));
                        break;
                }

            ret = new double[al.Count];
            for (int i = 0; i < al.Count; i++)
                if (name == "Angle")
                    ret[i] = (double)al[i];
                else
                    ret[i] = (double)al[i];
            return ret;
        }

        private static string[] FilterTable(string name, blob_tag[] blobs)
        {

            string[] ret = new string[blobs.Length];

            for (int i = 0; i < blobs.Length; i++)
                switch (name)
                {
                    case "Angle":
                        ret[i] = blobs[i].orientation.ToString("F2");
                        break;
                    case "Area":
                        ret[i] = ( (blobs[i].area != null) ? blobs[i].area.value : 0 ).ToString("F2");
                        break;
                    case "BigLen":
                        ret[i] = ((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0).ToString("F2");
                        break;
                    case "Compactness":
                            ret[i] = blobs[i].compactness.ToString("F2");
                        break;
                    case "ConvArea":
                        ret[i] = ((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0).ToString("F2");
                        break;
                    case "ConvCompactness":
                            ret[i] = blobs[i].convexity_compactness.ToString("F2");
                        break;
                    case "Convexity":
                            ret[i] = blobs[i].convexity.ToString();
                        break;
                    case "ConvPerimeter":
                        ret[i] = ((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0).ToString("F2");
                        break;
                    case "Elongation":
                            ret[i] = blobs[i].elongation.ToString("F2");
                        break;
                    case "Perimeter":
                        ret[i] = ((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0).ToString("F2");
                        break;
                    case "Rmax":
                        ret[i] = ((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0).ToString("F2");
                        break;
                    case "Rmin":
                        ret[i] = ((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0).ToString("F2");
                        break;
                    //case "RoughnessBArea":
                    //    ret[i] = blobs[i].RoughnessBarea.ToString("F2");
                    //    break;
                    case "Roundness":
                            ret[i] = blobs[i].roundness.ToString("F2");
                        break;
                    case "SmallLen":
                        ret[i] = ((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0).ToString("F2");
                        break;
                    case "Rmax-Rmin":
                        ret[i] =( ((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0)- ((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0)).ToString("F2");
                        break;
                }

            return ret;
        }

        public static blob_tag[] Filter(blob_tag[] blobs, double exactMin, double exactMax, string param)
        {
            if (blobs == null) return null;

            ArrayList al = new ArrayList();

            for (int i = 0; i < blobs.Length; i++)
                switch (param)
                {
                    case "Angle":
                        if (blobs[i].orientation  >= exactMin && blobs[i].orientation  <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Area":
                        if ( ((blobs[i].area != null) ? blobs[i].area.value : 0) >= exactMin && ((blobs[i].area != null) ? blobs[i].area.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "BigLen":
                        if (((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0) >= exactMin && ((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Compactness":
                            if (blobs[i].compactness >= exactMin && blobs[i].compactness <= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "ConvArea":
                        if (((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0) >= exactMin && ((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0)<= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "ConvCompactness":
                            if (blobs[i].convexity_compactness >= exactMin && blobs[i].convexity_compactness <= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "Convexity":
                    //        if (blobs[i].convexity >= exactMin && blobs[i].convexity <= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "ConvPerimeter":
                        if (((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0) >= exactMin && ((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Elongation":
                            if (blobs[i].elongation >= exactMin && blobs[i].elongation <= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "Perimeter":
                        if (((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0) >= exactMin && ((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmax":
                        if (((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0) >= exactMin && ((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmin":
                        if (((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0) >= exactMin && ((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0)<= exactMax)
                            al.Add(blobs[i]);
                        break;
                    //case "RoughnessBArea":
                    //    if (blobs[i].RoughnessBAreaK >= exactMin && blobs[i].RoughnessBAreaK <= exactMax)
                    //        al.Add(blobs[i]);
                    //    break;
                    case "Roundness":
                        if (blobs[i].roundness >= exactMin && blobs[i].roundness <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "SmallLen":
                        if (((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0) >= exactMin && ((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmax-Rmin":
                        if (((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0)-((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0) >= exactMin && ((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0)-((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                }

            blob_tag[] ret = new blob_tag[al.Count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = (blob_tag)al[i];
            return ret;
        }

        public static blob_tag[] FilterOuter(blob_tag[] blobs, double exactMin, double exactMax, string param)
        {
            if (blobs == null) return null;

            ArrayList al = new ArrayList();

            for (int i = 0; i < blobs.Length; i++)
                switch (param)
                {
                    case "Angle":
                        if (blobs[i].orientation <= exactMin || blobs[i].orientation >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Area":
                        if (((blobs[i].area != null) ? blobs[i].area.value : 0) >= exactMin && ((blobs[i].area != null) ? blobs[i].area.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "BigLen":
                        if (((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0) >= exactMin && ((blobs[i].maxAxis != null) ? blobs[i].maxAxis.value : 0) <= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Compactness":
                            if (blobs[i].compactness <= exactMin || blobs[i].compactness >= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "ConvArea":
                        if (((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0)  <= exactMin || ((blobs[i].convexity_area!= null) ? blobs[i].convexity_area.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "ConvCompactness":
                            if (blobs[i].convexity_compactness <= exactMin || blobs[i].convexity_compactness >= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "Convexity":
                            //if (blobs[i].convexity <= exactMin || blobs[i].convexity >= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "ConvPerimeter":
                        if (((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0) <= exactMin || ((blobs[i].convexity_perimetr!= null) ? blobs[i].convexity_perimetr.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Elongation":
                            if (blobs[i].elongation <= exactMin || blobs[i].elongation >= exactMax)
                                al.Add(blobs[i]);
                        break;
                    case "Perimeter":
                        if (((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0) <= exactMin || ((blobs[i].perimetr != null) ? blobs[i].perimetr.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmax":
                        if (((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0) <= exactMin || ((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmin":
                        if (((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0) <= exactMin || ((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    //case "RoughnessBArea":
                    //    if (blobs[i].RoughnessBAreaK <= exactMin || blobs[i].RoughnessBAreaK >= exactMax)
                    //        al.Add(blobs[i]);
                    //    break;
                    case "Roundness":
                            if (blobs[i].roundness <= exactMin || blobs[i].roundness >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "SmallLen":
                        if (((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0) <= exactMin || ((blobs[i].minAxis != null) ? blobs[i].minAxis.value : 0) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                    case "Rmax-Rmin":
                        if ((((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0)-((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0)) <= exactMin || (((blobs[i].Rmax != null) ? blobs[i].Rmax.value : 0)-((blobs[i].Rmin != null) ? blobs[i].Rmin.value : 0)) >= exactMax)
                            al.Add(blobs[i]);
                        break;
                }

            blob_tag[] ret = new blob_tag[al.Count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = (blob_tag)al[i];
            return ret;
        }

        static Bitmap resize(Bitmap  src, Size size, bool isColor )
        {
            Bitmap res = null;

            if (isColor)
            {
                Image<Bgr, Byte> stemp = new Image<Bgr, Byte>(size);
                CvInvoke.Resize(new Image<Bgr, Byte>(src), stemp, size, 0D, 0D, Inter.Nearest);
                res  = stemp.Bitmap;
            }
            else
            {
                Image<Gray, Byte> btemp = new Image<Gray, Byte>(size);
                CvInvoke.Resize(new Image<Gray, Byte>(src ), btemp, size, 0, 0, Inter.Nearest);
                res  = btemp.Bitmap;
            }

            return res;
        }










        //public static blob_tag[] GetFilteredBlobs(ImageProcessor.Reports.MembersMorphoParameters mmp, Bitmap bin)
        //{
        //    float ig_max = (float)(bin.Width * bin.Height * 0.2);
        //    float ig_min = 5;
        //    ImageProcessor3D.MembersScaleFactor msf = ImageProcessor3D.MembersScaleFactor.Deserialization();

        //    blob_tag[] White_blobs = BlobCounter.GetBlobs(bin, true, true, ig_min, ig_max, msf, 1000);


        //    for (int i = 0; i < 16; i++)
        //    {
        //        if (mmp.mp[i].filter)
        //        {
        //            if (mmp.mp[i].InsideOutside)
        //                White_blobs = ImageProcessor.Reports.ReportStandart1.Filter(White_blobs, mmp.mp[i].from, mmp.mp[i].till, mmp.mp[i].name);
        //            else
        //                White_blobs = ImageProcessor.Reports.ReportStandart1.FilterOuter(White_blobs, mmp.mp[i].from, mmp.mp[i].till, mmp.mp[i].name);
        //        }
        //    }
        //    return White_blobs;
        //}

        //public static blob_tag[] GetFilteredBlobs1(ImageProcessor.Reports.MembersMorphoParameters mmp, Bitmap bin, blob_tag[] blbs)
        //{
        //    float ig_max = (float)(bin.Width * bin.Height * 0.2);
        //    float ig_min = 5;
        //    ImageProcessor3D.MembersScaleFactor msf = ImageProcessor3D.MembersScaleFactor.Deserialization();

        //    blob_tag[] White_blobs = null;

        //    if (blbs == null)
        //        White_blobs = BlobCounter.GetBlobs(bin, true, true, ig_min, ig_max, msf, 1000);
        //    else
        //        White_blobs = blbs;

        //    for (int i = 0; i < 16; i++)
        //    {
        //        if (mmp.mp[i].filter)
        //        {
        //            if (mmp.mp[i].InsideOutside)
        //                White_blobs = ImageProcessor.Reports.ReportStandart1.Filter(White_blobs, mmp.mp[i].from, mmp.mp[i].till, mmp.mp[i].name);
        //            else
        //                White_blobs = ImageProcessor.Reports.ReportStandart1.FilterOuter(White_blobs, mmp.mp[i].from, mmp.mp[i].till, mmp.mp[i].name);
        //        }
        //    }
        //    return White_blobs;
        //}

        #endregion

    }
}
