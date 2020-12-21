using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace beans.methods
{
        public class SvrImgInTreard
        {
            BackgroundWorkParams conteiner;
            BackgroundWorker bw;

            public SvrImgInTreard(Bitmap _bmp, string _pref, int _count)
            {
                conteiner = new BackgroundWorkParams();
                conteiner.param1 = _bmp;
                conteiner.param2 = _pref;
                conteiner.param3 = _count;

                bw = new BackgroundWorker();
                bw.DoWork += bw_dowork;
                bw.RunWorkerCompleted += bw_completed;

            }

            public void Save()
            {
                bw.RunWorkerAsync(conteiner);
            }

            private void bw_dowork(object sender, DoWorkEventArgs e)
            {
                try
                {
                    BackgroundWorkParams param = (BackgroundWorkParams)e.Argument;
                    string numb = param.param3.ToString().PadLeft(4, '0');
                    string fileNM = param.param2 + numb + ".jpg";
                    if( !Directory.Exists( Path.GetDirectoryName(fileNM ) ) )  Directory.CreateDirectory( Path.GetDirectoryName(fileNM )  );
                    Bitmap tmp = Flokal.Common.Methods.Image.Clone(param.param1);
                    tmp.Save( fileNM );
                }
                catch( Exception ex )
                {
                    string error = " SvrImgInTreard.Dowork(). Error in time save temp file - " + ex.Message;
                    Flokal.Protocol.Logging.Error(error);
                    // throw new Exception(error); - no sens
                }
            }

            private void bw_completed(object sender, RunWorkerCompletedEventArgs e)
            {
                // check error, check cancel, then use result
                if (e.Error != null)
                {
                    // handle the error
                }
                else if (e.Cancelled)
                {
                    // handle cancellation
                }
                else
                {
                    // reading e.Result when e.Error != null throws the exception
                    // use it on the UI thread
                }
            }

        }

        struct BackgroundWorkParams
        {
            public Bitmap param1;
            public string param2;
            public int param3;

        }
    

}
