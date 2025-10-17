using System.ComponentModel.DataAnnotations;
using VibrationDetectors.Services;
using static VibrationDetectors.Models.Enumerators;

namespace VibrationDetectors.Models
{
    public class VDChangeValueRequest
    {
        [Required]
        public int VibrationDetectorId { get; set; }

        [Required]
        public DeviceAction UserPanelAction { get; set; } = (DeviceAction)99;

        [Required]
        public double NewValue { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime UserPanelActionDate { get; set; }


    }
}
