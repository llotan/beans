using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
{
    public partial class FormFixImg : Form
    {
        public FormFixImg()
        {
            InitializeComponent();
        }

        public FormFixImg( Bitmap srv, List<Bitmap> blobs)
        {
            int i = 0;
            InitializeComponent();
            pictureBox1.Image = srv;
            listView1.SmallImageList = imageList1;
            listView1.View = View.SmallIcon;
            imageList1.ImageSize=new Size(64,64);
            foreach (Bitmap  bitmap in blobs)
            {
                imageList1.Images.Add(((blob_tag) bitmap.Tag).time.ToShortTimeString(), bitmap);

                ListViewItem item = new ListViewItem();
                item.ImageIndex = i++;
                item.Text = ((blob_tag)bitmap.Tag).time.ToLongTimeString() + " "+ ((blob_tag) bitmap.Tag).time.Millisecond+"\n"+ ((blob_tag)bitmap.Tag).center+"\n" + ((blob_tag)bitmap.Tag).check;
                            listView1.Items.Add(item);
            }
            
        }

    }
}
