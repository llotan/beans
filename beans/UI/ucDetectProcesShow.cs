using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
{
    public partial class ucDetectProcesShow : UserControl
    {

        public string Caption
        {
            get { return gbCamera.Text; }
            set { gbCamera.Text = value; }
        }
        public ucDetectProcesShow()
        {
            InitializeComponent();
        }

        private void GbCamera_Enter(object sender, EventArgs e)
        {

        }
    }
}
