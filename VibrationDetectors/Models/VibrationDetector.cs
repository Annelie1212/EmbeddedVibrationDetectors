using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationDetectors.Models
{
    public class VibrationDetector 
    {
        public static int DeviceId { get; set; } = 12;

        public static int UserId { get; set; } = 1;
        public static string DeviceName { get; set; } = "Vibration Detector";
        public static string Location { get; set; } = "Annelies vardagsrum";

        //Larmad eller inte larmad
        public static bool AlarmArmed { get; set; } = false;

        //Alarm utlöst eller inte
        public static  bool AlarmTriggered { get; set; } = false;

        public static int VibrationLevel { get; set; } = 0;

        //Tröskelvärdets maxvärde ska vara 10.
        public static int VibrationLevelThreshold { get; set; } = 5;


    }
}
