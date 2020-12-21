namespace beans
{
    partial class FormSync                                                                                                                                                                                                                                                  
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
            this.components = new System.ComponentModel.Container();
            this.pbMC = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pbMB = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbSC = new System.Windows.Forms.PictureBox();
            this.pbSB = new System.Windows.Forms.PictureBox();
            this.pbRC = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.timerAccessToButton = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbMC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMB)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRC)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMC
            // 
            this.pbMC.Location = new System.Drawing.Point(6, 19);
            this.pbMC.Name = "pbMC";
            this.pbMC.Size = new System.Drawing.Size(776, 89);
            this.pbMC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMC.TabIndex = 1;
            this.pbMC.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(352, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = " ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(718, 440);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // pbMB
            // 
            this.pbMB.Location = new System.Drawing.Point(6, 114);
            this.pbMB.Name = "pbMB";
            this.pbMB.Size = new System.Drawing.Size(776, 87);
            this.pbMB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMB.TabIndex = 4;
            this.pbMB.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbMC);
            this.groupBox1.Controls.Add(this.pbMB);
            this.groupBox1.Location = new System.Drawing.Point(5, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(788, 209);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " camera M";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbSC);
            this.groupBox2.Controls.Add(this.pbSB);
            this.groupBox2.Location = new System.Drawing.Point(6, 225);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(788, 209);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " camera S";
            // 
            // pbSC
            // 
            this.pbSC.Location = new System.Drawing.Point(6, 19);
            this.pbSC.Name = "pbSC";
            this.pbSC.Size = new System.Drawing.Size(776, 89);
            this.pbSC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSC.TabIndex = 1;
            this.pbSC.TabStop = false;
            // 
            // pbSB
            // 
            this.pbSB.Location = new System.Drawing.Point(6, 114);
            this.pbSB.Name = "pbSB";
            this.pbSB.Size = new System.Drawing.Size(776, 87);
            this.pbSB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSB.TabIndex = 4;
            this.pbSB.TabStop = false;
            // 
            // pbRC
            // 
            this.pbRC.Location = new System.Drawing.Point(12, 470);
            this.pbRC.Name = "pbRC";
            this.pbRC.Size = new System.Drawing.Size(776, 89);
            this.pbRC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRC.TabIndex = 7;
            this.pbRC.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 440);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "aqustment";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 440);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "compare";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.Button3_Click_1);
            // 
            // timerAccessToButton
            // 
            this.timerAccessToButton.Interval = 1000;
            this.timerAccessToButton.Tick += new System.EventHandler(this.TimerAccessToButton_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 449);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = " ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 565);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pbRC);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbMC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMB)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbMC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbMB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbSC;
        private System.Windows.Forms.PictureBox pbSB;
        private System.Windows.Forms.PictureBox pbRC;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timerAccessToButton;
        private System.Windows.Forms.Label label1;
    }
}

