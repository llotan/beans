namespace beans
{
    partial class FormShowTxt
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbContext = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbContext
            // 
            this.rtbContext.BackColor = System.Drawing.Color.Black;
            this.rtbContext.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContext.ForeColor = System.Drawing.SystemColors.Control;
            this.rtbContext.Location = new System.Drawing.Point(0, 0);
            this.rtbContext.Name = "rtbContext";
            this.rtbContext.Size = new System.Drawing.Size(620, 487);
            this.rtbContext.TabIndex = 0;
            this.rtbContext.Text = "";
            // 
            // FormShowTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(620, 487);
            this.Controls.Add(this.rtbContext);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShowTxt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbContext;
    }
}