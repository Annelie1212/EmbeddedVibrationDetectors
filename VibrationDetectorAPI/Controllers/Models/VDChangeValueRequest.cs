using System.ComponentModel.DataAnnotations;

namespace VibrationDetectorAPI.Controllers.Models
{
    public class VDChangeValueRequest
    {
        [Required]
        public int VibrationDetectorId { get; set; }

        [Required]
        public string UserPanelAction { get; set; } = string.Empty;

        [Required]
        public double NewValue { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime UserPanelActionDate { get; set; }


    }
}
