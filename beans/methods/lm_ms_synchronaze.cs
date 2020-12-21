using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beans
{

    class comparator
    {
        private blobs_ms_compare b; //rellization;

        public delegate void ComplitReorddHandle(object sender, RunWorkerCompletedEventArgs e);
        public event ComplitReorddHandle ComplitParticle;

        public comparator()
        {
            b = new blobs_ms_compare( SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath));
        }




        public void Compare(long cnt_slave, List<Bitmap> master, List<Bitmap> slave, List<long[]> mslink)
        {
            var bgwMSCompare = new BackgroundWorker();
            bgwMSCompare.DoWork += new DoWorkEventHandler(doMSCompare);

            if (ComplitParticle != null)
                bgwMSCompare.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ComplitParticle);

            var arg = new ArrayList();
            arg.Add(master);
            arg.Add(slave);
            arg.Add(mslink);
            arg.Add(cnt_slave);

            bgwMSCompare.RunWorkerAsync(arg);
        }




   




        public void Compare( List<Bitmap> master, List<Bitmap> slave, List<long[]> mslink, bool isUseValves = false)
        {
            mslink.Clear();
            for (int m = (mslink.Count == 0) ? 0 : (int)mslink[mslink.Count-1][0]; m < master.Count; m++)
            {
                for (int s = (mslink.Count == 0) ? 0 : (int)mslink[mslink.Count - 1][1]; s < slave.Count; s++)
                {
                    if (b.Compare((Bitmap)master[m], (Bitmap)slave[s],isUseValves  ))
                    {
                        mslink.Add(new long[] {m, s});
                        // fill blog_tag.index
                        ((blob_tag)(master[m].Tag)).index = mslink.Count-1;
                        ((blob_tag)(slave[s].Tag)).index = mslink.Count - 1;

                        break;
                    }
                }
            }
        }



        public void Compare(long cnt_slave, ucBlobsView master, ucBlobsView slave, List<long[]> mslink)
        {
            var bgwMSCompare = new BackgroundWorker();
            bgwMSCompare.DoWork += new DoWorkEventHandler(doMSCompareBlobsView);

            if(ComplitParticle !=null)
               bgwMSCompare.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ComplitParticle);

            var arg = new ArrayList();
            arg.Add(master);
            arg.Add(slave);
            arg.Add(mslink);
            arg.Add(cnt_slave);

            bgwMSCompare.RunWorkerAsync(arg);
        }


        private void doMSCompare(object sender, DoWorkEventArgs e)
        {
            const int MASTER = 0;
            const int SLAVE = 1;

            var res = new long[] { -1, -1 };
            var arg = (ArrayList)e.Argument;

            var master = (List<Bitmap>)arg[MASTER];
            var slave = (List<Bitmap>)arg[SLAVE];
            var link = (List<long[]>)arg[2];
            var cnt = (long)arg[3];

            var LAST = link.Count - 1;


            if (master.Count >= cnt)
            {

                res[MASTER] = cnt;

                for (var index = link.Count > 0 ? link[LAST][SLAVE] : 0; index < slave.Count; index++)
                    if (b.Compare((Bitmap)master[(int)res[MASTER]], (Bitmap)slave[(int)index]))
                    {
                        res[SLAVE] = index;

                        // fill blog_tag.index
                        ((blob_tag)(master[(int)res[MASTER]].Tag)).index = res[MASTER];
                        ((blob_tag)(slave[(int)res[SLAVE]].Tag)).index = res[SLAVE];


                        break;
                    }
            }

            e.Result = res;
        }

        private void doMSCompareBlobsView(object sender, DoWorkEventArgs e)
        {
            const int MASTER = 0;
            const int SLAVE = 1;

            var res = new long[] { -1, -1 };
            var arg = (ArrayList)e.Argument;

            var master = (ucBlobsView)arg[MASTER];
            var slave = (ucBlobsView)arg[SLAVE];
            var link = (List<long[]>)arg[2];
            var cnt = (long)arg[3];

            var LAST = link.Count - 1;


            if (master.Blobs.Count > cnt)
            {

                res[MASTER] = cnt;

                for (var index = link.Count > 0 ? link[LAST][SLAVE] : 0; index < slave.Blobs.Count; index++)
                    if (b.Compare((Bitmap)master.Blobs[(int)res[MASTER]], (Bitmap)slave.Blobs[(int)index]))
                    {
                        res[SLAVE] = index;

                        // fill blog_tag.index
                        ((blob_tag)(master.Blobs[(int)res[MASTER]].Tag)).index=res[MASTER];
                        ((blob_tag)(slave.Blobs[(int)res[SLAVE]].Tag)).index=res[SLAVE];

                        break;
                    }
            }

            e.Result = res;
        }

    }

    class blobs_ms_compare
    {
        private ConfigSetting cfg;
        private int widthOfCamera;

        public blobs_ms_compare()
        {
            cfg = SettingsManager.Instance.GetCurrent(System.Windows.Forms.Application.StartupPath);
            widthOfCamera = cfg.widthOfCameraView;
        }


        public blobs_ms_compare(ConfigSetting _cfg)
        {
            cfg = _cfg;
            widthOfCamera = cfg.widthOfCameraView;
        }

        /// <summary>
        /// run once intime aqustment device, it detect  depends between master and slave cameras.
        /// process: pitch down ONLY ONE sample, process it detect_blobs & run this  method
        /// </summary>
        /// <param name="master">from master camera, master.tag have obj type of blob_tag</param>
        /// <param name="slave">from slave camera, slave.tag have obj type of blob_tag</param>
        /// <returns></returns>
        public Point Aqustment(Bitmap master, Bitmap slave,int width)
        {
            var res = new Point();

            cfg.widthOfCameraView = widthOfCamera = width;

            cfg.XShiftMS = res.X = calcSynchronDeltaX(master, slave);
            cfg.YShiftMS = res.Y = calcSynchronDeltaY(master, slave);

            SettingsManager.Instance.SaveCurrent(cfg, System.Windows.Forms.Application.StartupPath);
            return res;
        }


        /// <summary>
        /// detect if 2 blobs from different cameras is one sample
        /// </summary>
        /// <param name="master">from master camera, master.tag have obj type of blob_tag</param>
        /// <param name="slave">from slave camera, slave.tag have obj type of blob_tag</param>
        /// <returns></returns>
        public bool Compare(Bitmap master, Bitmap slave, bool isUseValve = true )
        {
            bool res = false;
            var deltaT = Math.Abs(calcDeltaTime(master, slave));

            if (isUseValve)
            {
                bool span = (deltaT <= cfg.msDelta);
                int valve = Math.Abs(((blob_tag)master.Tag).valve - ((blob_tag)slave.Tag).valve);
                res = (valve == 0 && span);

                if (!res && (valve == 1 && span))
                {
                    res = Compare(master, slave, false);
                }
            }
            else
            {
//              Flokal.Protocol.Logging.Trace("============== Compare: begin ****************************");
//              Flokal.Protocol.Logging.Trace($"master: {(blob_tag)master.Tag}");
//              Flokal.Protocol.Logging.Trace($"slave : {(blob_tag)slave.Tag}");

                var deltaX = Math.Abs(calcXslaveAproximate(master) - ((blob_tag) slave.Tag).center.X);
                var deltaY = Math.Abs(calcYslaveAproximate(master) - ((blob_tag) slave.Tag).center.Y);

                res = (deltaX < cfg.XDelta) && (deltaY < cfg.YDelta) && (deltaT <= cfg.msDelta);

//              Flokal.Protocol.Logging.Trace($"deltaX : {deltaX}");
//              Flokal.Protocol.Logging.Trace($"deltaY : {deltaY}");
//              Flokal.Protocol.Logging.Trace($"deltaT : {deltaT}");
//              Flokal.Protocol.Logging.Trace($"result : {res }");
//
//              Flokal.Protocol.Logging.Trace("======================= end ****************************");
//              Flokal.Protocol.Logging.Trace("");

            }



            return res;
        }

        private int calcXslaveAproximate(Bitmap master)
        {
            return widthOfCamera - ((blob_tag)master.Tag).center.X + cfg.XShiftMS;
        }


        private int calcYslaveAproximate(Bitmap master)
        {
            return ((blob_tag)master.Tag).center.Y + cfg.YShiftMS;
        }

        private int calcSynchronDeltaX(Bitmap master, Bitmap slave )
        {
            var Xm = (((blob_tag)master.Tag).center.X < widthOfCamera / 2)
                ? widthOfCamera / 2 - ((blob_tag)master.Tag).center.X
                : ((blob_tag)master.Tag).center.X - widthOfCamera / 2;

            var Xs = (((blob_tag)slave.Tag).center.X < widthOfCamera / 2)
                ? widthOfCamera / 2 - ((blob_tag)slave.Tag).center.X
                : ((blob_tag)slave.Tag).center.X - widthOfCamera / 2;

            return Xm - Xs;
        }

        private int calcSynchronDeltaY(Bitmap master, Bitmap slave)
        {
            return ((blob_tag)master.Tag).center.Y - ((blob_tag)slave.Tag).center.Y;
        }


        private int calcDeltaTime(Bitmap master, Bitmap slave)
        {
            /*int res = 999;
            if (((blob_tag)master.Tag).time.Second - ((blob_tag)slave.Tag).time.Second==0)
            {
                res=((blob_tag)master.Tag).time.Millisecond - ((blob_tag)slave.Tag).time.Millisecond;
            }

            if (Math.Abs(((blob_tag) master.Tag).time.Second - ((blob_tag) slave.Tag).time.Second) == 1)
            {
                int t1 = 0, t2 = 0;
                if (((blob_tag)master.Tag).time.Millisecond>>>>>>>>)
                {
                    
                }
            }*/
            var s =   (((blob_tag)master.Tag).time - ((blob_tag)slave.Tag).time);
            return s.Minutes * 60 * 1000 + s.Seconds * 1000 + s.Milliseconds;

        }
    }
}
