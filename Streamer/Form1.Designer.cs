namespace Freelance_VideoTransmissionApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.selectVideoSourceComboBox = new System.Windows.Forms.ComboBox();
            this.remoteIPAddressTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(456, 251);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // selectVideoSourceComboBox
            // 
            this.selectVideoSourceComboBox.FormattingEnabled = true;
            this.selectVideoSourceComboBox.Location = new System.Drawing.Point(347, 23);
            this.selectVideoSourceComboBox.Name = "selectVideoSourceComboBox";
            this.selectVideoSourceComboBox.Size = new System.Drawing.Size(121, 23);
            this.selectVideoSourceComboBox.TabIndex = 2;
            this.selectVideoSourceComboBox.Text = "Select Video Source";
            this.selectVideoSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.selectVideoSourceComboBox_SelectedIndexChanged);
            // 
            // remoteIPAddressTextBox
            // 
            this.remoteIPAddressTextBox.Location = new System.Drawing.Point(190, 23);
            this.remoteIPAddressTextBox.Name = "remoteIPAddressTextBox";
            this.remoteIPAddressTextBox.PlaceholderText = "Enter Remote IP Address";
            this.remoteIPAddressTextBox.Size = new System.Drawing.Size(151, 23);
            this.remoteIPAddressTextBox.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Freelance_VideoTransmissionApp.Properties.Resources.black;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(12, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(172, 52);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(480, 326);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.remoteIPAddressTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.selectVideoSourceComboBox);
            this.Name = "Form1";
            this.Text = "Holo3D";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private PictureBox leftPictureBox;
        private PictureBox pictureBox1;
        private ComboBox selectVideoSourceComboBox;
        //private PictureBox rightPictureBox;
        private TextBox remoteIPAddressTextBox;
        private PictureBox pictureBox2;
    }
}