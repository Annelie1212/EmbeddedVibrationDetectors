using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VibrationDetectors.Services;

namespace VibrationDetectors.Models
{
    public static class VibrationDetector
    {
        public static int DeviceId { get; set; } = 1;

        public static int UserId { get; set; } = 1;
        public static string DeviceName { get; set; } = "Vibration Detector";
        public static string Location { get; set; } = "Annelies vardagsrum";

        //Larmad eller inte larmad
        public static bool AlarmArmed { get; set; } = false;

        //Alarm utlöst eller inte
        public static bool AlarmTriggered { get; set; } = false;

        //Vibrationsnivå från 0-10
        public static int VibrationLevel { get; set; } = 0;

        //Tröskelvärdets maxvärde ska vara 10.
        public static int VibrationLevelThreshold { get; set; } = 7;

    }
}
