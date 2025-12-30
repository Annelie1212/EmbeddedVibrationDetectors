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
        public static void ToggleTriggedState()
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
    }
}
