using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibrationDetectorAPI.Controllers.Models;
using VibrationDetectors;
using VibrationDetectors.Models;
using VibrationDetectors.Services;

namespace VibrationDetectorAPI.Controllers
{

    //MainWindow _mainWindow = new MainWindow();

    [Route("api/[controller]")]
    [ApiController]
    public class VibrationDetectorsSyncController : ControllerBase
    {
        [HttpPost]
        public ActionResult<VDFetchStatusResponse> FetchVDStatus([FromBody] VDFetchStatusRequest request)
        {
            //istället för, ska vi nu skicka request vidare till VibrationDetectors serviceklass som hanterar logiken.
            //var fakeResponse = new VDChangeValueResponse()
            //{
            //    VibrationDetectorId = 666,
            //    RequestSuccessful = true,
            //    ErrorMessage = "No errors detected!"
            //};

            var vdService = new VDServerService();

            var response = vdService.FetchVibrationDetectorStatus(request);

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
