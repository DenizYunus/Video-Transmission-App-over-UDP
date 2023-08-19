using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Freelance_VideoTransmissionApp
{
    public partial class VideoForm : Form
    {
        public static VideoForm instance;
        public VideoForm()
        {
            if (instance == null)
            {
                InitializeComponent();
                instance = this;
            }
            else Dispose();
        }

        //public Size GetPictureBoxSize()
        //{
        //    //Invoke(new MethodInvoker(() => 
        //    return leftPictureBox.Size;
        //    //));
        //}

        //public void ChangeRightImage(Image image)
        //{
        //    //Invoke(new MethodInvoker(() => 
        //    rightPictureBox.BackgroundImage = image;
        //        //));
        //}

        public void ProcessAndDisplayImage(Image image)
        {
            // Flip the image horizontally
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            // Create copies of the image for each PictureBox
            Image leftImage = (Image)image.Clone();
            Image rightImage = (Image)image.Clone();

            // Rotate the images
            leftImage = RotateImage(leftImage, 45);
            rightImage = RotateImage(rightImage, -45);

            // Dispose of the old BackgroundImages
            Invoke(new MethodInvoker(() =>
            {
                rightPictureBox.BackgroundImage?.Dispose();
                leftPictureBox.BackgroundImage?.Dispose();

                // Display the images
                rightPictureBox.BackgroundImage = rightImage;
                leftPictureBox.BackgroundImage = leftImage;
            }));

            // Dispose of the cloned images and the original image
            image.Dispose();
        }


        // This function is used to rotate an image by a certain angle
        private static Image RotateImage(Image img, float rotationAngle)
        {
            // Create a new Bitmap that's big enough to fit the rotated image
            int maxSide = (int)Math.Sqrt(img.Width * img.Width + img.Height * img.Height);
            Bitmap bmp = new Bitmap(maxSide, maxSide);

            // Draw the rotated image onto the Bitmap
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                gfx.RotateTransform(rotationAngle);
                gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                gfx.DrawImage(img, (bmp.Width - img.Width) / 2, (bmp.Height - img.Height) / 2, img.Width, img.Height);
            }

            // Resize the Bitmap to fit the PictureBox while maintaining the image's aspect ratio
            int boxSize = 400; // Change this to the size of your PictureBox
            int x, y, width, height;
            if (bmp.Width > bmp.Height)
            {
                width = boxSize;
                height = bmp.Height * boxSize / bmp.Width;
                x = 0;
                y = (boxSize - height) / 2;
            }
            else
            {
                height = boxSize;
                width = bmp.Width * boxSize / bmp.Height;
                y = 0;
                x = (boxSize - width) / 2;
            }

            Bitmap result = new Bitmap(boxSize, boxSize);
            using (Graphics gfx = Graphics.FromImage(result))
            {
                gfx.DrawImage(bmp, x, y, width, height);
            }

            bmp.Dispose();

            return result;
        }

        private void VideoForm_Resize(object sender, EventArgs e)
        {
            leftPictureBox.Size = new Size((Width - 16) / 2, leftPictureBox.Height);
            if (WindowState == FormWindowState.Maximized)
            {
                FormBorderStyle = FormBorderStyle.None;
                //TopMost = true;
            }
        }
    }
}
