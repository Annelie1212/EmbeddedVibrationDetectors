using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Models
{
    public class DeviceLog
    {
        public int DeviceLogId { get; set; }
        public int UserId { get; set; }
        public string DeviceType { get; set; }
        public int DeviceId { get; set; }
        public DateTime LogTime { get; set; }
        public DeviceAction DeviceAction { get; set; }
        public bool DeviceActionSuccess { get; set; }
        public string ErrorMessage { get; set; }


        //Larmad eller inte larmad
        public bool AlarmArmed { get; set; } = false;

        //Alarm utlöst eller inte
        public bool AlarmTriggered { get; set; } = false;

        public int VibrationLevel { get; set; } = 0;

        //Tröskelvärdets maxvärde ska vara 10.
        public int VibrationLevelThreshold { get; set; } = 5;

        public string LogMessage { get; set; }



        private void BuildLogMessage()
        {
            string message = "";
            switch (DeviceAction)
            {
                case DeviceAction.ArmDevice:
                    message = AlarmArmed ? "Device armed successfully." : "Device disarmed successfully.";
                    break;
                case DeviceAction.TriggerDevice:
                    message = AlarmTriggered ? "Alarm triggered!" : "Alarm reset successfully.";
                    break;
                case DeviceAction.SetThreshold:
                    message = $"Vibration level threshold set to {VibrationLevelThreshold}.";
                    break;
                default:
                    message = "Annelies unknown action.";
                    break;
            }
            var dateString = LogTime.ToString("yyyy-MM-dd HH:mm:ss");
            LogMessage = dateString + ": " + message;
        }

        public void PopulateDeviceLog(VDFetchStatusResponse sres)
        {
            //DeviceLogId = sr.VibrationDetectorId;
            UserId = sres.VibrationDetectorId;
            DeviceType = "VibrationDetector";
            DeviceId = sres.VibrationDetectorId;
            LogTime = DateTime.Now;
            //TODO Ta från request!!!!
            DeviceAction = (DeviceAction)99;
            DeviceActionSuccess = sres.RequestSuccessful;
            ErrorMessage = sres.ErrorMessage;
            AlarmArmed = sres.AlarmArmed;
            AlarmTriggered = sres.AlarmTriggered;
            VibrationLevel = sres.VibrationLevel;
            VibrationLevelThreshold = sres.VibrationLevelThreshold;
            BuildLogMessage();
        }
    }
}
