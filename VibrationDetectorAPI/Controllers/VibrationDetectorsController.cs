using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibrationDetectorAPI.Controllers.Models;
//using VibrationDetectors;
//using VibrationDetectors.Models;
//using VibrationDetectors.Services;

//https://localhost:7034/api/VibrationDetectors

namespace VibrationDetectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VibrationDetectorsController : ControllerBase
    {

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
