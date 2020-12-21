using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans.UI
{
    public partial class FormTraceSensor : Form
    {
        private DataSet dataSet;

        public FormTraceSensor(DataSet ds)
        {
            dataSet = ds;

            InitializeComponent();

            BindingSource bb = new BindingSource(ds, "blow");
            BindingSource bs = new BindingSource(ds, "sensor");


            dataGridView1.DataSource = bb;
            dataGridView2.DataSource = bs;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewRow row = dataGridView1.SelectedRows[0]; //Selecting only first selected row.

            //Above code of getting row will not work if you have not set datagridview mode to full selection.
            //If you do not want to you could get row like this

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            if (checkBox1.Checked)
            {

                int value = Convert.ToInt32(row.Cells["id"].Value);
//                ((dataGridView2.DataSource as BindingSource).DataSource as DataSet).Tables["sensor"].DefaultView.RowFilter =
//                    string.Format("id= '{0}'", CategoryIdOfSelectedOne);

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int v = Convert.ToInt32(dataGridView1.Rows[i].Cells["id"].Value);
                    if (v == value)
                    {
                        dataGridView2.Rows[i].Selected = true;
                        break;
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) {/* dataGridView1_CellClick(null,null);*/}
                else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "";
            }
        }
    }
}
