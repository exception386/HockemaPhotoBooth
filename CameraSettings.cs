/****************************************************************************
    HockemaPhotoBooth - Open Source Photo Booth
    Copyright (C) 2013  Jeff Hockema - www.jeffhockema.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see [http://www.gnu.org/licenses/].
*****************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Media;


namespace HockemaPhotoBooth
{


    public class CameraSettings : System.Windows.Forms.Form
    {


        
        private ComboBox cboResolution;
        private Button btnCapture;
        Bitmap bmpPhoto1;
        private System.Windows.Forms.PictureBox picLivePreview;
        private System.Windows.Forms.PictureBox pic1;
        private Capture cam;

        //int VIDEODEVICE = 0; // zero based index of video capture device to use
        //int VIDEOWIDTH = 640; // Depends on video device caps
        //int VIDEOHEIGHT = 480;
        //short VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device
        //string VIDEOASPECT = "4x3";

        IntPtr m_ip = IntPtr.Zero;
        private Label label1;
        private ComboBox cboIndex;
        private Button btnPreview;
        private Label label2; // Depends on video device caps


        public CameraSettings()
        {
            InitializeComponent();

        }

        protected override void Dispose( bool disposing ) // Clean up any resources being used.
        {
            base.Dispose( disposing );

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }

		#region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picLivePreview = new System.Windows.Forms.PictureBox();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.cboResolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboIndex = new System.Windows.Forms.ComboBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLivePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.SuspendLayout();
            // 
            // picLivePreview
            // 
            this.picLivePreview.BackColor = System.Drawing.Color.Black;
            this.picLivePreview.Location = new System.Drawing.Point(505, 12);
            this.picLivePreview.Name = "picLivePreview";
            this.picLivePreview.Size = new System.Drawing.Size(320, 240);
            this.picLivePreview.TabIndex = 1;
            this.picLivePreview.TabStop = false;
            // 
            // pic1
            // 
            this.pic1.BackColor = System.Drawing.Color.Black;
            this.pic1.Location = new System.Drawing.Point(64, 12);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(320, 240);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1.TabIndex = 2;
            this.pic1.TabStop = false;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(168, 258);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(113, 31);
            this.btnCapture.TabIndex = 3;
            this.btnCapture.Text = "Capture Image";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // cboResolution
            // 
            this.cboResolution.FormattingEnabled = true;
            this.cboResolution.Items.AddRange(new object[] {
            "640x480 (default)",
            "800x600",
            "1280x720",
            "1920x1080"});
            this.cboResolution.Location = new System.Drawing.Point(635, 290);
            this.cboResolution.Name = "cboResolution";
            this.cboResolution.Size = new System.Drawing.Size(143, 21);
            this.cboResolution.TabIndex = 4;
            this.cboResolution.Text = "640x480 (default)";
            this.cboResolution.SelectedIndexChanged += new System.EventHandler(this.comboResolution_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(554, 293);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Resolution:";
            // 
            // cboIndex
            // 
            this.cboIndex.FormattingEnabled = true;
            this.cboIndex.Items.AddRange(new object[] {
            "0 (default)",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cboIndex.Location = new System.Drawing.Point(635, 263);
            this.cboIndex.Name = "cboIndex";
            this.cboIndex.Size = new System.Drawing.Size(143, 21);
            this.cboIndex.TabIndex = 6;
            this.cboIndex.Text = "0 (default)";
            this.cboIndex.SelectedIndexChanged += new System.EventHandler(this.comboIndex_SelectedIndexChanged);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(576, 317);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(174, 31);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "Start Preview / Change Mode";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(554, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Camera Index:";
            // 
            // CameraSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(890, 357);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.cboIndex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboResolution);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.pic1);
            this.Controls.Add(this.picLivePreview);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "CameraSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hockema WebCam";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Primary_FormClosed);
            this.Load += new System.EventHandler(this.Primary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLivePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion



        private void Primary_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
            }

            Globals.settingsPreview = false;
            Globals.goingToCameraSettings = false;

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

        }



        private void btnCapture_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Release any previous buffer
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

            if (Globals.cameraVideoAspect == "16x9" & pic1.Width != 427)
            {
                pic1.Left = 12;
                pic1.Width = 427;
                pic1.Height = 240;
                pic1.Refresh();
            }
            else if (Globals.cameraVideoAspect == "4x3" & pic1.Width != 320)
            {
                pic1.Left = 64;
                pic1.Width = 320;
                pic1.Height = 240;
                pic1.Refresh();
            }

            // capture image
            m_ip = cam.Click();
            Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);

            Graphics g1 = this.pic1.CreateGraphics();
            g1.DrawImage(b, new Rectangle(0, 0, pic1.Width, pic1.Height), new Rectangle(0, 0, Globals.cameraVideoWidth, Globals.cameraVideoHeight), GraphicsUnit.Pixel);
            bmpPhoto1 = new Bitmap(b);
            bmpPhoto1.Save("output\\" + DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".jpeg", ImageFormat.Jpeg);

            Cursor.Current = Cursors.Default;
        }

        private void comboResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboResolution.SelectedIndex == 3)
            {
                Globals.cameraVideoWidth = 1920;
                Globals.cameraVideoHeight = 1080;
                Globals.cameraVideoAspect = "16x9";
            }
            else if (cboResolution.SelectedIndex == 2)
            {
                Globals.cameraVideoWidth = 1280;
                Globals.cameraVideoHeight = 720;
                Globals.cameraVideoAspect = "16x9";
            }
            else if (cboResolution.SelectedIndex == 1)
            {
                Globals.cameraVideoWidth = 800;
                Globals.cameraVideoHeight = 600;
                Globals.cameraVideoAspect = "4x3";
            }
            else
            {
                Globals.cameraVideoWidth = 640;
                Globals.cameraVideoHeight = 480;
                Globals.cameraVideoAspect = "4x3";
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
            }

            if (Globals.cameraVideoAspect == "16x9")
            {
                picLivePreview.Refresh();
                picLivePreview.Left = 451;
                picLivePreview.Width = 427;
                picLivePreview.Height = 240;

            }
            else
            {
                picLivePreview.Refresh();
                picLivePreview.Left = 505;
                picLivePreview.Width = 320;
                picLivePreview.Height = 240;
            }

            try
            {
                Globals.settingsPreview = true;
                cam = new Capture(Globals.cameraVideoDevice, Globals.cameraVideoWidth, Globals.cameraVideoHeight, Globals.cameraVideoBitsPerPixel, picLivePreview);

            }
            catch
            {
                MessageBox.Show("Unable to start preview. Be sure to select a resolution that is supported by your camera.");
            }
        }

        private void comboIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.cameraVideoDevice = cboIndex.SelectedIndex;
        }

        private void Primary_Load(object sender, EventArgs e)
        {
            cboIndex.Text = Globals.cameraVideoDevice.ToString();
            if (Globals.cameraVideoDevice == 0) {cboIndex.Text = "0 (Default)";}

            cboResolution.Text = Globals.cameraVideoWidth + "x" + Globals.cameraVideoHeight;
            if (Globals.cameraVideoWidth == 6400 & Globals.cameraVideoHeight == 480) {cboResolution.Text = "640x480 (Default)";}

        }




    

    }
}
