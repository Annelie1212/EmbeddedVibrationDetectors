using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Models;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Services
{
    public static class DeviceActions
    {
        //public static bool RunningState { get; private set; } = false;
        public static void ToggleArmedState()
        {
            VibrationDetector.AlarmArmed = !VibrationDetector.AlarmArmed;
        }
        public static void  ToggleTriggedState()
        {
            VibrationDetector.AlarmTriggered = !VibrationDetector.AlarmTriggered;
        }
        public static string GetDeviceName()
        {
            return VibrationDetector.DeviceName;
        }
        public static bool GetArmedState() 
        {
            return VibrationDetector.AlarmArmed;
        }
        public static bool GetTriggedState()
        { 
            return VibrationDetector.AlarmTriggered;
        }
        public static string SetThresholdLevel(double sliderValue)
        {
            //Jag vill skriva till cacheminnet dvs använd Update!

            int userPanelAction = (int)DeviceAction.SetThreshold;

            VibrationDetector.VibrationLevelThreshold = (int)sliderValue;

            var logMessage = "Threshold set successfully!";
            //_______TILLFÄLLIG--------------
            //string logMessage = await VDClientService.SetVDAsync(sliderValue, userPanelAction);
            return logMessage;
        }


        public static List<string> Btn_Armed()
        {
            List<string> logList = new List<string>();


            DeviceActions.ToggleArmedState();
            
            if (DeviceActions.GetArmedState() == true)
            {
                var logMessage = "Device armed successfully!";
                logList.Add(logMessage);
            }
            else
            {
                var logMessage = "Device disarmed successfully!";
                logList.Add(logMessage);
            }

            //var logMessage = "Threshold set successfully!";

            if (DeviceActions.GetTriggedState() == true)
            {
                DeviceActions.ToggleTriggedState();
                var logMessage = "Alarm reset successfully.";
                logList.Add(logMessage);
                //var logMessage = "Threshold set successfully!";
            }

            return logList; 
                

        }

        public static string Btn_Trigged()
        {
            if (!DeviceActions.GetArmedState())
            {
                //make sure the button does nothing if the device is not armed
                var logMessage = "You have to press START first";
                return logMessage;
            }
            else
            {
                DeviceActions.ToggleTriggedState();
                if (VibrationDetector.AlarmTriggered == true)
                {
                    var logMessage = "Alarm triggered successfully!";
                    return logMessage;
                }
                else
                {
                    var logMessage = "Alarm reset successfully!";
                    return logMessage;
                }

            }
        }
    }
}
