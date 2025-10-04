using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Interfaces;
using VibrationDetectors.Models;

namespace VibrationDetectors.Services
{
    public class VDServerService
    {
        public VDServerService()
        {
        }
        //Denna metod anropas från VibrationDetectorsSyncController.
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

    }
}
