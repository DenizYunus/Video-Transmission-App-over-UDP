namespace Freelance_VideoTransmissionApp
{
    partial class VideoForm
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
            this.leftPictureBox = new System.Windows.Forms.PictureBox();
            this.rightPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // leftPictureBox
            // 
            this.leftPictureBox.BackColor = System.Drawing.Color.Black;
            this.leftPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.leftPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPictureBox.Location = new System.Drawing.Point(0, 0);
            this.leftPictureBox.Name = "leftPictureBox";
            this.leftPictureBox.Size = new System.Drawing.Size(500, 500);
            this.leftPictureBox.TabIndex = 1;
            this.leftPictureBox.TabStop = false;
            // 
            // rightPictureBox
            // 
            this.rightPictureBox.BackColor = System.Drawing.Color.Black;
            this.rightPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rightPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPictureBox.Location = new System.Drawing.Point(500, 0);
            this.rightPictureBox.Name = "rightPictureBox";
            this.rightPictureBox.Size = new System.Drawing.Size(500, 500);
            this.rightPictureBox.TabIndex = 5;
            this.rightPictureBox.TabStop = false;
            // 
            // VideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.rightPictureBox);
            this.Controls.Add(this.leftPictureBox);
            this.Name = "VideoForm";
            this.Text = "VideoForm";
            this.Resize += new System.EventHandler(this.VideoForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox leftPictureBox;
        private PictureBox rightPictureBox;
    }
}