using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace VibrationDetectors.Models
{
    public class VDFetchStatusResponse
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
}
