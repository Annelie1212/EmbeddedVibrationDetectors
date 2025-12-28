using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VibrationDetectors.Interfaces;
using VibrationDetectors.Models;
using static VibrationDetectors.Models.Enumerators;


namespace VibrationDetectors.Services
{
    public class VDServerService
    {
        //private static MainWindow _mainWindow;

        //private readonly MainWindow _mainWindow;
        //public VDServerService()
        //{
        //    //_mainWindow = mainWindow;
        //}
        //Denna metod anropas från VibrationDetectorsSyncController.



        public VDServerService()
        {

        }

        public VDFetchStatusResponse FetchVibrationDetectorStatus(VDFetchStatusRequest request)
        {
            //Här ska vi implementera logiken för att hämta status från en vibrationsdetektor.
            //Just nu returnerar vi bara null.
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }
            //var VDFetchStatusRequest = new VDFetchStatusRequest()
            //{
            //    VibrationDetectorId = request.VibrationDetectorId,
            //    UserId = request.UserId,
            //    UserPanelActionDate = request.UserPanelActionDate
            //};

            if (request.VibrationDetectorId == VibrationDetector.DeviceId)
            {
                if (request.UserId == VibrationDetector.UserId)
                {
                    var response = new VDFetchStatusResponse()
                    {
                        VibrationDetectorId = VibrationDetector.DeviceId,
                        DeviceName = VibrationDetector.DeviceName,
                        Location = VibrationDetector.Location,
                        AlarmArmed = VibrationDetector.AlarmArmed,
                        AlarmTriggered = VibrationDetector.AlarmTriggered,
                        VibrationLevel = VibrationDetector.VibrationLevel,
                        VibrationLevelThreshold = VibrationDetector.VibrationLevelThreshold,
                        RequestSuccessful = true,
                        ErrorMessage = "No errors detected!"
                    };
                    return response;
                }
                else
                {
                    var response = new VDFetchStatusResponse()
                    {
                        VibrationDetectorId = 0,
                        DeviceName = string.Empty,
                        Location = string.Empty,
                        AlarmArmed = false,
                        AlarmTriggered = false,
                        VibrationLevel = 0,
                        VibrationLevelThreshold = 0,
                        RequestSuccessful = false,
                        ErrorMessage = "UserId is incorrect."
                    };
                    return response;
                }
            }
            else
            {
                var response = new VDFetchStatusResponse()
                {
                    VibrationDetectorId = 0,
                    DeviceName = string.Empty,
                    Location = string.Empty,
                    AlarmArmed = false,
                    AlarmTriggered = false,
                    VibrationLevel = 0,
                    VibrationLevelThreshold = 0,
                    RequestSuccessful = false,
                    ErrorMessage = "Vibration Detector not found or UserId is incorrect."
                };

                return response;
            }
        }
        public VDChangeValueResponse SetVibrationDetectorStatus(VDChangeValueRequest request)
        {
            //Här ska vi implementera logiken för att ändra värden på en vibrationsdetektor.
            //Just nu returnerar vi bara null.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            //var VDChangeValueRequest = new VDChangeValueRequest()
            //{
            //    VibrationDetectorId = request.VibrationDetectorId,
            //    UserPanelAction = request.UserPanelAction,
            //    NewValue = request.NewValue,
            //    UserId = request.UserId,
            //    UserPanelActionDate = request.UserPanelActionDate
            //};




            if (request.VibrationDetectorId == VibrationDetector.DeviceId)
            {
                if (request.UserId == VibrationDetector.UserId)
                {
                    if (request.UserPanelAction == DeviceAction.SetThreshold)
                    {
                        VibrationDetector.VibrationLevelThreshold = (int)request.NewValue;

                        //TODO.
                        //Logik för att updatera view i WPF-appen

                        var response = new VDChangeValueResponse()
                        {
                            VibrationDetectorId = VibrationDetector.DeviceId,
                            RequestSuccessful = true,
                            ErrorMessage = "No errors detected!"
                        };
                        return response;
                    }
                    else if (request.UserPanelAction == DeviceAction.ArmDevice)
                    {

                        VibrationDetector.Btn_Armed();
                        
                        var response = new VDChangeValueResponse()
                        {
                            VibrationDetectorId = VibrationDetector.DeviceId,
                            RequestSuccessful = true,
                            ErrorMessage = "No errors detected!"
                        };

                        return response;
                    }
                    else if (request.UserPanelAction == DeviceAction.TriggerDevice)
                    {
                        VibrationDetector.Btn_Trigged();

                        var response = new VDChangeValueResponse()
                        {
                            VibrationDetectorId = VibrationDetector.DeviceId,
                            RequestSuccessful = true,
                            ErrorMessage = "No errors detected!"
                        };

                        return response;
                    }
                    else if (request.UserPanelAction == DeviceAction.Error)
                    {
                        return null;
                    }
                    else
                    {
                        var response = new VDChangeValueResponse()
                        {
                            VibrationDetectorId = 0,
                            RequestSuccessful = false,
                            ErrorMessage = "UserPanelAction is invalid."
                        };
                        return response;
                    }
                }
                else
                {
                    var response = new VDChangeValueResponse()
                    {
                        VibrationDetectorId = 0,
                        RequestSuccessful = false,
                        ErrorMessage = "UserId is incorrect."
                    };
                    return response;
                }
            }
            else
            {
                var response = new VDChangeValueResponse()
                {
                    VibrationDetectorId = 0,
                    RequestSuccessful = false,
                    ErrorMessage = "Vibration Detector not found."

                };
                return response;
            }
        }
    }
}
