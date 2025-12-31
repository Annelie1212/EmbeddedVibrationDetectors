
namespace VibrationDetectors.Models
{
    public class Enumerators
    {
         
        public enum DeviceAction
        {
            ArmDevice = 0,
            DisarmDevice = 4,
            TriggerDevice = 1,
            ResetDevice = 3,
            SetThreshold = 2,
            TriggerFailure = 5,
            Error = 99
        }
    }
}

