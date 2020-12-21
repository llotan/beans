namespace beans
{
    partial class ucBlobsView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxBlobsView = new System.Windows.Forms.GroupBox();
            this.listViewBlobs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListBlobs = new System.Windows.Forms.ImageList(this.components);
            this.timerM = new System.Windows.Forms.Timer(this.components);
            this.groupBoxBlobsView.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxBlobsView
            // 
            this.groupBoxBlobsView.Controls.Add(this.listViewBlobs);
            this.groupBoxBlobsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBlobsView.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBlobsView.Name = "groupBoxBlobsView";
            this.groupBoxBlobsView.Size = new System.Drawing.Size(480, 382);
            this.groupBoxBlobsView.TabIndex = 4;
            this.groupBoxBlobsView.TabStop = false;
            this.groupBoxBlobsView.Text = "Blobs";
            // 
            // listViewBlobs
            // 
            this.listViewBlobs.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewBlobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBlobs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewBlobs.GridLines = true;
            this.listViewBlobs.HideSelection = false;
            this.listViewBlobs.ImeMode = System.Windows.Forms.ImeMode.On;
            this.listViewBlobs.Location = new System.Drawing.Point(3, 16);
            this.listViewBlobs.MultiSelect = false;
            this.listViewBlobs.Name = "listViewBlobs";
            this.listViewBlobs.OwnerDraw = true;
            this.listViewBlobs.Size = new System.Drawing.Size(474, 363);
            this.listViewBlobs.SmallImageList = this.imageListBlobs;
            this.listViewBlobs.TabIndex = 0;
            this.listViewBlobs.UseCompatibleStateImageBehavior = false;
            this.listViewBlobs.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.DrawItem);
            this.listViewBlobs.SelectedIndexChanged += new System.EventHandler(this.listViewBlobs_SelectedIndexChanged);
            this.listViewBlobs.Click += new System.EventHandler(this.listViewBlobs_Click);
            this.listViewBlobs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewBlobs_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "list of  blobs";
            // 
            // imageListBlobs
            // 
            this.imageListBlobs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListBlobs.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListBlobs.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timerM
            // 
            this.timerM.Interval = 50;
            this.timerM.Tick += new System.EventHandler(this.TimerM_Tick);
            // 
            // ucBlobsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxBlobsView);
            this.Name = "ucBlobsView";
            this.Size = new System.Drawing.Size(480, 382);
            this.Load += new System.EventHandler(this.ucBlobsView_Load);
            this.groupBoxBlobsView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBlobsView;
        private System.Windows.Forms.Timer timerM;
        public System.Windows.Forms.ImageList imageListBlobs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listViewBlobs;
    }
}
