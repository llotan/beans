using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
//using ImageProcessor;
//using ImageProcessor.Reports;
//using ImageProcessor3D;

namespace ImageProcessor
{
    public partial class FormMorphoFilter : Form
    {
        public delegate void MMPSavedMeHandler(MembersMorphoParameters mmp);
        public event MMPSavedMeHandler MMPSaved;
        private ToolTip toolTip1 = new ToolTip();
        private MembersMorphoParameters mmp = new MembersMorphoParameters();
        Bitmap bmp = null;
        Bitmap origin = null;
        private void SetToolTip()
        {
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(pbPerimeter,        "Perimeter of a particle");
            toolTip1.SetToolTip(pbArea,             "Area of a particle");
            toolTip1.SetToolTip(pbRoundness,        "Roundness - locates the centre of gravity and builds the biggest inner and the smallest outer circle around it. The value of roundness is the relation between the inner and the outer circle. The roundness of a circle is 100%, all other forms <100%.");
            toolTip1.SetToolTip(pbElongation,       "Elongation - locates the centre of gravity, builds the longest axis in the particle and the axis orthogonal through the centre. The value of elongation is 100% minus the relation between the longest and the orthogonal axis. The elongation of a circle is 0, all other forms are >0 and <100%.");
            toolTip1.SetToolTip(pbRmin,             "R min - Radius of smallest outer circle");
            toolTip1.SetToolTip(pbRmax,             "R max - Radius of biggest inner circle");
            toolTip1.SetToolTip(pbRmaxRmin,         "R max - R min");
            toolTip1.SetToolTip(pbCompactness,      "Compactness - is calculated as the relation between the objects area and its perimeter ([4*Π*S]/Π2). A perfect circle has a compactness of 100%.");
            toolTip1.SetToolTip(pbConvexity,        "Convexity - builds a rubber band around the particle and determines the inner area. The value of convexity is the relation between the area of the particle and the built rubber band area. It is a characteristic number for all indentations of a particle.");
            toolTip1.SetToolTip(pbConvPerimeter,    "Conv Perimeter - is a perimeter of a rubber band around the particle");
            toolTip1.SetToolTip(pbConvArea,         "Conv Area - is an area of a rubber band around the particle");
            toolTip1.SetToolTip(pbConvCompactness,  "Conv Compactness - is calculated as the relation between the area of a rubber band around the particle and its perimeter ([4*Π*S]/Π2). A perfect circle has a compactness of 100%.");
            toolTip1.SetToolTip(pbRoughness,        "Roughness - is the relation of the areas which can be built by circuits of 80% of sieve diameter and the built area. It is a characteristic number for all domes sticked out of a particle.");
            toolTip1.SetToolTip(pbAngle,            "Orientation - is angle of gradient of big axis ");
            toolTip1.SetToolTip(pbBigLen,           "Size of big axis");
            toolTip1.SetToolTip(pbSmallLen,         "Size of small axis");
        }

        public FormMorphoFilter()
        {
            InitializeComponent();
            this.Enabled = false;
            mmp = MembersMorphoParameters.Deserialization();
        }
        public void Set(Bitmap _bmp, Bitmap _origin)
        {
            flag = true;
            mmp = MembersMorphoParameters.Deserialization();
            //System.Threading.Thread.Sleep(1000);
            this.Enabled = true;
            SetTextBoxes();
            SetCheckBoxes();
            SetToolTip();
            bmp = _bmp.Clone(new Rectangle(0, 0, _bmp.Width, _bmp.Height), PixelFormat.Format24bppRgb);//ImageProcessor.Image.Clone(_bmp);
            //bmp.Save(@"d:\qwerty.bmp");
            origin = _origin.Clone(new Rectangle(0, 0, _origin.Width, _origin.Height), PixelFormat.Format24bppRgb);//ImageProcessor.Image.Clone(_origin);
            //bmp.Save(@"d:\origin.bmp");
            flag = false;
            SetColor();
            //this.ShowDialog();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            SetFromCheckBoxes();
            SetFromTextBoxes();
            MembersMorphoParameters.Serialization(mmp);
            if (MMPSaved != null)
                MMPSaved(mmp);
            this.Close();
        }

        private void SetColor()
        {
            if (cbFPerimeter.Checked)
                cbFPerimeter.BackColor = Color.Maroon;
            else
                cbFPerimeter.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox5.Checked)
                checkBox5.BackColor = Color.Maroon;
            else
                checkBox5.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox8.Checked)
                checkBox8.BackColor = Color.Maroon;
            else
                checkBox8.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox11.Checked)
                checkBox11.BackColor = Color.Maroon;
            else
                checkBox11.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox14.Checked)
                checkBox14.BackColor = Color.Maroon;
            else
                checkBox14.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox17.Checked)
                checkBox17.BackColor = Color.Maroon;
            else
                checkBox17.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox20.Checked)
                checkBox20.BackColor = Color.Maroon;
            else
                checkBox20.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox23.Checked)
                checkBox23.BackColor = Color.Maroon;
            else
                checkBox23.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox47.Checked)
                checkBox47.BackColor = Color.Maroon;
            else
                checkBox47.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox44.Checked)
                checkBox44.BackColor = Color.Maroon;
            else
                checkBox44.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox41.Checked)
                checkBox41.BackColor = Color.Maroon;
            else
                checkBox41.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox38.Checked)
                checkBox38.BackColor = Color.Maroon;
            else
                checkBox38.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox35.Checked)
                checkBox35.BackColor = Color.Maroon;
            else
                checkBox35.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox32.Checked)
                checkBox32.BackColor = Color.Maroon;
            else
                checkBox32.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox29.Checked)
                checkBox29.BackColor = Color.Maroon;
            else
                checkBox29.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox26.Checked)
                checkBox26.BackColor = Color.Maroon;
            else
                checkBox26.BackColor = SystemColors.InactiveCaptionText;



            if (checkBox3.Checked)
                checkBox3.BackColor = Color.Navy;
            else
                checkBox3.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox4.Checked)
                checkBox4.BackColor = Color.Navy;
            else
                checkBox4.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox7.Checked)
                checkBox7.BackColor = Color.Navy;
            else
                checkBox7.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox10.Checked)
                checkBox10.BackColor = Color.Navy;
            else
                checkBox10.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox13.Checked)
                checkBox13.BackColor = Color.Navy;
            else
                checkBox13.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox25.Checked)
                checkBox25.BackColor = Color.Navy;
            else
                checkBox25.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox16.Checked)
                checkBox16.BackColor = Color.Navy;
            else
                checkBox16.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox19.Checked)
                checkBox19.BackColor = Color.Navy;
            else
                checkBox19.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox22.Checked)
                checkBox22.BackColor = Color.Navy;
            else
                checkBox22.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox46.Checked)
                checkBox46.BackColor = Color.Navy;
            else
                checkBox46.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox43.Checked)
                checkBox43.BackColor = Color.Navy;
            else
                checkBox43.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox40.Checked)
                checkBox40.BackColor = Color.Navy;
            else
                checkBox40.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox37.Checked)
                checkBox37.BackColor = Color.Navy;
            else
                checkBox37.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox34.Checked)
                checkBox34.BackColor = Color.Navy;
            else
                checkBox34.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox31.Checked)
                checkBox31.BackColor = Color.Navy;
            else
                checkBox31.BackColor = SystemColors.InactiveCaptionText;

            if (checkBox28.Checked)
                checkBox28.BackColor = Color.Navy;
            else
                checkBox28.BackColor = SystemColors.InactiveCaptionText;




        }

        private void SetCheckBoxes()
        {
            cbFPerimeter.Checked = mmp.mp[0].filter;
            checkBox5.Checked = mmp.mp[1].filter;
            checkBox8.Checked = mmp.mp[2].filter;
            checkBox11.Checked = mmp.mp[3].filter;
            checkBox14.Checked = mmp.mp[4].filter;
            checkBox17.Checked = mmp.mp[5].filter;
            checkBox20.Checked = mmp.mp[6].filter;
            checkBox23.Checked = mmp.mp[7].filter;
            checkBox47.Checked = mmp.mp[8].filter;
            checkBox44.Checked = mmp.mp[9].filter;
            checkBox41.Checked = mmp.mp[10].filter;
            checkBox38.Checked = mmp.mp[11].filter;
            checkBox35.Checked = mmp.mp[12].filter;
            checkBox32.Checked = mmp.mp[13].filter;
            checkBox29.Checked = mmp.mp[14].filter;
            checkBox26.Checked = mmp.mp[15].filter;

            checkBox3.Checked = mmp.mp[0].ToReport;
            checkBox4.Checked = mmp.mp[1].ToReport;
            checkBox7.Checked = mmp.mp[2].ToReport;
            checkBox10.Checked = mmp.mp[3].ToReport;
            checkBox13.Checked = mmp.mp[4].ToReport;
            checkBox16.Checked = mmp.mp[5].ToReport;
            checkBox19.Checked = mmp.mp[6].ToReport;
            checkBox22.Checked = mmp.mp[7].ToReport;
            checkBox46.Checked = mmp.mp[8].ToReport;
            checkBox43.Checked = mmp.mp[9].ToReport;
            checkBox40.Checked = mmp.mp[10].ToReport;
            checkBox37.Checked = mmp.mp[11].ToReport;
            checkBox34.Checked = mmp.mp[12].ToReport;
            checkBox31.Checked = mmp.mp[13].ToReport;
            checkBox28.Checked = mmp.mp[14].ToReport;
            checkBox25.Checked = mmp.mp[15].ToReport;

            checkBox1.Checked = mmp.mp[0].InsideOutside;
            checkBox6.Checked = mmp.mp[1].InsideOutside;
            checkBox9.Checked = mmp.mp[2].InsideOutside;
            checkBox12.Checked = mmp.mp[3].InsideOutside;
            checkBox15.Checked = mmp.mp[4].InsideOutside;
            checkBox18.Checked = mmp.mp[5].InsideOutside;
            checkBox21.Checked = mmp.mp[6].InsideOutside;
            checkBox24.Checked = mmp.mp[7].InsideOutside;
            checkBox48.Checked = mmp.mp[8].InsideOutside;
            checkBox45.Checked = mmp.mp[9].InsideOutside;
            checkBox42.Checked = mmp.mp[10].InsideOutside;
            checkBox39.Checked = mmp.mp[11].InsideOutside;
            checkBox36.Checked = mmp.mp[12].InsideOutside;
            checkBox33.Checked = mmp.mp[13].InsideOutside;
            checkBox30.Checked = mmp.mp[14].InsideOutside;
            checkBox27.Checked = mmp.mp[15].InsideOutside;

        }

        private void SetFromCheckBoxes()
        {
            mmp.mp[0].filter = cbFPerimeter.Checked;
            mmp.mp[1].filter = checkBox5.Checked;
            mmp.mp[2].filter = checkBox8.Checked;
            mmp.mp[3].filter = checkBox11.Checked;
            mmp.mp[4].filter = checkBox14.Checked;
            mmp.mp[5].filter = checkBox17.Checked;
            mmp.mp[6].filter = checkBox20.Checked;
            mmp.mp[7].filter = checkBox23.Checked;
            mmp.mp[8].filter = checkBox47.Checked;
            mmp.mp[9].filter = checkBox44.Checked;
            mmp.mp[10].filter = checkBox41.Checked;
            mmp.mp[11].filter = checkBox38.Checked;
            mmp.mp[12].filter = checkBox35.Checked;
            mmp.mp[13].filter = checkBox32.Checked;
            mmp.mp[14].filter = checkBox29.Checked;
            mmp.mp[15].filter = checkBox26.Checked;

            mmp.mp[0].ToReport = checkBox3.Checked;
            mmp.mp[1].ToReport = checkBox4.Checked;
            mmp.mp[2].ToReport = checkBox7.Checked;
            mmp.mp[3].ToReport = checkBox10.Checked;
            mmp.mp[4].ToReport = checkBox13.Checked;
            mmp.mp[5].ToReport = checkBox16.Checked;
            mmp.mp[6].ToReport = checkBox19.Checked;
            mmp.mp[7].ToReport = checkBox22.Checked;
            mmp.mp[8].ToReport = checkBox46.Checked;
            mmp.mp[9].ToReport = checkBox43.Checked;
            mmp.mp[10].ToReport = checkBox40.Checked;
            mmp.mp[11].ToReport = checkBox37.Checked;
            mmp.mp[12].ToReport = checkBox34.Checked;
            mmp.mp[13].ToReport = checkBox31.Checked;
            mmp.mp[14].ToReport = checkBox28.Checked;
            mmp.mp[15].ToReport = checkBox25.Checked;

            mmp.mp[0].InsideOutside = checkBox1.Checked;
            mmp.mp[1].InsideOutside = checkBox6.Checked;
            mmp.mp[2].InsideOutside = checkBox9.Checked;
            mmp.mp[3].InsideOutside = checkBox12.Checked;
            mmp.mp[4].InsideOutside = checkBox15.Checked;
            mmp.mp[5].InsideOutside = checkBox18.Checked;
            mmp.mp[6].InsideOutside = checkBox21.Checked;
            mmp.mp[7].InsideOutside = checkBox24.Checked;
            mmp.mp[8].InsideOutside = checkBox48.Checked;
            mmp.mp[9].InsideOutside = checkBox45.Checked;
            mmp.mp[10].InsideOutside = checkBox42.Checked;
            mmp.mp[11].InsideOutside = checkBox39.Checked;
            mmp.mp[12].InsideOutside = checkBox36.Checked;
            mmp.mp[13].InsideOutside = checkBox33.Checked;
            mmp.mp[14].InsideOutside = checkBox30.Checked;
            mmp.mp[15].InsideOutside = checkBox27.Checked;


        }

        private void SetTextBoxes()
        {
            textBox1.Text = mmp.mp[0].from.ToString("F3");
            textBox4.Text = mmp.mp[1].from.ToString("F3");
            textBox6.Text = mmp.mp[2].from.ToString("F3");
            textBox8.Text = mmp.mp[3].from.ToString("F3");
            textBox10.Text = mmp.mp[4].from.ToString("F3");
            textBox12.Text = mmp.mp[5].from.ToString("F3");
            textBox14.Text = mmp.mp[6].from.ToString("F3");
            textBox16.Text = mmp.mp[7].from.ToString("F3");
            textBox32.Text = mmp.mp[8].from.ToString("F3");
            textBox30.Text = mmp.mp[9].from.ToString("F3");
            textBox28.Text = mmp.mp[10].from.ToString("F3");
            textBox26.Text = mmp.mp[11].from.ToString("F3");
            textBox24.Text = mmp.mp[12].from.ToString("F3");
            textBox22.Text = mmp.mp[13].from.ToString("F3");
            textBox20.Text = mmp.mp[14].from.ToString("F3");
            textBox18.Text = mmp.mp[15].from.ToString("F3");

            textBox2.Text = mmp.mp[0].till.ToString("F3");
            textBox3.Text = mmp.mp[1].till.ToString("F3");
            textBox5.Text = mmp.mp[2].till.ToString("F3");
            textBox7.Text = mmp.mp[3].till.ToString("F3");
            textBox9.Text = mmp.mp[4].till.ToString("F3");
            textBox11.Text = mmp.mp[5].till.ToString("F3");
            textBox13.Text = mmp.mp[6].till.ToString("F3");
            textBox15.Text = mmp.mp[7].till.ToString("F3");
            textBox31.Text = mmp.mp[8].till.ToString("F3");
            textBox29.Text = mmp.mp[9].till.ToString("F3");
            textBox27.Text = mmp.mp[10].till.ToString("F3");
            textBox25.Text = mmp.mp[11].till.ToString("F3");
            textBox23.Text = mmp.mp[12].till.ToString("F3");
            textBox21.Text = mmp.mp[13].till.ToString("F3");
            textBox19.Text = mmp.mp[14].till.ToString("F3");
            textBox17.Text = mmp.mp[15].till.ToString("F3");
        }

        private void SetFromTextBoxes()
        {
            try
            {
                mmp.mp[0].from = Convert.ToDouble(textBox1.Text);
                mmp.mp[1].from = Convert.ToDouble(textBox4.Text);
                mmp.mp[2].from = Convert.ToDouble(textBox6.Text);
                mmp.mp[3].from = Convert.ToDouble(textBox8.Text);
                mmp.mp[4].from = Convert.ToDouble(textBox10.Text);
                mmp.mp[5].from = Convert.ToDouble(textBox12.Text);
                mmp.mp[6].from = Convert.ToDouble(textBox14.Text);
                mmp.mp[7].from = Convert.ToDouble(textBox16.Text);
                mmp.mp[8].from = Convert.ToDouble(textBox32.Text);
                mmp.mp[9].from = Convert.ToDouble(textBox30.Text);
                mmp.mp[10].from = Convert.ToDouble(textBox28.Text);
                mmp.mp[11].from = Convert.ToDouble(textBox26.Text);
                mmp.mp[12].from = Convert.ToDouble(textBox24.Text);
                mmp.mp[13].from = Convert.ToDouble(textBox22.Text);
                mmp.mp[14].from = Convert.ToDouble(textBox20.Text);
                mmp.mp[15].from = Convert.ToDouble(textBox18.Text);

                mmp.mp[0].till = Convert.ToDouble(textBox2.Text);
                mmp.mp[1].till = Convert.ToDouble(textBox3.Text);
                mmp.mp[2].till = Convert.ToDouble(textBox5.Text);
                mmp.mp[3].till = Convert.ToDouble(textBox7.Text);
                mmp.mp[4].till = Convert.ToDouble(textBox9.Text);
                mmp.mp[5].till = Convert.ToDouble(textBox11.Text);
                mmp.mp[6].till = Convert.ToDouble(textBox13.Text);
                mmp.mp[7].till = Convert.ToDouble(textBox15.Text);
                mmp.mp[8].till = Convert.ToDouble(textBox31.Text);
                mmp.mp[9].till = Convert.ToDouble(textBox29.Text);
                mmp.mp[10].till = Convert.ToDouble(textBox27.Text);
                mmp.mp[11].till = Convert.ToDouble(textBox25.Text);
                mmp.mp[12].till = Convert.ToDouble(textBox23.Text);
                mmp.mp[13].till = Convert.ToDouble(textBox21.Text);
                mmp.mp[14].till = Convert.ToDouble(textBox19.Text);
                mmp.mp[15].till = Convert.ToDouble(textBox17.Text);
            }
            catch
            {
                MessageBox.Show("wrong value");
            }
        }
        private bool flag = false;
        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (flag) return;
            SetFromCheckBoxes();
            SetColor();
            //ImageProcessor.Reports.MembersMorphoParameters.Serialization(mmp);
            //mmp = ImageProcessor.Reports.MembersMorphoParameters.Deserialization();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SetFromTextBoxes();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox1_Leave(this, null);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            flag = true;
            mmp.mp[0].filter = false;
            mmp.mp[1].filter = false;
            mmp.mp[2].filter = false;
            mmp.mp[3].filter = false;
            mmp.mp[4].filter = false;
            mmp.mp[5].filter = false;
            mmp.mp[6].filter = false;
            mmp.mp[7].filter = false;
            mmp.mp[8].filter = false;
            mmp.mp[9].filter = false;
            mmp.mp[10].filter = false;
            mmp.mp[11].filter = false;
            mmp.mp[12].filter = false;
            mmp.mp[13].filter = false;
            mmp.mp[14].filter = false;
            mmp.mp[15].filter = false;

            SetCheckBoxes();
            flag = false;
            SetColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Blob[] Black_blobs = ImageProcessor.Reports.ReportStandart1.GetFilteredBlobs(mmp, bmp);
            //Bitmap bitmap = ImageProcessor.Image.Clone(bmp);
            //ImageProcessor.Blobs.BlobsBuild.PutBlobsBlackWhite2(bitmap, Black_blobs, 1);
            //FormBitmapWithBlobs f = new FormBitmapWithBlobs();
            //f._Show(bitmap);
        }

        private void btReport_Click(object sender, EventArgs e)
        {
            //GC.Collect();
            //ImageProcessor.Reports.ReportStandart1 report = new ReportStandart1();
            //Blob[] Black_blobs = ReportStandart1.GetFilteredBlobs(mmp, bmp);
            //if (mmp.filter)
            //    ImageProcessor.Blobs.BlobsBuild.PutBlobsBlackWhite2(bmp, Black_blobs, 1);
            //report.CreateStandartReport2(origin, bmp, Black_blobs, mmp);
        }

        private void FormMorphoFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MMPSaved != null)
                MMPSaved(mmp);
        }

    }

    [Serializable()]
    public class MorphParameter
    {
        public string name = "";
        public double from = 0.0;
        public double till = 0.0;
        public bool InsideOutside = true;
        public bool filter = false;
        public bool ToReport = false;
    }
    [Serializable()]
    public class MembersMorphoParameters
    {
        public MorphParameter[] mp;
        public bool filter = false;
        public int countToReport = 0;

        private string[] item = new string[]
        {
            "Perimeter",
            "Area",
            "Roundness",
            "Elongation",
            "Rmin",
            "Rmax",
            "Rmax-Rmin",
            "Compactness",
            "Convexity",
            "ConvPerimeter",
            "ConvArea",
            "ConvCompactness",
            "RoughnessBArea",
            "Angle",
            "BigLen",
            "SmallLen"
        };
        public MembersMorphoParameters()
        {
            mp = new MorphParameter[16];
            for (int i = 0; i < 16; i++)
            {
                mp[i] = new MorphParameter();
                mp[i].name = item[i];
            }
        }

        public static void Serialization(MembersMorphoParameters mmp)
        {
            mmp.filter = false;
            mmp.countToReport = 0;
            for (int i = 0; i < 16; i++)
            {
                if (mmp.mp[i].filter && !mmp.filter)
                    mmp.filter = true;
                if (mmp.mp[i].ToReport)
                    mmp.countToReport++;
            }
            Stream stream = File.Open(Application.StartupPath + @"\mmp.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, mmp);
            stream.Close();
        }
        public static MembersMorphoParameters Deserialization()
        {
            MembersMorphoParameters ret;
            Stream stream;
            try
            {
                stream = File.Open(Application.StartupPath + @"\mmp.dat", FileMode.Open);
            }
            catch
            {
                ret = new MembersMorphoParameters();
                return ret;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                ret = (MembersMorphoParameters)formatter.Deserialize(stream);
            }
            catch
            {
                ret = new MembersMorphoParameters();
                stream.Close();
            }
            stream.Close();
            //ret.filter = false;
            //for (int i = 0; i < 16; i++)
            //{
            //    if (ret.mp[i].filter)
            //    {
            //        ret.filter = true;
            //        break;
            //    }
            //}
            return ret;

        }
    }

}
