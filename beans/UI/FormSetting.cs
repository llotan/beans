using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using beans.methods;

namespace beans
{
    public partial class FormSetting : Form
    {
        public ucSetting uc;
        public FormSetting( bool isShowMsg = false )
            {

                InitializeComponent();

                uc = new ucSetting();
                uc.Dock = DockStyle.Fill;        

                Controls.Add(uc);
            }

        private void DoIfDataChanged()
        {
            MessageBox.Show("Pls, rerun application");

        }

        private void FormSetting_Load(object sender, EventArgs e)
        {


        }
    }
}
