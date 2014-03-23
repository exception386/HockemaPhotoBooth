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
using System.IO;

namespace HockemaPhotoBooth
{

    public partial class Booth : Form
    {
        SoundPlayer sound1 = new SoundPlayer("resources\\sounds\\default\\1.wav");
        SoundPlayer sound2 = new SoundPlayer("resources\\sounds\\default\\2.wav");
        SoundPlayer sound3 = new SoundPlayer("resources\\sounds\\default\\3.wav");
        SoundPlayer sound4 = new SoundPlayer("resources\\sounds\\default\\4.wav");
        SoundPlayer sound5 = new SoundPlayer("resources\\sounds\\default\\5.wav");
        SoundPlayer soundLastOne = new SoundPlayer("resources\\sounds\\default\\lastOne.wav");
        SoundPlayer soundThankYou = new SoundPlayer("resources\\sounds\\default\\thankYou.wav");
        SoundPlayer soundGetReady = new SoundPlayer("resources\\sounds\\default\\getReady.wav");
        SoundPlayer soundShutter = new SoundPlayer("resources\\sounds\\default\\shutter.wav");
        SoundPlayer soundNowPrinting = new SoundPlayer("resources\\sounds\\default\\nowPrinting.wav");

        Bitmap bmpPhoto1;
        Bitmap bmpPhoto2;
        Bitmap bmpPhoto3;
        Bitmap bmpPhoto4;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Booth());
        }

        private Capture cam;
        IntPtr m_ip = IntPtr.Zero;


        public Booth()
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

        private void Booth_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
            }

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }
        
        private void btnCustomize_Click(object sender, EventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
            }
            Customize frmCustomize = new Customize();
            frmCustomize.FormClosed += new FormClosedEventHandler(frmCustomize_FormClosed);
            frmCustomize.Show();
        }

        void frmCustomize_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Globals.goingToCameraSettings == false)
            {
                loadSettings();
                startPreview();

            }
        }


        private void Booth_Load(object sender, EventArgs e)
        {

            loadSettings();
            startPreview();

        }

        private void loadSettings()
        {
            string[] lines = File.ReadAllLines("settings.ini");

            foreach (string line in lines)
            {
                switch (line.Split('=')[0])
                {
                    case "SoundEnabled":
                        Globals.soundEnabled = Convert.ToBoolean(line.Split('=')[1]);
                        break;
                    case "SoundVoice":
                        Globals.soundVoice = line.Split('=')[1];
                        break;
                    case "SoundGetReady":
                        Globals.soundGetReady = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundGetReady & Globals.soundEnabled)
                        {
                            soundGetReady = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\getReady.wav");
                        }
                        break;
                    case "SoundCountdown":
                        Globals.soundCountdown = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundCountdown & Globals.soundEnabled)
                        {
                            sound1 = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\1.wav");
                            sound2 = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\2.wav");
                            sound3 = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\3.wav");
                            sound4 = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\4.wav");
                            sound5 = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\5.wav");
                        }
                        break;
                    case "SoundShutter":
                        Globals.soundShutter = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundShutter & Globals.soundEnabled)
                        {
                            soundShutter = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\shutter.wav");
                        }
                        break;
                    case "SoundLastOne":
                        Globals.soundLastOne = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundLastOne & Globals.soundEnabled)
                        {
                            soundLastOne = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\lastOne.wav");
                        }
                        break;
                    case "SoundNowPrinting":
                        Globals.soundNowPrinting = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundNowPrinting & Globals.soundEnabled)
                        {
                            soundNowPrinting = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\nowPrinting.wav");
                        }
                        break;
                    case "SoundThankYou":
                        Globals.soundThankYou = Convert.ToBoolean(line.Split('=')[1]);
                        if (Globals.soundThankYou & Globals.soundEnabled)
                        {
                            soundThankYou = new SoundPlayer("resources\\sounds\\" + Globals.soundVoice + "\\thankYou.wav");
                        }
                        break;
                    case "VisualBackground":
                        Globals.visualBackground = line.Split('=')[1];
                        if (Globals.visualBackground == "None")
                        {
                            this.BackgroundImage = null;
                        }
                        else
                        {
                            this.BackgroundImage = Image.FromFile(@"resources\backgrounds\" + Globals.visualBackground);
                        }
                        break;
                    case "VisualBackgroundColor":
                        Globals.visualBackgroundColor = Convert.ToInt32(line.Split('=')[1]);
                        this.BackColor = Color.FromArgb(Globals.visualBackgroundColor);
                        break;
                    case "VisualPhotostripBackgroundColor":
                        Globals.visualPhotostripBackgroundColor = Convert.ToInt32(line.Split('=')[1]);
                        pic1.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
                        pic2.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
                        pic3.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
                        pic4.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
                        break;
                    case "VisualOverlayBackgroundColor":
                        Globals.visualOverlayBackgroundColor = Convert.ToInt32(line.Split('=')[1]);
                        lblAnnouncement.BackColor = Color.FromArgb(Globals.visualOverlayBackgroundColor);
                        lblCountdown.BackColor = Color.FromArgb(Globals.visualOverlayBackgroundColor);
                        break;
                    case "VisualOverlayForegroundColor":
                        Globals.visualOverlayForegroundColor = Convert.ToInt32(line.Split('=')[1]);
                        lblAnnouncement.ForeColor = Color.FromArgb(Globals.visualOverlayForegroundColor);
                        lblCountdown.ForeColor = Color.FromArgb(Globals.visualOverlayForegroundColor);
                        break;
                    case "PrintingAutomaticPrinting":
                        Globals.printingAutomaticPrinting = Convert.ToBoolean(line.Split('=')[1]);
                        break;
                    case "PrintingPrintStyle1":
                        Globals.printingPrintStyle1 = Convert.ToDouble(line.Split('=')[1]);
                        break;
                    case "PrintingPrintStyle2":
                        Globals.printingPrintStyle2 = Convert.ToDouble(line.Split('=')[1]);
                        break;
                    case "PrintingPrintStyle3":
                        Globals.printingPrintStyle3 = Convert.ToDouble(line.Split('=')[1]);
                        break;
                    case "CameraVideoDevice":
                        Globals.cameraVideoDevice = Convert.ToInt32(line.Split('=')[1]);
                        break;
                    case "CameraVideoWidth":
                        Globals.cameraVideoWidth = Convert.ToInt32(line.Split('=')[1]);
                        break;
                    case "CameraVideoHeight":
                        Globals.cameraVideoHeight = Convert.ToInt32(line.Split('=')[1]);
                        break;
                    case "CameraVideoAspect":
                        Globals.cameraVideoAspect = line.Split('=')[1];
                        break;
                }
            }
            Globals.displayWidth = this.Width;
            Globals.displayHeight = this.Height;
            checkSoundFiles();

        }

        private void checkSoundFiles()
        {
            Globals.soundCountdown = false;
            Globals.soundGetReady = false;
            Globals.soundLastOne = false;
            Globals.soundShutter = false;
            Globals.soundNowPrinting = false;
            Globals.soundThankYou = false;
            if (Globals.soundVoice != "None" & Directory.Exists(@"resources\sounds\" + Globals.soundVoice))
            {
                int numCount = 0;
                string[] files = Directory.GetFiles(@"resources\sounds\" + Globals.soundVoice);
                foreach (string file in files)
                {
                    switch (file.Split('\\')[3])
                    {
                        case "getReady.wav":
                            Globals.soundGetReady = true;
                            break;
                        case "5.wav":
                            numCount += 1;
                            break;
                        case "4.wav":
                            numCount += 1;
                            break;
                        case "3.wav":
                            numCount += 1;
                            break;
                        case "2.wav":
                            numCount += 1;
                            break;
                        case "1.wav":
                            numCount += 1;
                            break;
                        case "shutter.wav":
                            Globals.soundShutter = true;
                            break;
                        case "lastOne.wav":
                            Globals.soundLastOne = true;
                            break;
                        case "nowPrinting.wav":
                            Globals.soundNowPrinting = true;
                            break;
                        case "thankYou.wav":
                            Globals.soundThankYou = true;
                            break;
                    }
                }
                if (numCount == 5)
                {
                    Globals.soundCountdown = true;
                }
            }
        }

        private void startPreview()
        {
            if (cam != null)
            {
                cam.Dispose();
            }

            pic1.Refresh();
            pic2.Refresh();
            pic3.Refresh();
            pic4.Refresh();

            if ((float)Globals.displayHeight / (float)Globals.displayWidth < 0.65)
            {
                Globals.displayAspect = 0.5625;

                if (Globals.currentPrintAspect == 1.5)
                {
                    picLive.Left = (int)(0.252343 * this.Width);
                    picLive.Top = (int)(0.083333 * this.Height);
                    picLive.Width = (int)(0.703125 * this.Width);
                    picLive.Height = (int)(0.833333 * this.Height);
                    picFlash.Left = (int)(0.252343 * this.Width);
                    picFlash.Top = (int)(0.083333 * this.Height);
                    picFlash.Width = (int)(0.703125 * this.Width);
                    picFlash.Height = (int)(0.833333 * this.Height);
                    pic1.Left = (int)(0.009375 * this.Width);
                    pic1.Top = (int)(0.015277 * this.Height);
                    pic1.Width = (int)(0.199218 * this.Width);
                    pic1.Height = (int)(0.236111 * this.Height);
                    pic2.Left = (int)(0.009375 * this.Width);
                    pic2.Top = (int)(0.259722 * this.Height);
                    pic2.Width = (int)(0.199218 * this.Width);
                    pic2.Height = (int)(0.236111 * this.Height);
                    pic3.Left = (int)(0.009375 * this.Width);
                    pic3.Top = (int)(0.504166 * this.Height);
                    pic3.Width = (int)(0.199218 * this.Width);
                    pic3.Height = (int)(0.236111 * this.Height);
                    pic4.Left = (int)(0.009375 * this.Width);
                    pic4.Top = (int)(0.748611 * this.Height);
                    pic4.Width = (int)(0.199218 * this.Width);
                    pic4.Height = (int)(0.236111 * this.Height);
                    btnCustomize.Left = (int)(0.9125 * this.Width);
                    btnCustomize.Top = (int)(0.941666 * this.Height);
                    btnCustomize.Width = (int)(0.078125 * this.Width);
                    btnCustomize.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle1.Left = (int)(0.407812 * this.Width);
                    btnPrintStyle1.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle1.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle1.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle2.Left = (int)(0.564062 * this.Width);
                    btnPrintStyle2.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle2.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle2.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle3.Left = (int)(0.720312 * this.Width);
                    btnPrintStyle3.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle3.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle3.Height = (int)(0.041666 * this.Height);
                    lblAnnouncement.Left = (int)(0.402343 * this.Width);
                    lblAnnouncement.Top = (int)(0.4375 * this.Height);
                    lblAnnouncement.Width = (int)(0.403125 * this.Width);
                    lblAnnouncement.Height = (int)(0.122222 * this.Height);
                    lblCountdown.Left = (int)(0.58125 * this.Width);
                    lblCountdown.Top = (int)(0.802777 * this.Height);
                    lblCountdown.Width = (int)(0.040625 * this.Width);
                    lblCountdown.Height = (int)(0.084722 * this.Height);
                }
                else
                {
                    picLive.Left = (int)(0.340625 * this.Width);
                    picLive.Top = (int)(0.080555 * this.Height);
                    picLive.Width = (int)(0.46875 * this.Width);
                    picLive.Height = (int)(0.833333 * this.Height);
                    picFlash.Left = (int)(0.340625 * this.Width);
                    picFlash.Top = (int)(0.080555 * this.Height);
                    picFlash.Width = (int)(0.46875 * this.Width);
                    picFlash.Height = (int)(0.833333 * this.Height);
                    pic1.Left = (int)(0.009375 * this.Width);
                    pic1.Top = (int)(0.015277 * this.Height);
                    pic1.Width = (int)(0.132812 * this.Width);
                    pic1.Height = (int)(0.236111 * this.Height);
                    pic2.Left = (int)(0.009375 * this.Width);
                    pic2.Top = (int)(0.259722 * this.Height);
                    pic2.Width = (int)(0.132812 * this.Width);
                    pic2.Height = (int)(0.236111 * this.Height);
                    pic3.Left = (int)(0.009375 * this.Width);
                    pic3.Top = (int)(0.504166 * this.Height);
                    pic3.Width = (int)(0.132812 * this.Width);
                    pic3.Height = (int)(0.236111 * this.Height);
                    pic4.Left = (int)(0.009375 * this.Width);
                    pic4.Top = (int)(0.748611 * this.Height);
                    pic4.Width = (int)(0.132812 * this.Width);
                    pic4.Height = (int)(0.236111 * this.Height);
                    btnCustomize.Left = (int)(0.9125 * this.Width);
                    btnCustomize.Top = (int)(0.941666 * this.Height);
                    btnCustomize.Width = (int)(0.078125 * this.Width);
                    btnCustomize.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle1.Left = (int)(0.379687 * this.Width);
                    btnPrintStyle1.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle1.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle1.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle2.Left = (int)(0.535937 * this.Width);
                    btnPrintStyle2.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle2.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle2.Height = (int)(0.041666 * this.Height);
                    btnPrintStyle3.Left = (int)(0.692187 * this.Width);
                    btnPrintStyle3.Top = (int)(0.941666 * this.Height);
                    btnPrintStyle3.Width = (int)(0.078125 * this.Width);
                    btnPrintStyle3.Height = (int)(0.041666 * this.Height);
                    lblAnnouncement.Left = (int)(0.375 * this.Width);
                    lblAnnouncement.Top = (int)(0.4375 * this.Height);
                    lblAnnouncement.Width = (int)(0.403125 * this.Width);
                    lblAnnouncement.Height = (int)(0.122222 * this.Height);
                    lblCountdown.Left = (int)(0.553906 * this.Width);
                    lblCountdown.Top = (int)(0.802777 * this.Height);
                    lblCountdown.Width = (int)(0.040625 * this.Width);
                    lblCountdown.Height = (int)(0.084722 * this.Height);
                }

            }
            else
            {
                Globals.displayAspect = 0.75;
                MessageBox.Show("Support for your screen resolution is not yet developed. 1280x720 or 1920x1080 are ideal.");
                if (Globals.currentPrintAspect == 1.5)
                {
                    picLive.Left = (int)(0.255468 * this.Width);
                    picLive.Top = (int)(0.193359 * this.Height);
                    picLive.Width = (int)(0.734375 * this.Width);
                    picLive.Height = (int)(0.611328 * this.Height);
                    pic1.Left = (int)(0.009375 * this.Width);
                    pic1.Top = (int)(0.0488281 * this.Height);
                    pic1.Width = (int)(0.234375 * this.Width);
                    pic1.Height = (int)(0.195312 * this.Height);
                    pic2.Left = (int)(0.009375 * this.Width);
                    pic2.Top = (int)(0.2832031 * this.Height);
                    pic2.Width = (int)(0.234375 * this.Width);
                    pic2.Height = (int)(0.195312 * this.Height);
                    pic3.Left = (int)(0.009375 * this.Width);
                    pic3.Top = (int)(0.5205078 * this.Height);
                    pic3.Width = (int)(0.234375 * this.Width);
                    pic3.Height = (int)(0.195312 * this.Height);
                    pic4.Left = (int)(0.009375 * this.Width);
                    pic4.Top = (int)(0.7539062 * this.Height);
                    pic4.Width = (int)(0.234375 * this.Width);
                    pic4.Height = (int)(0.195312 * this.Height);
                    btnCustomize.Left = (int)(0.911718 * this.Width);
                    btnCustomize.Top = (int)(0.918945 * this.Height);
                    btnCustomize.Width = (int)(0.078125 * this.Width);
                    btnCustomize.Height = (int)(0.029296 * this.Height);
                }
                else
                {
                    picLive.Left = (int)(0.29375 * this.Width);
                    picLive.Top = (int)(0.108398 * this.Height);
                    picLive.Width = (int)(0.625 * this.Width);
                    picLive.Height = (int)(0.78125 * this.Height);
                    pic1.Left = (int)(0.009375 * this.Width);
                    pic1.Top = (int)(0.011718 * this.Height);
                    pic1.Width = (int)(0.192187 * this.Width);
                    pic1.Height = (int)(0.240234 * this.Height);
                    pic2.Left = (int)(0.009375 * this.Width);
                    pic2.Top = (int)(0.256835 * this.Height);
                    pic2.Width = (int)(0.192187 * this.Width);
                    pic2.Height = (int)(0.240234 * this.Height);
                    pic3.Left = (int)(0.009375 * this.Width);
                    pic3.Top = (int)(0.501953 * this.Height);
                    pic3.Width = (int)(0.192187 * this.Width);
                    pic3.Height = (int)(0.240234 * this.Height);
                    pic4.Left = (int)(0.009375 * this.Width);
                    pic4.Top = (int)(0.747070 * this.Height);
                    pic4.Width = (int)(0.192187 * this.Width);
                    pic4.Height = (int)(0.240234 * this.Height);
                    btnCustomize.Left = (int)(0.9125 * this.Width);
                    btnCustomize.Top = (int)(0.958984 * this.Height);
                    btnCustomize.Width = (int)(0.078125 * this.Width);
                    btnCustomize.Height = (int)(0.029296 * this.Height);
                }
            }

            try
            {
                cam = new Capture(Globals.cameraVideoDevice, Globals.cameraVideoWidth, Globals.cameraVideoHeight, Globals.cameraVideoBitsPerPixel, picLive);
            }
            catch
            {
                MessageBox.Show("Unable to start camera. Click 'Customize' to review your camera settings.");
            }
        }

        private void btnPrintStyle1_Click(object sender, EventArgs e)
        {
            lblAnnouncement.Text = "PRINT STYLE 1 SELECTED";
            Globals.currentPrintStyle = 1;
            if (Globals.currentPrintAspect != Globals.printingPrintStyle1)
            {
                Globals.currentPrintAspect = Globals.printingPrintStyle1;
                startPreview();
            }
            tmrChangePrintStyle.Enabled = true;
        }

        private void btnPrintStyle2_Click(object sender, EventArgs e)
        {
            lblAnnouncement.Text = "PRINT STYLE 2 SELECTED";
            Globals.currentPrintStyle = 2;
            if (Globals.currentPrintAspect != Globals.printingPrintStyle2)
            {
                Globals.currentPrintAspect = Globals.printingPrintStyle2;
                startPreview();
            }
            tmrChangePrintStyle.Enabled = true;
        }

        private void btnPrintStyle3_Click(object sender, EventArgs e)
        {
            lblAnnouncement.Text = "PRINT STYLE 3 SELECTED";
            Globals.currentPrintStyle = 3;
            if (Globals.currentPrintAspect != Globals.printingPrintStyle3)
            {
                Globals.currentPrintAspect = Globals.printingPrintStyle3;
                startPreview();
            }
            tmrChangePrintStyle.Enabled = true;
        }

        private void tmrChangePrintStyle_Tick(object sender, EventArgs e)
        {
            tmrChangePrintStyle.Enabled = false;
            lblAnnouncement.Text = "PRESS SPACE TO START";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            pic1.Refresh();
            pic2.Refresh();
            pic3.Refresh();
            pic4.Refresh();
            Globals.countdown = 60;
            Globals.currentPhoto = 1;
            tmrCountdown.Enabled = true;
        }

        private void tmrCountdown_Tick(object sender, EventArgs e)
        {
            Globals.countdown--;
            if (Globals.countdown == 59)
            {
                if (Globals.soundEnabled & Globals.soundGetReady)
                {
                    soundGetReady.Play();
                }
                lblAnnouncement.Text = "GET READY!";
            }
            else if (Globals.countdown == 49)
            {
                lblAnnouncement.Visible = false;
                if (Globals.soundEnabled & Globals.soundCountdown)
                {
                    sound5.Play();
                }
                lblCountdown.Text = "5";
                lblCountdown.Visible = true;
            }
            else if (Globals.countdown == 39)
            {
                if (Globals.currentPhoto == 4)
                {
                    if (Globals.soundEnabled & Globals.soundLastOne)
                    {
                        soundLastOne.Play();
                    }
                }
                else
                {
                    if (Globals.soundEnabled & Globals.soundCountdown)
                    {
                        sound4.Play();
                    }
                }
                lblCountdown.Text = "4";
            }
            else if (Globals.countdown == 29)
            {
                if (Globals.soundEnabled & Globals.soundCountdown)
                {
                    sound3.Play();
                }
                lblCountdown.Text = "3";
                lblCountdown.Visible = true;
            }
            else if (Globals.countdown == 19)
            {
                if (Globals.soundEnabled & Globals.soundCountdown)
                {
                    sound2.Play();
                }
                lblCountdown.Text = "2";
            }
            else if (Globals.countdown == 9)
            {
                if (Globals.soundEnabled & Globals.soundCountdown)
                {
                    sound1.Play();
                }
                
                lblCountdown.Text = "1";
            }
            else if (Globals.countdown == 1)
            {
                if (Globals.soundEnabled & Globals.soundShutter)
                {
                    soundShutter.Play();
                }
            }
            else if (Globals.countdown == 0)
            {
                lblCountdown.Visible = false;
                picFlash.BringToFront();
                picFlash.Visible = true;
                
            }
            else if (Globals.countdown == -1)
            {
                picFlash.Visible = false;
                takeShot();
                if (Globals.currentPhoto <= 2)
                {
                    Globals.countdown = 38;
                }
                else if (Globals.currentPhoto == 3)
                {
                    Globals.countdown = 48;
                }
                Globals.currentPhoto++;
            }
            else if (Globals.countdown == -9)
            {
                if (Globals.printingAutomaticPrinting)
                {
                    if (Globals.soundEnabled & Globals.soundNowPrinting)
                    {
                        soundNowPrinting.Play();
                    }
                    lblAnnouncement.Text = "NOW PRINTING...";
                    lblAnnouncement.Visible = true;
                }
                else
                {
                    if (Globals.soundEnabled & Globals.soundThankYou)
                    {
                        soundThankYou.Play();
                    }
                    lblAnnouncement.Text = "THANK YOU";
                    lblAnnouncement.Visible = true;
                }

            }
            else if (Globals.countdown == -19)
            {
                if (Globals.printingAutomaticPrinting)
                {
                    if (Globals.soundEnabled & Globals.soundThankYou)
                    {
                        soundThankYou.Play();
                    }
                    lblAnnouncement.Text = "THANK YOU";
                    lblAnnouncement.Visible = true;
                }
            }
            else if (Globals.countdown == -39)
            {
                lblAnnouncement.Visible = false;
            }
            else if (Globals.countdown == -49)
            {
                lblAnnouncement.Text = "PRESS SPACE TO START";
                lblAnnouncement.Visible = true;
            }
            else if (Globals.countdown == -99)
            {
                pic1.Refresh();
            }
            else if (Globals.countdown == -104)
            {
                pic2.Refresh();
            }
            else if (Globals.countdown == -109)
            {
                pic3.Refresh();
            }
            else if (Globals.countdown == -114)
            {
                pic4.Refresh();
                tmrCountdown.Enabled = false;
            }

        }

        private void takeShot()
        {
            Cursor.Current = Cursors.WaitCursor;

            // Release any previous buffer
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
            
            // capture image
            m_ip = cam.Click();
            Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);

            Graphics g1;

            if (Globals.currentPhoto == 4)
            {
                bmpPhoto4 = new Bitmap(b);
                bmpPhoto4.Save("output\\" + DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".jpeg", ImageFormat.Jpeg);
                g1 = this.pic4.CreateGraphics();
            }
            else if (Globals.currentPhoto == 3)
            {
                bmpPhoto3 = new Bitmap(b);
                bmpPhoto3.Save("output\\" + DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".jpeg", ImageFormat.Jpeg);
                g1 = this.pic3.CreateGraphics();
            }
            else if (Globals.currentPhoto == 2)
            {
                bmpPhoto2 = new Bitmap(b);
                bmpPhoto2.Save("output\\" + DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".jpeg", ImageFormat.Jpeg);
                g1 = this.pic2.CreateGraphics();
            }
            else
            {
                bmpPhoto1 = new Bitmap(b);
                bmpPhoto1.Save("output\\" + DateTime.Now.ToString("yyyyMMdd.hhmmss") + ".jpeg", ImageFormat.Jpeg);
                g1 = this.pic1.CreateGraphics();
            }
            
            if ((Globals.cameraVideoHeight * Globals.currentPrintAspect) > Globals.cameraVideoWidth)
            {
                g1.DrawImage(b, new Rectangle(0, 0, pic1.Width, pic1.Height), new Rectangle(0, 25, b.Width, b.Height - 50), GraphicsUnit.Pixel);
            }
            else
            {
                g1.DrawImage(b, new Rectangle(0, 0, pic1.Width, pic1.Height), new Rectangle(100, 0, b.Width - 200, b.Height), GraphicsUnit.Pixel);
            }

            Cursor.Current = Cursors.Default;
        }

    }


}
