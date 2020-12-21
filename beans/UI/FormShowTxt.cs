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
    public partial class FormShowTxt : Form
    {
        bool fileExist = true;

        public FormShowTxt()
        {
            InitializeComponent();
        }

        public FormShowTxt(string path )
        {
            InitializeComponent();
            if (System.IO.File.Exists(path))
            {
                this.Text = System.IO.Path.GetFileName(path);
                rtbContext.LoadFile(path,RichTextBoxStreamType.PlainText);
                fileExist = true;
            }
        }

        public void Show_()
        {
            if (fileExist)
                ShowDialog();
        }
    }
}
