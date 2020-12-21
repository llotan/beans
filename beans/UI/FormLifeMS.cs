using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using beans.methods;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Flokal.Camera;
using Utilities;
using Timer = System.Timers.Timer;
using System.Collections;
using System.IO;
using System.Windows.Forms.VisualStyles;
using beans.UI;
using ImageProcessor;

namespace beans
{
    public partial class FormLife : Form
    {

        #region fields

        private ICamera drv,drvSetCamera;
        private detect_blobs detectorMaster;
        private detect_blobs detectorSlave;
        private List<bool> listMaster;
        private List<bool> listSlave;
        private Bitmap currBitmapMaster, currBitmapSlave;
        private ConfigSetting cfg;
        private ucDetectProcesShow dpsMaster, dpsSlave;
        private int currValve;
        private ucBlobsView bvMaster;
        private ucBlobsView bvSlave;
        private List<long[]> MSLink; // array that link bvMaster.Blobs & bvSlave.Blons
        private comparator linkMS;
        private ucDeviceSetting devSet;
       
        private MimasMeasure.FormMeasure formMeasure;
        private bool isSetProcessCanSave;
        private int cntSetProcess = 0;
        private string pthSetProcess;
        private DateTime dtsetprocess;
        private Image<Gray, byte> startMaskMaster;
        long countFrameOfMaster = 0;
        long countFrameOfSlave = 0;
        long countBadFrameOfMaster = 0;
        long countBadFrameOfSlave = 0;
        private long countSampleMasterGood=0, countSampleMasterBad=0;
        private long countSampleSlaveGood=0, countSampleSlaveBad=0;

        private double sumIntesitiveSampleMasterGood=0, sumIntesitiveSampleMasterBad=0;
        private double sumIntesitiveSampleSlaveGood=0, sumIntesitiveSampleSlaveBad=0;

#if TRACE
        static private int cntBlow = 0;
        static private DataSet ds;
        static private DataTable dtSensor;
        static private DataTable dtBlow;
#endif 

        private static int  cntSensorWorked;
        private static int cntBlowSentTotal;
        
        private bool isDetectItensivity;
        private List<double[]> detectIntensivityList;
        private int detectIntensivityCount;

        #endregion

        #region constructor

        public FormLife()
        {
            InitializeComponent();

            dpsMaster = new ucDetectProcesShow();
            dpsMaster.Dock = DockStyle.Fill;
            scMaster.Panel1.Controls.Add(dpsMaster);

            dpsSlave = new ucDetectProcesShow();
            dpsSlave.Dock = DockStyle.Fill;
            scSlave.Panel1.Controls.Add(dpsSlave);


            bvMaster = new ucBlobsView();
            bvMaster.Dock = DockStyle.Fill;
            scMaster.Panel2.Controls.Add(bvMaster);
            bvMaster.indexChanged += new ucBlobsView.indexChangedHandle( masterListViewIndexChanged);

            bvSlave = new ucBlobsView();
            bvSlave.Dock = DockStyle.Fill;
            scSlave.Panel2.Controls.Add(bvSlave);

//            devSet = new ucDeviceSetting {Dock = DockStyle.Bottom};
//            gbDev.Controls.Add(devSet);

            for (var i = ThresholdType.Binary; i <= (ThresholdType) 7; i++) cbBinMethodMaster.Items.Add(i);

            listMaster = new List<bool>();
            listSlave = new List<bool>();
        }



        #endregion
        
        private void start( bool isCameraStart = true )
        {
            labelMsg.Visible = true;

#if (TRACE)
            ds = new DataSet("trace");
            dtBlow = new DataTable("blow");
            dtBlow.Columns.Add("id", typeof(int));
            dtBlow.Columns.Add("valve", typeof(int));
            dtBlow.Columns.Add("sec", typeof(int));
            dtBlow.Columns.Add("milisec", typeof(int));

            dtSensor = new DataTable("sensor");
            dtSensor.Columns.Add("id", typeof(int));
            dtSensor.Columns.Add("valve", typeof(int));
            dtSensor.Columns.Add("sec", typeof(int));
            dtSensor.Columns.Add("milisec", typeof(int));

            ds.Tables.Add(dtBlow);
            ds.Tables.Add(dtSensor);

//            ds.Relations.Add("relation_id", dtBlow.Columns["id"], dtSensor.Columns["id"]);/**/

#endif

            // detectors
            {

                valves.init();

                if (true)
                {
                    detectorMaster =
                        new detect_blobs(new ColorRange("Red"), (ThresholdType) cbBinMethodMaster.SelectedIndex,
                            (byte) trBinValMaster.Value, 255, 2, true);
                    if (!checkBoxIsUseCore.Checked)
                    {
                        detectorMaster.NewBlobs += new detect_blobs.DetectNewBlobHandle(masterBlobsHaveAdded);
                        emulationZeroSample4Master(); // first sample detected long time -> emulate firs/t sample & delete  it .................

                        if (cfg.isShowFrameRgb) detectorMaster.ShowFrameRgb +=
                            new detect_blobs.ShowImageHandle(masterShowFrameRgb); // slowing - don't use
                        if (cfg.isShowFrameBin) detectorMaster.ShowFrameBin += new detect_blobs.ShowImageHandle(masterShowFrameBin);

                        timerM.Start();
                    }
                    
                }

#if (DEMO)
                cbCamMS.Checked = true;
#endif
                
                if (cbCamMS.Checked)
                {

                    if (MSLink == null) MSLink = new List<long[]>();
                    linkMS = new comparator();
                    linkMS.ComplitParticle += new comparator.ComplitReorddHandle(complectMSCompare);

                    detectorSlave =
                        new detect_blobs(new ColorRange("Red"), (ThresholdType) cbBinMethodMaster.SelectedIndex,
                            (byte) trBinValMaster.Value, 255,  2, false );
                    if (!checkBoxIsUseCore.Checked)
                    {
                        detectorSlave.NewBlobs += new detect_blobs.DetectNewBlobHandle(slaveBlobsHaveAdded);
                        emulationZeroSample4Slave(); // first sample detected long time -> emulate first sample & delete  it .................

                        if (cfg.isShowFrameRgb) detectorSlave.ShowFrameRgb +=
                            new detect_blobs.ShowImageHandle(slaveShowFrameRgb); // slowing - don't use
                        if (cfg.isShowFrameBin) detectorSlave.ShowFrameBin += new detect_blobs.ShowImageHandle(slaveShowFrameBin);

                        timerS.Start();
                    }
                }
            }

            cbBinMethodMaster.SelectedItem = detectorMaster.ThresholdType_METHOD;
            trBinValMaster.Value = detectorMaster.ThresholdType_VAL;

            if (isCameraStart)
            {
#if (DEMO)
                cbCamOne.Checked = true;
#endif
                // camera engine
                if (cbCamOne.Checked)
                {
                    drv = new IDS();
                    drv.Init(((IDS)drv).GetNoInDeviceList(cfg.CameraMaster), -1, -1, 24, dpsMaster.pbLife, IntPtr.Zero);
                }
                else
                {
                    drv = new IDSMS();
                    ((IDSMS)drv).SetCameraID(cfg.CameraMaster,1);
                    drv.Init(0, -1, -1, 24, dpsSlave.pbLife, dpsMaster.pbLife.Handle);
                    ((IDSMS)drv).ImageCapturedSlave += new CameraBridgeEventHandler(cameraSlaveCapture);
                }

                // device
                HidController.Controller.Start(cfg.FrontLight, (int)cfg.BackLigth,
                    cfg.VibroVal, cfg.VibroVal2);
                HidController.Controller.BlowSetTime(cfg.BlowInterval);
                HidController.Controller.SensorsClear();

                drv.TurnON();
                drv.ImageCaptured += new CameraBridgeEventHandler(cameraMasterCapture);

                if (cbCamOne.Checked) valves.init(((IDS) drv).GetWidth(), ((IDS) drv).GetWidth());
                else valves.init(((IDSMS) drv).GetMasterWidth(), ((IDSMS) drv).GetSlaveWidth());

#if (DEMO)
                cbCamMS.Checked = true;
                timerDEMO.Start();
#endif

             
                labelMsg.Visible = false;
                clearCounters();
#if (TRACE)
                dtSensor.Rows.Clear();
                dtBlow.Rows.Clear();
                cntBlow = 0;
#endif
            }


        }

        private void stop()
        {
            if (drv != null)
            {
                drv.TurnOFF();
                drv.Release();
            }

            if (drvSetCamera != null  )
            {
                drvSetCamera.TurnOFF();
                drvSetCamera.Release();
            }

            timerM.Stop();
            timerS.Stop();

            bvMaster.stopFillProcess();
            bvSlave.stopFillProcess();

#if (TRACE)
            HidController.Controller.SensorStatusRead(0, out cntBlowSentTotal, out cntSensorWorked);
            toolStripStatusTrace.Text = $"Blow/Sensor = {cntBlowSentTotal}/{cntSensorWorked}";
#endif

#if (!TEST)
            HidController.Controller.Stop();
#endif
            if (isDetectItensivity)
            {
                try
                {
                    DetectItensivity();
                }
                finally
                {
                    if (detectIntensivityCount > 0)
                    {
                        isDetectItensivity = false;
                    }
                    else
                    {
                        detectIntensivityToolStripMenuItem_Step2();
                    }
                }
            }
        }


        private void cameraCapture(object source, CameraBridgeEventArgs e)
        {
            try
            {

                new Task(() => { cameraMasterCapture(source, e); }).Start();

                new Task(() => { cameraSlaveCapture(source, e); }).Start();

                if (countFrameOfMaster % 10 == 0)
                    toolStripStatusLabelFps.Text = (currBitmapSlave != null)
                        ? $"FPS master:{e.FrameNumber},slave:{e.BPP}"
                        : $"FPS:{e.FrameNumber}";

            }
            catch (Exception exception)
            {
                Flokal.Protocol.Logging.Error("FormLifeMS.cameraCapture(): " + exception.Message);
            }

        }

        private void cameraMasterCapture(object source, CameraBridgeEventArgs e)
        {
            var tick = DateTime.Now;

            Bitmap front = (Bitmap) e.ImageMaster;//Flokal.Common.Methods.Image.Clone((Bitmap)e.ImageMaster);

            if (countFrameOfMaster++ == 0) testIfCameraSetCorrect(front, "Master");

            try
            {
                if (!checkBoxIsUseCore.Checked)
                {
                    detectorMaster.Core(front , tick);
                }

            }
            catch (Exception ex)
            {
//                to-do: must solve problem read data from cameras
//                Flokal.Protocol.Logging.Error("FormLifeMS.cameraMasterCapture(/*async*/): " + ex.Message);
            }
        }

        private void cameraSlaveCapture(object source, CameraBridgeEventArgs e)
        {
            var tick = DateTime.Now;

#if (DEMO)
                e.ImageSlave = methods.common.mirror((Bitmap)e.ImageMaster);
#endif

            Bitmap back = (Bitmap)e.ImageSlave;//Flokal.Common.Methods.Image.Clone( (Bitmap)e.ImageSlave);

            if (countFrameOfSlave++ == 0) testIfCameraSetCorrect(back , "Slave");

            try
            {
                if (cbCamMS.Checked)
                {
                    detectorSlave.Core(back , tick);
                }
            }
            catch (Exception ex)
            {
//                to-do: must solve problem read data from cameras
//                Flokal.Protocol.Logging.Error("FormLifeMS.cameraSlaveCapture(/*async*/): " + ex.Message);
            }

        }

        private void masterBlobsHaveAdded(List<Bitmap> blobs)
        {
            try
             {
#if (DEMO)
                DateTime timeTotal= DateTime.Now;
#endif
//                lock (listMaster)/**/
                {       
                    foreach (var bitmap  in blobs)
                    {
#if (DEMO)
                        Flokal.Protocol.Logging.Trace($"FormLifeMS.masterBlobsHaveAdded(...)", "Clone time", timeTotal );
#endif
                        dtsetprocess = ((blob_tag) bitmap.Tag).time;
                        bvMaster.Add(bitmap);
#if (DEMO)
                        listMaster.Add(((blob_tag) bitmap.Tag).check);
                        DateTime timeValve = DateTime.Now;
#endif
                        ((blob_tag)bitmap.Tag).valve = currValve = valves.calc(bitmap, true);
#if (DEMO)
                        Flokal.Protocol.Logging.Trace($"FormLifeMS.masterBlobsHaveAdded(...)", "Valve time", timeValve);
                        DateTime timeBlow = DateTime.Now;
#endif
                        if ( !isDetectItensivity  && ((blob_tag) bitmap.Tag).check)
                        {
                            ((blob_tag) bitmap.Tag).blowInterval = cfg.Blow;

                            var task = new BackgroundWorker();
                            task.DoWork += new DoWorkEventHandler(doBackgroundBlow);
                            task.RunWorkerAsync(bitmap.Tag);

                            sumIntesitiveSampleMasterBad += ((blob_tag) bitmap.Tag).intensivity;
                            countSampleMasterBad++;

                        }
                        else
                        {
                            sumIntesitiveSampleMasterGood += ((blob_tag)bitmap.Tag).intensivity;
                            countSampleMasterGood++;
                        }
#if (DEMO)
                        Flokal.Protocol.Logging.Trace($"FormLifeMS.masterBlobsHaveAdded(...)", "Blow time", timeBlow );
#endif

                    }
                }
#if (DEMO)
                        Flokal.Protocol.Logging.Trace($"FormLifeMS.masterBlobsHaveAdded(...)","Total time", timeTotal                       );
#endif
            }
            catch (Exception e)                                                                 
            {
                Flokal.Protocol.Logging.Error("FormLifeMS.masterBlobsHaveAdded(...): " + e.Message);
                throw;
            }
        }

        private void slaveBlobsHaveAdded(List<Bitmap> blobs)
        {

            long count = bvSlave.Blobs.Count;

            try
            {
#if (DEMO)
                DateTime timeTotal= DateTime.Now;
#endif
                lock (listSlave)
                {
                    foreach (var bitmap in blobs)
                    {
                        bvSlave.Add(bitmap);
#if (DEMO)
                        listSlave.Add(((blob_tag) bitmap.Tag).check);
#endif
                        ((blob_tag)bitmap.Tag).valve = currValve = valves.calc(bitmap, false);

#if (!DEMO)
                        if (!isDetectItensivity && ((blob_tag) bitmap.Tag).check)
                        {
                            ((blob_tag) bitmap.Tag).blowInterval = cfg.BlowInterval;

                            var task = new BackgroundWorker();
                            task.DoWork += new DoWorkEventHandler(doBackgroundBlow);
                            task.RunWorkerAsync(bitmap.Tag);

                            sumIntesitiveSampleSlaveBad += ((blob_tag)bitmap.Tag).intensivity;
                            countSampleSlaveBad++;
                        }
                        else
                        {
                            sumIntesitiveSampleSlaveGood += ((blob_tag)bitmap.Tag).intensivity;
                            countSampleSlaveGood++;
                        }

#endif
                        //                        if (!timerSYNC_MS.Enabled)
                        //                        {
                        //                            timerSYNC_MS.Start();
                        //                        }

                    }

                }
#if (DEMO)
                        Flokal.Protocol.Logging.Trace($"FormLifeMS.SLAVEBlobsHaveAdded(...)","Total time", timeTotal                       );
#endif
            }
            catch (Exception e)
            {
                Flokal.Protocol.Logging.Error("FormLifeMS.SlaveBlobsHaveAdded(...): " + e.Message);
                throw;
            }
        }


        private void masterShowFrameBin(Bitmap b)
        {
            dpsMaster.pbBin.Image = b;
        }

        private void masterShowFrameRgb(Bitmap b)
        {
//            dpsMaster.pbFrame.Image = b;
            dpsMaster.pbFrame.Image = valves.show(b);
        }

        private void slaveShowFrameBin(Bitmap b)
        {
            dpsSlave.pbBin.Image = b;
            }

        private void slaveShowFrameRgb(Bitmap b)
        {
//            dpsSlave.pbFrame.Image = b; 
            dpsSlave.pbFrame.Image = valves.show(b,false); 
        }

        private void aqastmet()
        {

            if(drv != null) currBitmapMaster = drv.CaptureImage();

            if (checkBoxIsUseCore.Checked && currBitmapMaster != null)
            {
                var front = (Bitmap) currBitmapMaster.Clone(new Rectangle(0, 0, currBitmapMaster.Width, currBitmapMaster.Height), PixelFormat.Format24bppRgb);
                Image<Gray, byte> bin = common.Binaraze(
                    front,
                    detectorMaster.ThresholdType_METHOD, detectorMaster.ThresholdType_VAL, 255);
                dpsMaster.pbBin.Image = bin.Bitmap;

                Image<Bgr, byte> brg = new Image<Bgr, byte>(front);
                dpsMaster.pbFrame.Image = filters.colorRED.DetectColorExist(brg.Copy(bin).Bitmap, cfg).Bitmap;
               
            }
        }

        void clear()
        {
            if (bvMaster.Blobs.Count > 0 && MessageBox.Show("Clear current result of experement?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clearCounters();
                clearExperiment();
                clearUI();
            }
        }

        void clearCounters()
        {
            countFrameOfMaster = 0;
            countFrameOfSlave = 0;
            countBadFrameOfMaster = 0;
            countBadFrameOfSlave = 0;
            countSampleMasterGood = 0;
            countSampleMasterBad = 0;
            countSampleSlaveGood = 0;
            countSampleSlaveBad = 0;

            cntBlowSentTotal = 00;
            cntSensorWorked = 0;

            sumIntesitiveSampleMasterGood = sumIntesitiveSampleMasterBad = 0;
            sumIntesitiveSampleSlaveGood = sumIntesitiveSampleSlaveBad = 0;
        }

        void clearUI()
        {
            listMaster.Clear();
            listSlave.Clear();

            toolStripStatusMaster.Text = "";
            toolStripStatusSlave.Text = "";
            toolStripStatusLabelFps.Text = "";
            toolStripStatusSync.Text = "";
            toolStripStatusTrace.Text = "";  
        }

        private void clearExperiment(bool refresh = true)
        {
            bvMaster?.clear();
            bvSlave?.clear();

            if (refresh)
            {
                refreshExperiment();
            }
        }

        private void refreshExperiment()
        {
            bvMaster.refresh();
            bvSlave.refresh();
        }


        private void complectMSCompare(object sender, RunWorkerCompletedEventArgs e)
        {

            long[] arg = (long[]) e.Result;
            if (arg[1] != -1)
            {
                MSLink.Add(arg);
                MSLink = MSLink.Distinct().ToList();
            }
        }

        private void masterListViewIndexChanged(int i)
        {
            if (MSLink != null && MSLink.Count > i)
            {
                if (MSLink[i][1] != -1)
                {
                    bvSlave.currentIndex = (int)MSLink[i][1];
                }
            }
        }


        private void configUpdate(ConfigSetting configSetting)
        {
            this.cfg = configSetting;
        }

        private void emulationZeroSample4Master()
        {
            List<Bitmap> b = common.generateSample(cfg);

            foreach (var l in b)
            {
                var BridgeBetweenDrvAndApp = new CameraBridgeEventArgs();
                BridgeBetweenDrvAndApp.ImageMaster = l;
                BridgeBetweenDrvAndApp.ImageSlave = null; //LastFrame2;

                cameraMasterCapture(drv, BridgeBetweenDrvAndApp);
            }

            Task.Delay(500).Wait(500);

            bvMaster.clear();
            bvMaster.refresh();
            countSampleMasterBad--;

            HidController.Controller.Blow(0,false );
//            HidController.Controller.SensorsClear();

        }

        private void emulationZeroSample4Slave()
        {
            List<Bitmap> b = common.generateSample(cfg);

            foreach (var l in b)
            {
                var BridgeBetweenDrvAndApp = new CameraBridgeEventArgs();
                BridgeBetweenDrvAndApp.ImageSlave = l;
                cameraSlaveCapture(drv, BridgeBetweenDrvAndApp);
            }

            Task.Delay(500).Wait();

            bvSlave.clear();
            bvSlave.refresh();
            countSampleSlaveBad--;

            HidController.Controller.Blow(0, false);
//            HidController.Controller.SensorsClear();

        }

        private void testIfCameraSetCorrect(Bitmap front, string master)
        {
            if (!common.LineIsEmpty(front, cfg))
            {
                if (MessageBox.Show(master + " camera setted incorrect,\n  frame must be empty \n Interup process?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.stop();
                }
            }

        }

        private void DetectItensivity()
        {
            List<double> intensivity = new List<double>();
            foreach (var item in bvMaster.Blobs) intensivity.Add(((blob_tag)item.Tag).intensivity);
            foreach (var item in bvSlave .Blobs) intensivity.Add(((blob_tag)item.Tag).intensivity);

            detectIntensivityList.Add( new double[] {intensivity.Min(), intensivity.Average(), intensivity.Max()});

            if (detectIntensivityList.Count==2)
            {
                double res = detectIntensivityList[0][2] + (detectIntensivityList[1][0]- detectIntensivityList[0][2]) / 2;
                string msg =
                    $"Itensivity of good sample: min={detectIntensivityList[0][0].ToString("F")}, avr={detectIntensivityList[0][1].ToString("F")}, max={detectIntensivityList[0][2].ToString("F")}\n"
                    +$"Itensivity of bed sample: min={detectIntensivityList[1][0].ToString("F")}, avr={detectIntensivityList[1][1].ToString("F")}, max={detectIntensivityList[1][2].ToString("F")}\n"
                    +$"Recomend value:{(res   .ToString("F"))}, save in config?"
                    +$"if your want save other value, you can do it in [Setting/Variables](filterBAD_Intensivity)";


                if (MessageBox.Show(msg, "Result", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) ==
                    DialogResult.OK)
                {
                    cfg.filterBAD_Intensivity = Convert.ToByte(res);
                    SettingsManager.Instance.SaveCurrent(cfg, Application.StartupPath);
                }

            }
        }


        #region backgroud processes

        private void TimerM_Tick(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusMaster.Text = $"master cnt:{countSampleMasterGood}/{countSampleMasterBad} intesivity:{((int)(sumIntesitiveSampleMasterGood / countSampleMasterGood))}/{((int)(sumIntesitiveSampleMasterBad / countSampleMasterBad))}";
#if DEMO
                lock (listMaster)
                {
                    if (listMaster.Count > 00) panelRed.Visible = false;

                    foreach (var b in listMaster)
                    {
                    }

                    listMaster.Clear();
                }
#endif
            }
            catch (Exception exception)
            {
                Flokal.Protocol.Logging.Error("TimerMaster_Tick(): " + exception.Message);
            }
        }


        private void TimerS_Tick(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusSlave.Text = $"slave cnt:{countSampleSlaveGood}/{countSampleSlaveBad}  intesivity:{((int)(sumIntesitiveSampleSlaveGood / countSampleSlaveGood))}/{((int)(sumIntesitiveSampleSlaveBad / countSampleSlaveBad))}";
#if DEMO
                lock (listSlave)
                {
                    if (listSlave.Count > 00) panelRed.Visible = false;

                    foreach (var b in listSlave)
                    {
                    }

                    listSlave.Clear();
                }
#endif

            }
            catch (Exception exception)
            {
                Flokal.Protocol.Logging.Error("TimerSlave_Tick(): " + exception.Message);
            }
        }


        private void doBackgroundBlow(object sender, DoWorkEventArgs e)
        {
            var arg = (blob_tag) e.Argument;
            var timer = new Timer();
            timer.Interval = arg.blowInterval;
            timer.Elapsed += (sender1, e1) => doBlowTick(sender1, e1, arg.valve, arg.time, arg.blowInterval);
            timer.Enabled = true;
        }

        
        private static void doBlowTick(object sender, ElapsedEventArgs e, int valve, DateTime st, int interval)
        {
            var bs = DateTime.Now;
            var tmp = ((Timer)sender).Interval;

            try
            {
                ((Timer) sender).Interval = 1000;
                ((Timer) sender).Enabled = false;

                HidController.Controller.Blow(valve);

#if (TRACE)
                Console.WriteLine($">  index-{++cntBlow}" + ", valve -" + valve+"                                            "
                                  + ",blob detect at-" + st.Second + ":" + st.Millisecond
                                  + ", blow start at-" + bs.Second + ":" + bs.Millisecond
                                  + ". Delay before blow -" + (bs - st).Milliseconds
                                  + ", total delay -" + (bs - st).Milliseconds
                                  + ", valve -" + valve);


                var task = new BackgroundWorker();
                task.DoWork += new DoWorkEventHandler(doBackgroundSensor);
                task.RunWorkerAsync(new ArrayList(){valve,cntBlow,DateTime.Now.AddMilliseconds(250) });

                dtBlow.Rows.Add(new object[]{cntBlow,valve ,bs.Second ,bs.Millisecond});
#endif
            }
            catch (Exception exception)
            {
                Flokal.Protocol.Logging.Error($"FormLife.doBlowTick(valve={valve}): {exception.Message}");
                throw;
            }
            finally
            {
                ((Timer)sender).Interval=tmp;


            }



        }

        // only for TRACE mode
        private static void doBackgroundSensor(object sender, DoWorkEventArgs e)
        {
            var arg = (ArrayList)e.Argument;
            int valve = (int)arg[0];
            int cnt = (int)arg[1];
            DateTime ti = (DateTime) arg[2];
            

            var timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += (sender1, e1) => doSensorTick(sender1, e1, valve, ti, cnt);
            timer.Enabled = true;
        }

        // only for TRACE mode
        private static void doSensorTick(object sender1, ElapsedEventArgs e1, int valve, DateTime addMilliseconds, int i)
        {
            bool res = false;

            try
            {
                var bs = DateTime.Now;
                bool ceiknot = bs > addMilliseconds;

                if (ceiknot )
                {
                    ((Timer) sender1).Interval = 1000;
                    ((Timer)sender1).Enabled = false;
                }

                    // read  status off sensor
                    res=HidController.Controller.SensorStatusRead(valve,  out cntBlowSentTotal, out cntSensorWorked);


                if (res)
                {
                    ((Timer) sender1).Interval = 1000;
                    ((Timer)sender1).Enabled = false;

                    Console.WriteLine($">> index-{i}" + ", valve -" + valve +
                                      "                                            "
                                      + ", sensor done at-" + bs.Second + ":" + bs.Millisecond);

#if TRACE
                    dtSensor .Rows.Add(new object[] { i, valve, bs.Second, bs.Millisecond });

#endif

                }

            }
            catch (Exception e)
            {
                                Console.WriteLine(e.Message+" "+valve);
            }

            
            
        }

        private void timerSetProcess_Tick(object sender, EventArgs e)
        {
            isSetProcessCanSave = false;
            timerSetProcess.Stop();
        }

        private void timerDEMO_Tick(object sender, EventArgs e)
        {
            if (currBitmapSlave!=null)
            {
                try
                {
                    Bitmap tmp = Flokal.Common.Methods.Image.Clone(currBitmapSlave);//.Clone(new Rectangle(0, 0, currBitmapSlave.Width, currBitmapSlave.Height), PixelFormat.Format24bppRgb);
                    dpsSlave.pbLife.Image = tmp;
                }
                catch (Exception exception)
                {
//                    Flokal.Protocol.Logging.Error(exception.Message);
                }
            }
        }

#endregion

#region form events

        private void FormLife_Load(object sender, EventArgs e)
        {
            cfg = SettingsManager.Instance.GetCurrent(Application.StartupPath);

//            measure.distance.factor = measure.square.factor = cfg.MeasureFactor;

            Text = cfg.ProgramName;

            trBinValMaster.Value = cfg.BinVal;
            cbBinMethodMaster.SelectedIndex = cfg.BinMethod;

            //labelValve.Visible = true;labelValve.Text=(filters.morphoMinEncloseCircle.ComputeDistance(new Point(0, 0), new Point(100, 100)).ToString());panelRed.Visible = true;
        }

        private void FormLife_FormClosing(object sender, FormClosingEventArgs e)
        {
            cfg.BinVal = trBinValMaster.Value;
            cfg.BinMethod = cbBinMethodMaster.SelectedIndex;

            SettingsManager.Instance.SaveCurrent(cfg, Application.StartupPath);

            stop();
        }

        private void CbCamOne_CheckedChanged(object sender, EventArgs e)
        {
            cbCamMS.Checked = !cbCamOne.Checked;
            if (cbCamMS.Checked) checkBoxIsUseCore.Checked = false;
        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormFilterSet())
            {
                form.ShowDialog();
            }
        }

        private void scProcessShow_Resize(object sender, EventArgs e)
        {
            var sc = (SplitContainer) sender;
            sc.SplitterDistance = sc.Width / 2;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormAbout())
            {
                form.ShowDialog();
            }
        }

        private void readExperimentFromFilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var blobs = bvMaster.read();
        }

        
        private void measurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
            // get image
            try
            {
                stop();

                // device
#if (!TEST)
                MessageBox.Show("Pls, put rule in area vision and continue\n(device must be switch on)", "Information",MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                HidController.Controller.Start(cfg.FrontLight, (int) cfg.BackLigth, 0, 0);
                HidController.Controller.BlowSetTime(cfg.BlowInterval);
#endif
                drv = new IDS();
                drv.Init(0, 0, 0, 24, null, IntPtr.Zero);
                drv.TurnON();
                Task.Delay(1000).Wait();
                var fullBmp = drv.CaptureImage(); //new Bitmap(800, 600, PixelFormat.Format24bppRgb);
                formMeasure = new MimasMeasure.FormMeasure(true);
                formMeasure.Finish += formMeasure_Finish;
                formMeasure.Show_(fullBmp);
            }
            finally
            {
                stop();
            }
        }


        private void formMeasure_Finish()
        {
            cfg.MeasureFactor = formMeasure.ScaleFactor;
            cfg.MeasureType = formMeasure.MeasureType;
            SettingsManager.Instance.SaveCurrent(cfg, Application.StartupPath);
            formMeasure.Dispose();
        }

        private void valvesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSetBlow(cfg))
            {
                form.ShowDialog();
                //devSet.trBlow.Value = cfg.Blow;
            }
        }

        private void morphoFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FormMorphoFilter())
            {
                f.Set(new Bitmap(800, 600), new Bitmap(800, 600));
                f.ShowDialog();
            }
        }

        private void standartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reports.CreateStandartReport3(bvMaster.Blobs);
        }

        private void fixedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start(false);

            countFrameOfMaster = countFrameOfSlave = 0;

            List<Bitmap> listOfLines = methods .common.PrepareLinesBitmaps(/*@"..\..\..\..\merge" + inx + ".jpg"*/);
            foreach (var item in listOfLines )
            {

                var arg = new CameraBridgeEventArgs();
                arg.ImageMaster = (Bitmap)item;
                arg.ImageSlave = methods.common.mirror(item);

                cameraCapture(sender , arg);

            }

            stop();

            MessageBox.Show($"master done {countFrameOfMaster} lines, error {countBadFrameOfMaster}\n slave done {countFrameOfSlave} lines, error {countBadFrameOfSlave}");

//            MSCompare();

        }

        private void developeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reports.CreateLocalReport0(bvMaster.Blobs, bvSlave.Blobs, MSLink);
        }

        private void saveExperimentToFilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bvMaster.Save();

        }

        private void devicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormDev())
            {
                form.devSet.DataChanged+=new ucDeviceSetting.DataChangedHandle(configUpdate);
                form.ShowDialog();
            }
        }

  


        private void syncMSCamerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSync())
            {
                form.ShowDialog();
            }
        }


        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormSetting())
            {
                form.uc.DataChanged += new ucSetting.DataChangedHandle(configUpdate);
                form.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearExperiment(true );
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = Flokal.Common.Methods.Files.GetDocumentFolderPath(cfg.ProgramName);
            dlg.Filter = "xml files (*.xml)|*.xml|all files (*.*)|*.*";

            if (dlg.ShowDialog()==DialogResult.OK)
            {
                string mask = Path.GetFileNameWithoutExtension(dlg.FileName).Replace("Master", "").Replace("Slave", "");
                mask = Path.Combine(dlg.InitialDirectory, mask);
                if ( bvMaster!=null && bvMaster.Blobs.Count>0 ) bvMaster.Save(mask+"Master.xml");
                if (bvSlave != null && bvSlave.Blobs.Count > 0) bvSlave.Save(mask + "Slave.xml");
            }

        }

        private void detectIntensivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            detectIntensivityToolStripMenuItem_Step1();
        }

        private void detectIntensivityToolStripMenuItemDo(string msg, string step)
        {
            if (MessageBox.Show(msg, step , MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                isDetectItensivity = true; // switch off into stop()
                Button1_Click(null, null);    //start();
            }
        }

        private void detectIntensivityToolStripMenuItem_Step1()
        {
            string msg = "This procedure autodetect intensivity (for good(lighter) samples).\n"
                         + "Prepare:\n"
                         + "1. Set filter what you need, ex. RED  [Setting/Filters];\n"
                         + "2. Prepare array only good samples & input them to box; \n"
                         + "3. Press buttom [Ok]. \n"
                         + "... will do step 2 ... \n"
                         + "\n"
                         + "Start autodetect?";

            detectIntensivityList = new List<double[]>();

            detectIntensivityCount = 0;

            detectIntensivityToolStripMenuItemDo(msg,"Step 1");
        }

        private void detectIntensivityToolStripMenuItem_Step2()
        {
            string msg = "This procedure autodetect intensivity (for bad(darknees) samples).\n"
                         + "Prepare:\n"
                         + "1. Prepare array onl bad samples & input them to box; \n"
                         + "2. Press buttom [Ok]. \n"
                         + "\n"
                         + "Continue?";

//            button1.Text = "start";

            detectIntensivityCount = 1;


            detectIntensivityToolStripMenuItemDo(msg,"Step 2");
        }


        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Flokal.Common.Methods.Files.GetDocumentFolderPath(cfg.ProgramName);
            dlg.Filter = "xml files (*.xml)|*.xml|all files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string mask = Path.GetFileNameWithoutExtension(dlg.FileName).Replace("Master", "").Replace("Slave", "");
                mask = Path.Combine(dlg.InitialDirectory, mask);
                if (File.Exists(mask + "Master.xml")) bvMaster.read(mask + "Master.xml");
                if (File.Exists(mask + "Slave.xml"))
                {
                    bvSlave.read(mask + "Slave.xml");
                    syncToolStripMenuItem_Click(null, null);
                }
            }

        }

        private void traceToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if TRACE

            if (dtBlow.Rows.Count > 0)
            {

                FormTraceSensor f = new FormTraceSensor(ds);
                f.ShowDialog();
            }


#endif
        }
    

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MSLink == null) MSLink = new List<long[]>();
            if (linkMS == null) linkMS = new comparator();
            linkMS.Compare(bvMaster.Blobs, bvSlave.Blobs, MSLink);
            refreshExperiment();
        }

        private void timerSYNC_MS_Tick(object sender, EventArgs e)
        {
            if (bvMaster.Blobs.Count >= bvSlave.Blobs.Count)
            {
                linkMS.Compare(bvSlave.Blobs.Count-1, bvMaster.Blobs, bvSlave.Blobs, MSLink);
                timerSYNC_MS.Stop();
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("start"))
            {
                clear();
                start();
                button1.Text = "stop";

                toolStripStatusSync.Text = toolStripStatusLabelFps.Text="";

            }
            else
            {
                button1.Text = "start";

                stop();

                toolStripStatusLabelFps.Text = "";
                if (!cbCamOne.Checked)
                {
                    linkMS.Compare(bvMaster.Blobs, bvSlave.Blobs, MSLink, cfg.isUseValvesInSync );
                    refreshExperiment();
                    int cntSync = MSLink.Count;
                    toolStripStatusSync.Text =$"sync:{cntSync}"; 
                }
            }
        }

  

        private void CbBinMethodMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (detectorMaster != null)
            {
                detectorMaster.ThresholdType_METHOD = (ThresholdType)cbBinMethodMaster.SelectedItem;
                aqastmet();
            }
        }

        private void TrBinValMaster_ValueChanged(object sender, EventArgs e)
        {
            if (detectorMaster != null) detectorMaster.ThresholdType_VAL = (byte)trBinValMaster.Value;
            label5.Text = trBinValMaster.Value.ToString();

            aqastmet();
        }



#endregion
    }
}