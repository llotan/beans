using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using beans.filters;

namespace beans
{
    public partial class ucFilterSet : UserControl
    {
        private filters.manager mng;
        public string Caption
        {
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }
        public ucFilterSet()
        {
            InitializeComponent();

            mng = new filters.manager();
        }

        private void UcFilterSet_Load(object sender, EventArgs e)
        {

            foreach (var fnctn in mng .get_available())
            {
                listBoxAvailable.Items.Add(fnctn);
            }

            listBoxAvailable.SelectedIndex = 0;

            foreach (var fnctn in mng.get_active())
            {
                listBoxUsing.Items.Add(fnctn);
            }

            if (listBoxUsing.Items.Count > 0) listBoxUsing.SelectedIndex = 0;

            ListBoxAvailable_MouseDown(sender , null);

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<filters.fnctn4save> list=new List<fnctn4save>();

            foreach (filters.fnctn4save item in listBoxUsing.Items)
                list.Add(item);

            mng.save(list);
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (listBoxUsing.Items.Count > 0)
            {
                listBoxUsing.Items.Remove(listBoxUsing.SelectedItem);
            }

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            filters.fnctn4save item = new fnctn4save();
            item.id = ((filters.fnctn) listBoxAvailable.SelectedItem).id;
            item.text  = ((filters.fnctn)listBoxAvailable.SelectedItem).text;

            listBoxUsing.Items.Add(item);
        }

      
        private void ListBoxAvailable_MouseDown(object sender, MouseEventArgs e)
        {
            btnDown.Enabled = btnUp.Enabled = btnDel.Enabled = false;
            btnAdd.Enabled = true;
        }

        private void ListBoxUsing_MouseDown(object sender, MouseEventArgs e)
        {
            btnDown.Enabled = btnUp.Enabled = btnDel.Enabled = true  ;
            btnAdd.Enabled = false ;
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (listBoxUsing.Items.Count > 0 && listBoxUsing.SelectedIndex>0)
            {
                var temp = listBoxUsing.SelectedItem;
                int inx = listBoxUsing.SelectedIndex;
                listBoxUsing.Items.Remove(listBoxUsing.SelectedItem);
                listBoxUsing.Items.Insert(inx-1,temp);
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (listBoxUsing.Items.Count > 0 && listBoxUsing.SelectedIndex < listBoxUsing.Items.Count - 1 )
            {
                var temp = listBoxUsing.SelectedItem;
                int inx = listBoxUsing.SelectedIndex;
                listBoxUsing.Items.Remove(listBoxUsing.SelectedItem);
                listBoxUsing.Items.Insert(inx + 1, temp);
            }
        }

        private void ListBoxAvailable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnAdd_Click(sender , null);
        }
    }
}
