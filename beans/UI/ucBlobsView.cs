using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using Polenter.Serialization;

namespace beans
{
    public partial class ucBlobsView : UserControl
    {

        public delegate void indexChangedHandle(int i);
        public event indexChangedHandle indexChanged;

        public int currentIndex
        {
            set {
                if (listViewBlobs.Items.Count>value)
                {
//                    listViewBlobs.SelectedIndices.Clear();
//                    listViewBlobs.SelectedIndices.Add(value);
                    listViewBlobs.Items[value].Selected = true;
                    listViewBlobs.Items[value].Focused = true;
                    listViewBlobs.Select();
                    listViewBlobs.Invalidate();
                    listViewBlobs.Focus();
                }
            }
        }

        public string Caption
        {
            get => groupBoxBlobsView.Text;
            set => groupBoxBlobsView.Text = value;
        }

        private List<ArrayList> garbageOfBlobs;
        public List<Bitmap> Blobs;
        
        public ucBlobsView()
        {
            InitializeComponent();

            var cfg = SettingsManager.Instance.GetCurrent(Application.StartupPath);

            listViewBlobs.SmallImageList = imageListBlobs;
            listViewBlobs.View = View.SmallIcon;
            listViewBlobs.FullRowSelect = true;
            imageListBlobs.ImageSize = new Size(cfg.BlobViewItemWidth, cfg.BlobViewItemHeight);

            garbageOfBlobs = new List<ArrayList>();
            Blobs = new List<Bitmap>();

        }

        public void startFillProcess()
        {
            timerM.Start();
        }

        public void stopFillProcess()
        {
            timerM.Stop();
        }

        public void Add(Bitmap blob)
        {
            try
            {
                lock (garbageOfBlobs)
                {
                    
                    imageListBlobs.Images.Add(/*((blob_tag) blob.Tag).time.ToShortTimeString(),*/ blob);
       
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = imageListBlobs.Images.Count - 1;
                    item.Text = ((blob_tag) blob.Tag).ToString();

                    garbageOfBlobs.Add( new ArrayList(){item,blob});

                    if (!timerM.Enabled) startFillProcess();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void clear()
        {
            imageListBlobs.Images.Clear();
            listViewBlobs.Clear();
            garbageOfBlobs.Clear();
            Blobs.Clear();
        }




        public void Save(string file_name = "blobs_list.xml")
        {

            List<Blob> temp =    new List<Blob>();
            foreach (var bmp in Blobs)
            {
                temp.Add( new Blob( bmp ));
            }

            
            var serializer = new Polenter.Serialization.SharpSerializer();
            serializer.Serialize(temp, file_name);

        }

        public List <Bitmap>   read(string file_name = "blobs_list.xml")
        {
            var serializer = new Polenter.Serialization.SharpSerializer();
            List<Blob> temp =  (List < Blob >) serializer.Deserialize(file_name);

            foreach (var bmp in temp    )
            {
                Bitmap b = bmp.source;
                bmp.morpho.Image_bgr= new Image<Bgr, byte>(bmp.source);
                b.Tag = bmp.morpho;
                Add(b);
            }

            return new List<Bitmap>();
        }

        private void DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e == null || e.ItemIndex < 0 || e.ItemIndex > ((ListView)sender).Items.Count) return;
            e.DrawBackground();
            Brush br = Brushes.Black;
            Brush brblue = Brushes.Blue;
            string it = ((ListView)sender).Items[e.ItemIndex].ToString().Replace("ListViewItem:", "").Replace("{", "").Replace("}", "").TrimStart();
            int i = it.LastIndexOf("\r\n");
            string begin, end;
            if (i > 0)
            {
                //img
                var img = ((ListView)sender).SmallImageList.Images[((ListView)sender).Items[e.ItemIndex].ImageIndex];
//                e.Graphics.DrawImage(img, new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y,100,100), new Rectangle(0,0, 100,100), GraphicsUnit.Pixel);
                e.Graphics.DrawImage(img, e.Bounds.Location);
                Rectangle Bounds = new Rectangle(new Point(e.Bounds.X + img.Width + 10, e.Bounds.Y), e.Bounds.Size);
                //text
                begin = it.Substring(0, i);
                end = it.Substring(i + 2);
                Size szbegin = TextRenderer.MeasureText(begin, ((ListView)sender).Font);
                szbegin.Width = Bounds.Width;
                Size szend = TextRenderer.MeasureText(end, ((ListView)sender).Font);
                szend.Width = Bounds.Width;
                RectangleF recbegin = new RectangleF(Bounds.Location, szbegin);
                PointF pointend = new PointF(Bounds.X, Bounds.Y + szbegin.Height);
                RectangleF recend = new RectangleF(pointend, szend);
                e.Graphics.DrawString(begin, ((ListView)sender).Font, br, recbegin, StringFormat.GenericDefault);
                e.Graphics.DrawString(end, ((ListView)sender).Font, brblue, recend, StringFormat.GenericDefault);
            }
            else
            {

                e.Graphics.DrawString(it, ((ListView)sender).Font, brblue, e.Bounds, StringFormat.GenericDefault);
            }


        }

        private void TimerM_Tick(object sender, EventArgs e)
        {
            try
            {

                lock (garbageOfBlobs)
                {

                    foreach (ArrayList  item in garbageOfBlobs)
                    {

                        Bitmap blob = (Bitmap) item[1];
                        Blobs.Add(blob.Clone(new Rectangle(0, 0, blob.Width, blob.Height), PixelFormat.Format24bppRgb));
                        Blobs[Blobs.Count - 1].Tag = blob.Tag;

                        ListViewItem viewItem = (ListViewItem) item[0];
                        listViewBlobs.Items.Add(viewItem);
                        listViewBlobs.Items[listViewBlobs.Items.Count - 1].Text  = ((blob_tag) blob.Tag).ToString();  // ((blob_tag) Blobs[listViewBlobs.Items.Count - 1].Tag).ToString();
                        listViewBlobs.FocusedItem = viewItem;

                    }

                    garbageOfBlobs.Clear();

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("TimerM_Tick(): " + exception);
                throw;
            }

        }

        public void refresh()
        {
            for (int i =0; i < Blobs.Count;i++)
            {
                listViewBlobs.Items[i].Text = ((blob_tag) Blobs[i].Tag).ToString();
            }
        }

        

        private void ucBlobsView_Load(object sender, EventArgs e)
        {

        }

        private void listViewBlobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (indexChanged != null)
            {
                if (listViewBlobs.SelectedIndices.Count > 0)
                {
                    indexChanged(listViewBlobs.SelectedIndices[0]);
                }
            }
        }

        private void listViewBlobs_Click(object sender, EventArgs e)
        {
    

        }

        private void listViewBlobs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewBlobs.SelectedIndices.Count > 0)
            {
                using (Form f = new Form())
                {
                    PictureBox p = new PictureBox();
                    p.Dock = DockStyle.Fill;
//                    p.Image = imageListBlobs.Images[listViewBlobs.SelectedIndices[0]];
                    p.Image =((blob_tag)(Blobs[listViewBlobs.SelectedIndices[0]].Tag)).Image_bgr.Bitmap;
                    p.SizeMode = PictureBoxSizeMode.Zoom;

                    f.Controls.Add(p);
                    f.WindowState = FormWindowState.Maximized;
                    f.ShowDialog();
                }
            }
        }
    }

    class Blob
    {

        public Blob() { }

        public Blob(Bitmap bitmap_with_tag)
        {
            morpho = (blob_tag) bitmap_with_tag.Tag;
            source = morpho.Image_bgr.Bitmap;
        }

        [ExcludeFromSerialization]
        public Bitmap source { get; set; }

        [ExcludeFromSerialization]
        public Bitmap bin
        {
            get
            {
                return morpho.Image_bin.Bitmap;
            }
            set
            {
                morpho.Image_bin= new Image<Gray, byte>(value);
            }
        }

        public blob_tag morpho { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("bin")]
        public byte[] binSerialized
        {
            get
            { // serialize
                if (bin  == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    bin .Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    bin  = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        bin = new Bitmap(ms);
                    }
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("source")]
        public byte[] sourceSerialized
        {
            get
            { // serialize
                if (source  == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    source .Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    source  = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        source = new Bitmap(ms);
                    }
                }
            }
        }
    }


}
