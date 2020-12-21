using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace beans
{
    public partial class ucSetting : UserControl
    {

        public delegate void DataChangedHandle(ConfigSetting cfg);
        public event DataChangedHandle DataChanged;

        public ConfigSetting cfg_main;
        private bool isChanges = false;

        public string Caption
        {
            get { return groupBoxMain.Text; }
            set { groupBoxMain.Text = value; }
        }


        public ucSetting()
        {
            cfg_main = SettingsManager.Instance.GetCurrent(Application.StartupPath);

            InitializeComponent();

            propertyGridMain.SelectedObject = cfg_main;
        }

        
        #region init load

        
        private void ucSetting_Load(object sender, EventArgs e)
        {
            if (cfg_main != null)
            {
                //lbGroupSettings.Items.Clear();
                //lbGroupSettings.Items.Add(cfg_main);
                //lbGroupSettings.Items.Add(cfg_main.CameraLowResolution);
                //lbGroupSettings.Items.Add(cfg_main.CameraBigResolution);
                //lbGroupSettings.Items.Add(cfg_main.MotorsDirection);

            }



        }

        #endregion

        #region option

  

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:

                    SettingsManager.Instance.SaveCurrent(cfg_main, System.Windows.Forms.Application.StartupPath);

                    DataChanged?.Invoke(cfg_main);

                    isChanges = true;


                    break;
            }
        }

        private void lbGroupSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //propertyGridCB.SelectedObject = lbGroupSettings.SelectedItem;
        }


        #endregion

        private void UcSetting_Leave(object sender, EventArgs e)
        {
        }

        public static void GrantAccess(string folderNM)
        {
            bool exists = System.IO.Directory.Exists(folderNM);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(folderNM);
                Console.WriteLine("The Folder is created Sucessfully");
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
            DirectoryInfo dInfo = new DirectoryInfo(folderNM);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }

        private void UcSetting_Layout(object sender, LayoutEventArgs e)
        {
         //   UcSetting_Leave(sender, null);
        }
    }
}
