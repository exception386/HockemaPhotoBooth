using System;
using System.Collections.Generic;
using System.Text;

namespace HockemaPhotoBooth
{
    public class Globals
    {
        public static int displayWidth = 1280;
        public static int displayHeight = 1024;
        public static double displayAspect = 0.75;
        public static double currentPrintAspect = 1.5;
        public static int currentPrintStyle = 1;
        public static int currentPhoto = 1;

        public static double printingPrintStyle1 = 1.5;
        public static double printingPrintStyle2 = 1.0;
        public static double printingPrintStyle3 = 1.0;

        public static int countdown = 60;
        public static bool settingsPreview = false;
        public static bool goingToCameraSettings = false;

        public static bool soundEnabled = true;
        public static string soundVoice = "default";
        public static bool soundGetReady = true;
        public static bool soundCountdown = true;
        public static bool soundShutter = true;
        public static bool soundLastOne = true;
        public static bool soundNowPrinting = true;
        public static bool soundThankYou = true;
        public static string visualBackground = "default";
        public static int visualBackgroundColor = -16777216;
        public static int visualPhotostripBackgroundColor = -16777216;
        public static int visualOverlayBackgroundColor = -16777216;
        public static int visualOverlayForegroundColor = -1;
        public static bool printingAutomaticPrinting = true;

        public static int cameraVideoDevice = 0; // zero based index of video capture device to use
        public static int cameraVideoWidth = 640; // Depends on video device caps
        public static int cameraVideoHeight = 480;
        public static string cameraVideoAspect = "4x3";

        public static short cameraVideoBitsPerPixel = 24; // BitsPerPixel values determined by device

    }
}
