using GrpcShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using VibrationDetectorAPI.Controllers.Models;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using static VibrationDetectorAPI.Controllers.VibrationDetectorsSyncController;
//using VibrationDetectors;
//using VibrationDetectors.Models;
//using VibrationDetectors.Services;

using Google.Protobuf.WellKnownTypes;

//http://localhost:7034/api/VibrationDetectors

namespace VibrationDetectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VibrationDetectorsController : ControllerBase
    {
        private readonly VDStatusHandler.VDStatusHandlerClient _client;

        public VibrationDetectorsController(VDStatusHandler.VDStatusHandlerClient client)
        {
            _client = client;
        }
        public class FakeResponse
        {
            [Required]
            public int VibrationDetectorId { get; set; }
            [Required]
            public string DeviceName { get; set; }
            [Required]
            public string Location { get; set; }

            //Larmad eller inte larmad
            [Required]
            public bool AlarmArmed { get; set; }

            //Alarm utlöst eller inte   
            [Required]
            public bool AlarmTriggered { get; set; }
            [Required]
            public int VibrationLevel { get; set; }
            [Required]
            public int VibrationLevelThreshold { get; set; }
            [Required]
            public bool RequestSuccessful { get; set; }
            [Required]
            public string ErrorMessage { get; set; }
        }

        public class FakeRequest
        {
            [Required]
            public int VibrationDetectorId { get; set; }
            [Required]
            public int UserId { get; set; }
            [Required]
            public DateTime UserPanelActionDate { get; set; }
        }

        public class ListApiRequest
        {
            public int VibrationDetectorId { get; set; }
            public int UserId { get; set; }

            public DateTime UserPanelActionDate { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ListApiRequest request)
        {
            var tempDate = Google.Protobuf.WellKnownTypes.Timestamp
                        .FromDateTime(request.UserPanelActionDate.ToUniversalTime());

            var grpcRequest = new GetVDStatusRequest
            {
                VibrationDetectorId = request.VibrationDetectorId,
                UserId = request.UserId,
                UserPanelActionDate = tempDate
            };

            //var grpcRequest = request;

            var response = await _client.GetVDStatusAsync(grpcRequest);

            return Ok(response.Vdstatuses.Select(s => new { s.VibrationDetectorId, s.DeviceName, s.Location, s.AlarmArmed, s.AlarmTriggered, s.VibrationLevel, s.VibrationLevelThreshold, s.RequestSuccessful, s.ErrorMessage }));


            //LITET TEST FÖR GRPC-KOMMUNIKATION
            //Console.WriteLine("TestGRPC:Starting gRPC test client from VibrationDetectorsSyncController...");
            //var testClient = new VibrationDetectorAPI.TestClientGrpc();
            //testClient.Testserver();

            //Console.WriteLine("TestGRPC:gRPC test client completed.");

            //var response = new FakeResponse()
            //{
            //    VibrationDetectorId = 21,
            //    DeviceName = "fakename",
            //    Location = "fake location",
            //    AlarmArmed = true,
            //    AlarmTriggered = true,
            //    VibrationLevel = 1,
            //    VibrationLevelThreshold = 2,
            //    RequestSuccessful = true,
            //    ErrorMessage = "No fake errors detected!"
            //};

            //return Ok(response);
        }

        //[HttpPost]
        //public ActionResult<VDChangeValueResponse> SetVDStatus([FromBody] VDChangeValueRequest request)
        //{

        //    var vdService = new VDServerService();

        //    var response = vdService.SetVibrationDetectorStatus(request);
           
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
            
        //    return Ok(response);
        //    //return CreatedAtAction(nameof(GetAllRequests), new { id = request.VibrationDetectorId }, request);
        //}



    }
}
