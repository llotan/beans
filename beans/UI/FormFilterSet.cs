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
    public partial class FormFilterSet : Form
    {
        public FormFilterSet()
        {
            InitializeComponent();


            ucFilterSet  uc=new ucFilterSet();
            uc.Dock = DockStyle.Fill;
            
            this.Controls.Add(uc);

        }
    }
}
