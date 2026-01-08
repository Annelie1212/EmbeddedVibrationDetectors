using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VibrationDetectors.Models;
using VibrationDetectors.Services;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Models
{

    


    public class DeviceActions
    {

        private DbLogService _dbLogService;

        private static double _pendingSliderValue;
        public static bool _skipSliderAction = false;

        public static ObservableCollection<string>? _eventLog = [];

        public static DateTime LatestDateTimeStamp = DateTime.Now;

        //public static bool RunningState { get; private set; } = false;

        public DeviceActions(DbLogService dbLogService)
        {
            _dbLogService = dbLogService;
        }

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
        public static void Btn_Armed()
        {
            //this method should raise an exception if called!
            throw new Exception("This method cannot be used right now!");


            //DeviceActions.ToggleArmedState();

            //if (DeviceActions.GetTriggedState() == true)
            //{
            //    DeviceActions.ToggleTriggedState();
            //}
        }

        public static void Btn_Trigged()
        {
            //this method should raise an exception if called!
            throw new Exception("This method cannot be used right now!");

            //if (!DeviceActions.GetArmedState())
            //{
            //    //make sure the button does nothing if the device is not armed
            //}
            //else
            //{
            //    DeviceActions.ToggleTriggedState();
            //}
        }

        public static void ThresholdChangedAction(double slider_threshold_value)
        {
            LatestDateTimeStamp = DateTime.Now;


            if (_skipSliderAction)
            {
                _skipSliderAction = false;
                return;
            }

            // Store latest value
            //_pendingSliderValue = Slider_Threshold.Value;
            _pendingSliderValue = slider_threshold_value;


        }


        public DeviceLog SliderDebounceTimer_Tick_Stop()
        {
            //FORTSÄTT HÄR! TODO se till att inte logga innan slider används!!! + dagbok från igår.


            //1. Get old user value before it is overwritten.
            var oldUserValue = VibrationDetector.VibrationLevelThreshold;

            //2. Set threshold level, nothing else!
            DeviceActions.SetThresholdLevel(_pendingSliderValue);

            //3 Get new user value.
            var newUserValue = VibrationDetector.VibrationLevelThreshold;

            var tempDeviceLog = CompleteLogAction(oldUserValue, newUserValue, DeviceAction.SetThreshold, StatusAndErrorType.Success);

            //11. Debug output
            Debug.WriteLine($"Slider committed value: {_pendingSliderValue}");

            _dbLogService.CreateOne(tempDeviceLog);

            return tempDeviceLog;
        }


        public static DeviceLog CompleteLogAction(int oldUserValue, int newUserValue, DeviceAction deviceAction, StatusAndErrorType errorType)
        {
            //Instatiate devicelog object
            DeviceLog deviceLog = new DeviceLog();

            //3.1. Create success log messsage
            string logMessage = deviceLog.BuildLogMessage(LatestDateTimeStamp,
                                                          oldUserValue,
                                                          newUserValue,
                                                          deviceAction,
                                                          errorType);

            //4. add logmessage to eventlog view.
            _eventLog?.Add(logMessage);



            //6. Update devicelog object.
            deviceLog.UpdateDeviceLog(LatestDateTimeStamp,
                            deviceAction,
                            oldUserValue,
                            newUserValue,
                            VibrationDetector.UserId,
                            VibrationDetector.DeviceId,
                            VibrationDetector.DeviceName,
                            VibrationDetector.Location,
                            //StatusAndErrorType.Success,
                            errorType,
                            VibrationDetector.AlarmArmed,
                            VibrationDetector.AlarmTriggered,
                            VibrationDetector.VibrationLevel,
                            VibrationDetector.VibrationLevelThreshold,
                            logMessage
                            );

            return deviceLog;

        }
        public DeviceLog Btn_Armed_Click_Action()
        {
            LatestDateTimeStamp = DateTime.Now;
            StatusAndErrorType errorType = StatusAndErrorType.Success;

            //THIS MUST COME BEFORE DISARMING!!!
            if (DeviceActions.GetTriggedState() == true)
            {
                //DeviceActions.ToggleTriggedState();
                Btn_TriggedState_Action();
            }

            var oldUserValue = VibrationDetector.AlarmArmed;

            //DeviceActions.Btn_Armed();
            DeviceActions.ToggleArmedState();

            var newUserValue = VibrationDetector.AlarmArmed;

            var tempDeviceLog = CompleteLogAction((oldUserValue == true) ? 1 : 0, (newUserValue == true) ? 1 : 0, DeviceAction.ArmDevice, errorType);

            _dbLogService.CreateOne(tempDeviceLog);

            return tempDeviceLog;
        }
        public DeviceLog Btn_TriggedState_Action()
        {
            //Better to update date here again!
            LatestDateTimeStamp = DateTime.Now;
            StatusAndErrorType errorType = StatusAndErrorType.Success;

            var oldUserValue = VibrationDetector.AlarmTriggered;

            //DeviceActions.Btn_Trigged();
            if (!DeviceActions.GetArmedState())
            {
                //make sure the button does nothing if the device is not armed
                errorType = StatusAndErrorType.FailedToTriggerAlarm;
            }
            else
            {
                DeviceActions.ToggleTriggedState();
            }

            var newUserValue = VibrationDetector.AlarmTriggered;

            var tempDeviceLog = CompleteLogAction((oldUserValue == true) ? 1 : 0, (newUserValue == true) ? 1 : 0, DeviceAction.TriggerDevice, errorType);

            _dbLogService.CreateOne(tempDeviceLog);

            return tempDeviceLog;
        }
    }
}
