
namespace VibrationDetectors.Interfaces
{
    public interface IVDFetchStatusRequest
    {
        int UserId { get; set; }
        DateTime UserPanelActionDate { get; set; }
        int VibrationDetectorId { get; set; }
    }
}