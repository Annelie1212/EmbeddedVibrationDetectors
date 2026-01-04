//using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Services;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Models
{
    public class DeviceLog
    {
        public int VibrationDetectorStatusLogId { get; set; }
        public int UserId { get; set; }
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public DateTime ActionLogDateTime { get; set; }

        //string i databasen så man måste konvertera.
        public DeviceAction DeviceAction { get; set; }
        //public bool DeviceActionSuccess { get; set; }

        public int OldUserValue { get; set; }
        public int NewUserValue { get; set; }

        public string Location { get; set; }

        public StatusAndErrorType ErrorMessage { get; set; }

        //Larmad eller inte larmad
        public bool AlarmArmed { get; set; } = false;

        //Alarm utlöst eller inte
        public bool AlarmTriggered { get; set; } = false;

        public int VibrationLevel { get; set; } = 0;

        //Tröskelvärdets maxvärde ska vara 10.
        public int VibrationLevelThreshold { get; set; } = 5;

        public string LogMessage { get; set; }

        public void AddDateToLM(DateTime dt)
        {
            //the @ means that the string is a verbatim string literal.
            //a verbatim string is a string in which escape sequences are not processed.
            //an example of an escape sequence is \n which represents a new line.
            LogMessage += @$"{dt:yyyy-MM-dd HH:mm:ss}";
        }
        public void AddErrorToLM(StatusAndErrorType statusAndErrorType)
        {
            
            switch(statusAndErrorType)
            {
                case StatusAndErrorType.Success:
                    LogMessage += " - Success - ";
                    break;
                case StatusAndErrorType.DeviceIdNotFound:
                    LogMessage += " - Error: Device ID not found - ";
                    break;
                case StatusAndErrorType.FailedToTriggerAlarm:
                    LogMessage += " - Error: Failed to trigger alarm - ";
                    break;
                //case StatusAndStatusAndErrorType.ConnectionFailed:
                //LogMessage += " - Error: Connection failed - ";
                //  break;
                default:
                    LogMessage += " - Error: Unknown error - ";
                    break;
            }
        }

        public void AddMessageToLM(DeviceAction deviceAction, int oldValueInt, int newValueInt)
        {
            string message;
            int deviceActionInt = (int)deviceAction;

            string oldValue;
            string newValue;
            if (deviceActionInt == 0 || deviceActionInt == 1 || deviceActionInt == 3 || deviceActionInt == 4)
            {
                oldValue = (oldValueInt == 1) ? "On" : "Off";
                newValue = (newValueInt == 1) ? "On" : "Off";
                message = $"Changed {deviceAction} from {oldValue} to {newValue} successfully.";
            }
            else if (deviceActionInt == 2)
            {
                message = $"Changed {deviceAction} from {oldValueInt} to {newValueInt} successfully.";
            }

            else
            {
                message = "Annelies unknown action.";

            }
            LogMessage += message;
        }



        public string BuildLogMessage(DateTime dt,
                                      int oldValueInt, 
                                      int newValueInt, 
                                      DeviceAction da, 
                                      StatusAndErrorType statusAndErrorType)
        {

            LogMessage = "";
            //1. Add date
            AddDateToLM(dt);
            //2. status StatusAndErrorType
            AddErrorToLM(statusAndErrorType);
            //3. Add message with details.
            AddMessageToLM(da,oldValueInt,newValueInt);

            return LogMessage;
        }

        //private void LogMessage2(string message, DeviceAction deviceAction)
        //{
            

        //    //try
        //    //{
        //    //    //File.AppendAllText(_logFilePath, line + Environment.NewLine);
        //    //}
        //    //catch
        //    //{

        //    //}



        //    //var logEntry = new DeviceLog
        //    //{

        //    //    ActionLogDateTime = DateTime.Now,
        //    //    DeviceAction = deviceAction,

        //    //    OldUserValue = -1, // not tracked, get from database later.

        //    //    NewUserValue = ActionToValue(deviceAction),

        //    //    UserId = VibrationDetector.UserId,
        //    //    DeviceId = VibrationDetector.DeviceId,
        //    //    DeviceName = DeviceActions.GetDeviceName(),
        //    //    Location = VibrationDetector.Location,
        //    //    AlarmArmed = DeviceActions.GetArmedState(),
        //    //    AlarmTriggered = DeviceActions.GetTriggedState(),
        //    //    VibrationLevel = VibrationDetector.VibrationLevel,
        //    //    VibrationLevelThreshold = VibrationDetector.VibrationLevelThreshold,
        //    //    LogMessage = message,
        //    //};
 
        //}

        //används ej längre. kanske använda senare.
        //public void PopulateDeviceLog(VDFetchStatusResponse sres)
        //{
        //    //DeviceLogId = sr.VibrationDetectorId;
        //    UserId = sres.VibrationDetectorId;
        //    DeviceName = "VibrationDetector";
        //    DeviceId = sres.VibrationDetectorId;
        //    ActionLogDateTime = DateTime.Now;
        //    //TODO Ta från request!!!!
        //    DeviceAction = (DeviceAction)99;
        //    //DeviceActionSuccess = sres.RequestSuccessful;
        //    ErrorMessage = (Enumerators.StatusAndErrorType)Enum.Parse(typeof(Enumerators.ErrorType), sres.ErrorMessage);
        //    AlarmArmed = sres.AlarmArmed;
        //    AlarmTriggered = sres.AlarmTriggered;
        //    VibrationLevel = sres.VibrationLevel;
        //    VibrationLevelThreshold = sres.VibrationLevelThreshold;
        //    BuildLogMessage();
        //}

        public void UpdateDeviceLog(DateTime latestDateTimeStamp,
                            DeviceAction deviceAction,
                            int oldUserValue,
                            int newUserValue,
                            int userId,
                            int deviceId,
                            string deviceName,
                            string location,
                            StatusAndErrorType errorMessage,
                            bool alarmArmed,
                            bool alarmTriggered,
                            int vibrationLevel,
                            int vibrationLevelThreshold,
                            string logMessage)
        {
            //add all input parameters to the properties of the DeviceLog class.
            ActionLogDateTime = latestDateTimeStamp;
            DeviceAction = deviceAction;
            OldUserValue = oldUserValue;
            NewUserValue = newUserValue;
            UserId = userId;
            DeviceId = deviceId;
            DeviceName = deviceName;
            Location = location;
            ErrorMessage = errorMessage;
            AlarmArmed = alarmArmed;
            AlarmTriggered = alarmTriggered;
            VibrationLevel = vibrationLevel;
            VibrationLevelThreshold = vibrationLevelThreshold;
            LogMessage = logMessage;


        }


    }
}
