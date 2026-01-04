using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Services;
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
        public static void SetThresholdLevel(double sliderValue)
        {
            //TODO: kolla så att DeviceAction värdet är satt innan BuildLogMessage anropas.
            //Fundera över dlogs livscykel.
            //dl.BuildLogMessage();

            int userPanelAction = (int)DeviceAction.SetThreshold;

            VibrationDetector.VibrationLevelThreshold = (int)sliderValue;

            //var logMessage = "Threshold set successfully!";
            //var logMessage = dl.LogMessage;

            //_______TILLFÄLLIG--------------
            //string logMessage = await VDClientService.SetVDAsync(sliderValue, userPanelAction);
            //return logMessage;

        }


        public static DeviceAction Btn_Armed()
        {
            //List<string> logList = new List<string>();


            DeviceActions.ToggleArmedState();

            if (DeviceActions.GetArmedState() == true)
            {
                //var logMessage = "Device armed successfully!";
                //logList.Add(logMessage);
                return DeviceAction.ArmDevice;
            }
            else if(DeviceActions.GetArmedState() == false)
            {
                //var logMessage = "Device disarmed successfully!";
                //logList.Add(logMessage);
                return DeviceAction.DisarmDevice;
            }else if(DeviceActions.GetTriggedState() == true)
            {
                //var logMessage = "Alarm reset successfully.";
                //logList.Add(logMessage);
                return DeviceAction.TriggerDevice;
            }
            else if(DeviceActions.GetTriggedState() == false)
            {
                //var logMessage = "Alarm reset successfully.";
                //logList.Add(logMessage);
                return DeviceAction.ResetDevice;
            }
            else
            {
                return DeviceAction.Error;
            }

            //var logMessage = "Threshold set successfully!";

            //if (DeviceActions.GetTriggedState() == true)
            //{
            //    DeviceActions.ToggleTriggedState();
            //    var logMessage = "Alarm reset successfully.";
            //    logList.Add(logMessage);
            //    //var logMessage = "Threshold set successfully!";
            //}

            //return logList; 
                

        }

        public static DeviceAction Btn_Trigged()
        {

            DeviceActions.ToggleTriggedState();

            if (DeviceActions.GetArmedState() == true)
            {
                //var logMessage = "Device armed successfully!";
                //logList.Add(logMessage);
                return DeviceAction.ArmDevice;
            }
            else if (DeviceActions.GetArmedState() == false)
            {
                //var logMessage = "Device disarmed successfully!";
                //logList.Add(logMessage);
                return DeviceAction.DisarmDevice;
            }
            else if (DeviceActions.GetTriggedState() == true)
            {
                //var logMessage = "Alarm reset successfully.";
                //logList.Add(logMessage);
                return DeviceAction.TriggerDevice;
            }
            else if (DeviceActions.GetTriggedState() == false)
            {
                //var logMessage = "Alarm reset successfully.";
                //logList.Add(logMessage);
                return DeviceAction.ResetDevice;
            }
            else
            {
                return DeviceAction.Error;
            }

            //if (!DeviceActions.GetArmedState())
            //{
            //    //make sure the button does nothing if the device is not armed
            //    var logMessage = "You have to press START first";
            //    return logMessage;
            //}
            //else
            //{
            //    DeviceActions.ToggleTriggedState();
            //    if (VibrationDetector.AlarmTriggered == true)
            //    {
            //        var logMessage = "Alarm triggered successfully!";
            //        return logMessage;
            //    }
            //    else
            //    {
            //        var logMessage = "Alarm reset successfully!";
            //        return logMessage;
            //    }

            //}
        }
    }
}
