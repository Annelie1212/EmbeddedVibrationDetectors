using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibrationDetectorAPI.Controllers.Models;

//https://localhost:7034/api/VibrationDetectors

namespace VibrationDetectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VibrationDetectorsController : ControllerBase
    {
        private static readonly List<VDChangeValueRequest> _fakeRequests = new List<VDChangeValueRequest>
        {
            new VDChangeValueRequest
            {
                VibrationDetectorId = 1,
                UserPanelAction = "SetThreshold",
                NewValue = 5.5,
                UserId = 101,
                UserPanelActionDate = DateTime.UtcNow.AddMinutes(-15)
            },
            new VDChangeValueRequest
            {
                VibrationDetectorId = 2,
                UserPanelAction = "Calibrate",
                NewValue = 3.2,
                UserId = 102,
                UserPanelActionDate = DateTime.UtcNow.AddMinutes(-10)
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<VDChangeValueRequest>> GetAllRequests()
        {
            return Ok(_fakeRequests);
        }
        [HttpPost]
        public ActionResult<VDChangeValueResponse> CreateRequest([FromBody] VDChangeValueRequest request)
        {

            var fakeResponse = new VDChangeValueResponse()
            {
                VibrationDetectorId = 666,
                RequestSuccessful = true,
                ErrorMessage = "No errors detected!"
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _fakeRequests.Add(request);

            return Ok(fakeResponse);
            //return CreatedAtAction(nameof(GetAllRequests), new { id = request.VibrationDetectorId }, request);
        }



    }
}
