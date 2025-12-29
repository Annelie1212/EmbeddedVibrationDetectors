using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VibrationDetectorAPI.Controllers.Models;
//using VibrationDetectors;
//using VibrationDetectors.Models;
//using VibrationDetectors.Services;

namespace VibrationDetectorAPI.Controllers
{

    //MainWindow _mainWindow = new MainWindow();

    [Route("api/[controller]")]
    [ApiController]
    public class VibrationDetectorsSyncController : ControllerBase
    {

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


        [HttpPost]
        public ActionResult<FakeResponse> FetchVDStatus([FromBody] FakeRequest request)
        {
            //LITET TEST FÖR GRPC-KOMMUNIKATION
            //Console.WriteLine("Starting gRPC test client from VibrationDetectorsSyncController...");
            //var testClient = new VibrationDetectorAPI.TestClientGrpc();
            //testClient.Testserver();

            //Console.WriteLine("gRPC test client completed.");


            //istället för, ska vi nu skicka request vidare till VibrationDetectors serviceklass som hanterar logiken.
            //var fakeResponse = new VDChangeValueResponse()
            //{
            //    VibrationDetectorId = 666,
            //    RequestSuccessful = true,
            //    ErrorMessage = "No errors detected!"
            //};

            //var vdService = new VDStatusService();
            //var response = vdService.FetchVibrationDetectorStatus(request);
            var response = new FakeResponse()
            {
                VibrationDetectorId = 21,
                DeviceName = "fakename",
                Location = "fake location",
                AlarmArmed = true,
                AlarmTriggered = true,
                VibrationLevel = 1,
                VibrationLevelThreshold = 2,
                RequestSuccessful = true,
                ErrorMessage = "No fake errors detected!"
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_fakeRequests.Add(request);




            //return Ok(fakeResponse);
            return Ok(response);
            //return CreatedAtAction(nameof(GetAllRequests), new { id = request.VibrationDetectorId }, request);
        }
    }
}
