using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared; // namespace from your proto
using System.Collections.Generic;

namespace VibrationDetectors.Services
{
    public class VDStatusHandlerService : VDStatusHandler.VDStatusHandlerBase
    {

        public override Task<GetVDStatusResponse> GetVDStatus(GetVDStatusRequest request,ServerCallContext context)
        {
           // TODO: Replace with real logic

            var reply = new GetVDStatusResponse();

            var status1 = new VDStatus
            {
                VibrationDetectorId = request.VibrationDetectorId,
                DeviceName = "Test Device",
                Location = "Test Lab",
                AlarmArmed = true,
                AlarmTriggered = false,
                VibrationLevel = 5,
                VibrationLevelThreshold = 10,
                RequestSuccessful = true,
                ErrorMessage = ""
            };

            var status2 = new VDStatus
            {
                VibrationDetectorId = request.VibrationDetectorId,
                DeviceName = "Test Device",
                Location = "Test Lab 2",
                AlarmArmed = true,
                AlarmTriggered = true,
                VibrationLevel = 2,
                VibrationLevelThreshold = 5,
                RequestSuccessful = true,
                ErrorMessage = "Error! you did bad!"
            };

            List<VDStatus> statuses = new List<VDStatus> { status1, status2 };

            reply.Vdstatuses.AddRange(statuses);

            return Task.FromResult(reply);

        }
    }
}
