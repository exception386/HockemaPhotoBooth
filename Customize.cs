using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace HockemaPhotoBooth
{
    public partial class Customize : Form
    {
        public Customize()
        {
            InitializeComponent();
        }
        
        private void btnCameraSettings_Click(object sender, EventArgs e)
        {
            Globals.goingToCameraSettings = true;

            CameraSettings frmCameraSettings = new CameraSettings();
            frmCameraSettings.Show();
        }

        private void Customize_Load(object sender, EventArgs e)
        {

            chkSoundEnabled.Checked = Globals.soundEnabled;
            cboVoice.Text = Globals.soundVoice;
            if (Globals.soundVoice == "None")
            {
                chkSoundGetReady.Checked = true;
                chkSoundCountdown.Checked = true;
                chkSoundShutter.Checked = true;
                chkSoundLastOne.Checked = true;
                chkSoundNowPrinting.Checked = true;
                chkSoundThankYou.Checked = true;
            }
            else
            {
                chkSoundGetReady.Checked = Globals.soundGetReady;
                chkSoundCountdown.Checked = Globals.soundCountdown;
                chkSoundShutter.Checked = Globals.soundShutter;
                chkSoundLastOne.Checked = Globals.soundLastOne;
                chkSoundNowPrinting.Checked = Globals.soundNowPrinting;
                chkSoundThankYou.Checked = Globals.soundThankYou;
            }
            cboBackground.Text = Globals.visualBackground;
            if (Globals.visualBackground == "None")
            {
                picBackground.Image = null;
            }
            else
            {
                picBackground.Image = Image.FromFile(@"resources\backgrounds\" + Globals.visualBackground);
            }
            picBackgroundColor.BackColor = Color.FromArgb(Globals.visualBackgroundColor);
            picBackground.BackColor = Color.FromArgb(Globals.visualBackgroundColor);
            picPhotostripBGColor.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
            picPhotostrip1.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
            picPhotostrip2.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
            picPhotostrip3.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
            picPhotostrip4.BackColor = Color.FromArgb(Globals.visualPhotostripBackgroundColor);
            picOverlayBGColor.BackColor = Color.FromArgb(Globals.visualOverlayBackgroundColor);
            lblOverlay.BackColor = Color.FromArgb(Globals.visualOverlayBackgroundColor);
            picOverlayFGColor.BackColor = Color.FromArgb(Globals.visualOverlayForegroundColor);
            lblOverlay.ForeColor = Color.FromArgb(Globals.visualOverlayForegroundColor);
            chkAutomaticPrinting.Checked = Globals.printingAutomaticPrinting;
            cboPrintStyle1.Text = Globals.printingPrintStyle1.ToString();
            cboPrintStyle2.Text = Globals.printingPrintStyle2.ToString();
            cboPrintStyle3.Text = Globals.printingPrintStyle3.ToString();

            cboBackground.Items.Clear();
            cboBackground.Items.Add("None");
            string[] files = Directory.GetFiles(@"resources\backgrounds\");
            foreach (string file in files)
            {
                cboBackground.Items.Add(file.Split('\\')[2]);
            }

            cboVoice.Items.Clear();
            cboVoice.Items.Add("None");
            string[] folders = Directory.GetDirectories(@"resources\sounds\");
            foreach (string folder in folders)
            {
                cboVoice.Items.Add(folder.Split('\\')[2]);
            }

            checkSoundFiles();

        }
        
        private void btnApply_Click(object sender, EventArgs e)
        {
            Globals.soundEnabled = chkSoundEnabled.Checked;
            Globals.soundVoice = cboVoice.Text;
            Globals.soundGetReady = chkSoundGetReady.Checked & chkSoundGetReady.Enabled;
            Globals.soundCountdown = chkSoundCountdown.Checked & chkSoundCountdown.Enabled;
            Globals.soundShutter = chkSoundShutter.Checked & chkSoundShutter.Enabled;
            Globals.soundLastOne = chkSoundLastOne.Checked & chkSoundLastOne.Enabled;
            Globals.soundNowPrinting = chkSoundNowPrinting.Checked & chkSoundNowPrinting.Enabled;
            Globals.soundThankYou = chkSoundThankYou.Checked & chkSoundThankYou.Enabled;
            Globals.visualBackground = cboBackground.Text;
            Globals.visualBackgroundColor = picBackgroundColor.BackColor.ToArgb();
            Globals.visualPhotostripBackgroundColor = picPhotostripBGColor.BackColor.ToArgb();
            Globals.visualOverlayBackgroundColor = picOverlayBGColor.BackColor.ToArgb();
            Globals.visualOverlayForegroundColor = picOverlayFGColor.BackColor.ToArgb();
            Globals.printingAutomaticPrinting = chkAutomaticPrinting.Checked;
            Globals.printingPrintStyle1 = Convert.ToDouble(cboPrintStyle1.Text);
            Globals.printingPrintStyle2 = Convert.ToDouble(cboPrintStyle2.Text);
            Globals.printingPrintStyle3 = Convert.ToDouble(cboPrintStyle3.Text);

            string settingsToWrite = "";
            settingsToWrite += "SoundEnabled=" + Globals.soundEnabled + "\r\n";
            settingsToWrite += "SoundVoice=" + Globals.soundVoice + "\r\n";
            settingsToWrite += "SoundGetReady=" + Globals.soundGetReady + "\r\n";
            settingsToWrite += "SoundCountdown=" + Globals.soundCountdown + "\r\n";
            settingsToWrite += "SoundShutter=" + Globals.soundShutter + "\r\n";
            settingsToWrite += "SoundLastOne=" + Globals.soundLastOne + "\r\n";
            settingsToWrite += "SoundNowPrinting=" + Globals.soundNowPrinting + "\r\n";
            settingsToWrite += "SoundThankYou=" + Globals.soundThankYou + "\r\n";
            settingsToWrite += "VisualBackground=" + Globals.visualBackground + "\r\n";
            settingsToWrite += "VisualBackgroundColor=" + Globals.visualBackgroundColor + "\r\n";
            settingsToWrite += "VisualPhotostripBackgroundColor=" + Globals.visualPhotostripBackgroundColor + "\r\n";
            settingsToWrite += "VisualOverlayBackgroundColor=" + Globals.visualOverlayBackgroundColor + "\r\n";
            settingsToWrite += "VisualOverlayForegroundColor=" + Globals.visualOverlayForegroundColor + "\r\n";
            settingsToWrite += "PrintingAutomaticPrinting=" + Globals.printingAutomaticPrinting + "\r\n";
            settingsToWrite += "PrintingPrintStyle1=" + Globals.printingPrintStyle1 + "\r\n";
            settingsToWrite += "PrintingPrintStyle2=" + Globals.printingPrintStyle2 + "\r\n";
            settingsToWrite += "PrintingPrintStyle3=" + Globals.printingPrintStyle3 + "\r\n";

            settingsToWrite += "CameraVideoDevice=" + Globals.cameraVideoDevice + "\r\n";
            settingsToWrite += "CameraVideoWidth=" + Globals.cameraVideoWidth + "\r\n";
            settingsToWrite += "CameraVideoHeight=" + Globals.cameraVideoHeight + "\r\n";
            settingsToWrite += "CameraVideoAspect=" + Globals.cameraVideoAspect + "\r\n";

            File.WriteAllText("settings.ini", settingsToWrite);

        }

        private void picOverlayBG_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = picOverlayBGColor.BackColor;
            colorDialog1.ShowDialog();
            picOverlayBGColor.BackColor = colorDialog1.Color;
            lblOverlay.BackColor = colorDialog1.Color;
        }

        private void picOverlayFG_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = picOverlayFGColor.BackColor;
            colorDialog1.ShowDialog();
            picOverlayFGColor.BackColor = colorDialog1.Color;
            lblOverlay.ForeColor = colorDialog1.Color;
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            chkSoundEnabled.Checked = true;
            cboVoice.Text = "Default";
            chkSoundGetReady.Checked = true;
            chkSoundCountdown.Checked = true;
            chkSoundShutter.Checked = true;
            chkSoundLastOne.Checked = true;
            chkSoundNowPrinting.Checked = true;
            chkSoundThankYou.Checked = true;
            cboBackground.Text = "None";
            picBackground.Image = null;
            picBackgroundColor.BackColor = Color.FromArgb(-12550016);
            picBackground.BackColor = Color.FromArgb(-12550016);
            picPhotostripBGColor.BackColor = Color.FromArgb(-12566464);
            picPhotostrip1.BackColor = Color.FromArgb(-12566464);
            picPhotostrip2.BackColor = Color.FromArgb(-12566464);
            picPhotostrip3.BackColor = Color.FromArgb(-12566464);
            picPhotostrip4.BackColor = Color.FromArgb(-12566464);
            picOverlayBGColor.BackColor = Color.FromArgb(-16777216);
            picOverlayFGColor.BackColor = Color.FromArgb(-1);
            chkAutomaticPrinting.Checked = false;
            cboPrintStyle1.Text = "1.5";
            cboPrintStyle2.Text = "1";
            cboPrintStyle3.Text = "1";
        }

        private void picBackgroundColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = picBackgroundColor.BackColor;
            colorDialog1.ShowDialog();
            picBackgroundColor.BackColor = colorDialog1.Color;
            picBackground.BackColor = colorDialog1.Color;
        }

        private void picPhotostripBGColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = picPhotostripBGColor.BackColor;
            colorDialog1.ShowDialog();
            picPhotostripBGColor.BackColor = colorDialog1.Color;
            picPhotostrip1.BackColor = colorDialog1.Color;
            picPhotostrip2.BackColor = colorDialog1.Color;
            picPhotostrip3.BackColor = colorDialog1.Color;
            picPhotostrip4.BackColor = colorDialog1.Color;
        }

        private void cboBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBackground.SelectedItem.ToString() == "None")
            {
                picBackground.Image = null;
            }
            else
            {
                picBackground.Image = Image.FromFile(@"resources\backgrounds\" + cboBackground.SelectedItem.ToString());
            }
        }

        private void cboVoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkSoundFiles();
        }

        private void checkSoundFiles()
        {
            chkSoundGetReady.Enabled = false;
            chkSoundCountdown.Enabled = false;
            chkSoundLastOne.Enabled = false;
            chkSoundShutter.Enabled = false;
            chkSoundNowPrinting.Enabled = false;
            chkSoundThankYou.Enabled = false;
            if (cboVoice.Text != "None" & Directory.Exists(@"resources\sounds\" + cboVoice.Text))
            {
                int numCount = 0;
                string[] files = Directory.GetFiles(@"resources\sounds\" + cboVoice.Text);
                foreach (string file in files)
                {
                    switch (file.Split('\\')[3])
                    {
                        case "getReady.wav":
                            chkSoundGetReady.Enabled = true;
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
                            chkSoundShutter.Enabled = true;
                            break;
                        case "lastOne.wav":
                            chkSoundLastOne.Enabled = true;
                            break;
                        case "nowPrinting.wav":
                            chkSoundNowPrinting.Enabled = true;
                            break;
                        case "thankYou.wav":
                            chkSoundThankYou.Enabled = true;
                            break;
                    }
                }
                if (numCount == 5)
                {
                    chkSoundCountdown.Enabled = true;
                }
            }
        }

        private void chkSoundEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoundEnabled.Checked)
            {
                cboVoice.Enabled = true;
                checkSoundFiles();
            }
            else
            {
                cboVoice.Enabled = false;
                chkSoundGetReady.Enabled = false;
                chkSoundCountdown.Enabled = false;
                chkSoundLastOne.Enabled = false;
                chkSoundShutter.Enabled = false;
                chkSoundNowPrinting.Enabled = false;
                chkSoundThankYou.Enabled = false;
            }
        }





    }
}
