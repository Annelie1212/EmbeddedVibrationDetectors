using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationDetectors.Interfaces;

namespace VibrationDetectors.Models
{
    public class VDFetchStatusRequest : IVDFetchStatusRequest
    {
        [Required]
        public int VibrationDetectorId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime UserPanelActionDate { get; set; }

    }
}
